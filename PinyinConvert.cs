using Chan.Pinyin.Comparison;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chan.Pinyin
{
    /// <summary>
    /// 拼音
    /// </summary>
    public static class PinyinConvert
    {
        /// <summary>
        /// 汉字转换为拼音
        /// </summary>
        /// <param name="chinese">需要转为拼音的汉子或句子</param>
        /// <param name="options">拼音转换配置项</param>
        /// <returns></returns>
        public static string ConvertToPinyin(string chinese, PinyinOptions options = null)
        {
            if (string.IsNullOrEmpty(chinese))
            {
                return string.Empty;
            }
            if (options == null)
            {
                options = PinyinOptions.Default;
            }
            if (options.Separator == null)
            {
                options.Separator = string.Empty;
            }
            List<PloysyType> ploysList = new List<PloysyType>();
            var lastPolyphoneIndex = 0;
            var len = chinese.Length;
            var result = new List<string>();
            for (int i = 0; i < len; i++)
            {
                var word = chinese[i];
                var unicode = (int)word;
                //http://www.unicode.org/charts/PDF/U4E00.pdf unicode定义的汉字范围，但这里只是用了0x4E00~0x9FA5之间的汉字
                //如果unicode在符号，英文，数字或其他语系，则直接返回
                if (unicode > 0x9FA5 || unicode < 0x4E00)
                {
                    result.Add(word.ToString());
                    ploysList.Add(PloysyType.Ignore);
                }
                else
                {
                    //如果是支持的中文，则获取汉字的所有拼音
                    var pinyinArray = GetPinyin(word.ToString());
                    if (pinyinArray.Length == 1)
                    {
                        //单音字
                        result.Add(pinyinArray[0]);
                        ploysList.Add(PloysyType.Monosyllable);
                    }
                    else if (pinyinArray.Length > 1)
                    {
                        //多音字，需要进行特殊处理
                        var data = GetPolyphoneWord(chinese, word, i, lastPolyphoneIndex);
                        if (data != null)
                        {
                            for (var k = 0; k < data.Polyphonic.Length; k++)
                            {
                                result[i - data.Offset + k] = data.Polyphonic[k];
                                ploysList[i - data.Offset + k] = PloysyType.Polyphonic;
                            }
                            //修正偏移，有可能当前字是词组中非第一个字
                            i = i - data.Offset + data.Polyphonic.Length - 1;
                            //最后处理过的多音字位置，以防止一个多音字词组有多个多音字，例如患难与共，难和共都是多音字
                            lastPolyphoneIndex = i;
                        }
                        else
                        {
                            //没有找到多音字的词组，默认使用第一个发音
                            result.Add(pinyinArray[0]);
                            ploysList.Add(PloysyType.Monosyllable);
                        }
                    }
                    else
                    {
                        //未发现
                        result.Add(pinyinArray[0]);
                        ploysList.Add(PloysyType.Ignore);
                    }
                }
            }
            return MergePinyin(result, ploysList, options);
        }

        /// <summary>
        /// 汉字繁简转换
        /// </summary>
        /// <param name="chinese"></param>
        /// <param name="toComplex"></param>
        /// <returns></returns>
        public static string ConvertToComplex(string chinese, bool toComplex = true)
        {
            if (string.IsNullOrEmpty(chinese))
            {
                return string.Empty;
            }
            int len = chinese.Length;
            StringBuilder transBuilder = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                var word = chinese[i];
                transBuilder.Append(toComplex ? GetComplexWord(word) : GetSimpleWord(word));
            }
            return transBuilder.ToString();
        }
        /// <summary>
        /// 将处理好的的汉字拼音合并
        /// </summary>
        /// <param name="result"></param>
        /// <param name="types"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private static string MergePinyin(List<string> result, List<PloysyType> types, PinyinOptions options)
        {
            //定义无声调的元音字母
            var vowels = "aeiouv";
            //定义带有掉的元音字母
            var toneVowels = "āáǎàēéěèīíǐìōóǒòūúǔùǖǘǚǜ";
            StringBuilder pinyinBuilder = new StringBuilder();
            //循环的上一次是否添加分隔符
            bool isPrevAddSeparator = false;
            for (var i = 0; i < result.Count; i++)
            {
                var wordPinyin = result[i];
                var ploysyType = types[i];
                StringBuilder wordBuilder = new StringBuilder();
                //如果是拼音或者多音字
                if (ploysyType == PloysyType.Monosyllable || ploysyType == PloysyType.Polyphonic)
                {
                    //如需要数字声调或者无声调
                    if (options.PinyinEnum == PinyinEnum.ToneRightNum || options.PinyinEnum == PinyinEnum.None)
                    {
                        var tone = -1;//音调数字形式
                        foreach (var vowel in wordPinyin)
                        {
                            var k = -1;
                            //寻找在有声调声母中的位置
                            if ((k = toneVowels.IndexOf(vowel)) > -1)
                            {
                                tone = (k % 4);
                                //计算当前声母在无音调声母的位置
                                var pos = k / 4;
                                wordBuilder.Append(vowels[pos]);
                            }
                            else
                            {
                                //原样
                                wordBuilder.Append(vowel);
                            }
                        }
                        //如果是带音调数字形式，则将音调添加到末尾
                        wordBuilder.Append((options.PinyinEnum == PinyinEnum.ToneRightNum ? (tone + 1) + string.Empty : string.Empty));
                    }
                    else
                    {
                        wordBuilder.Append(wordPinyin);
                    }
                    if (options.CaseSensitive)
                    {
                        string value = wordBuilder.ToString();
                        wordBuilder.Clear();
                        wordBuilder.Append(value.ToUpper());
                    }
                    else if (options.FirstCapitalized)
                    {
                        string value = wordBuilder.ToString();
                        wordBuilder.Clear();
                        wordBuilder.Append(Capitalize(value));
                    }
                    if (options.Initials)
                    {
                        var initials = wordBuilder.ToString().First().ToString();
                        wordBuilder.Clear();
                        wordBuilder.Append(initials);
                    }
                    if (!isPrevAddSeparator)
                    {
                        pinyinBuilder.Append(options.Separator);
                    }
                    pinyinBuilder.Append(wordBuilder).Append(options.Separator);
                    isPrevAddSeparator = true;
                }
                else
                {
                    //如果不需要处理的非拼音
                    pinyinBuilder.Append(wordPinyin);
                    isPrevAddSeparator = false;
                }

            }
            return pinyinBuilder.ToString();
        }
        /// <summary>
        /// 单个汉字拼音，进行首字母大写
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private static string Capitalize(string py)
        {
            if (py.Length > 0)
            {
                var first = py.First().ToString().ToUpper();
                var spare = py.Substring(1, py.Length);
                return first + spare;
            }
            else
            {
                return py;
            }
        }

        /// <summary>
        ///获取一个汉字的所有拼音
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private static string[] GetPinyin(string word)
        {
            var pinyinSeparator = ","; // 拼音分隔符

            if (!PinyinSet.Dictionary.ContainsKey(word))
            {//如果不存在拼音
                return new string[0];
            }
            var pinyin = PinyinSet.Dictionary[word];
            var pysArray = pinyin.Split(new[] { pinyinSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return pysArray;
        }
        /// <summary>
        /// 获取多音字词，返回时返回替换起始位置和结束位置
        /// </summary>
        /// <param name="words">words 句子 </param>
        /// <param name="current">当前字</param>
        /// <param name="pos">当前汉字的位置</param>
        /// <param name="lastPolyphoneIndex"></param>
        /// <param name="pinyinEnum">拼音样式 0-声母带音调，1-音调在最后，2-无音调，默认值0</param>
        /// <returns></returns>
        private static PolysyllabicOptions GetPolyphoneWord(string words, char current, int pos, int lastPolyphoneIndex)
        {
            var pinyinSeparator = ","; // 拼音分隔符
            var results = new List<PolysyllabicOptions>();
            var maxMatchLen = 0;
            foreach (var w in PolysyllabicSet.Dictionary.Keys)
            {
                var len = w.Length;
                var beginPos = pos - len;
                beginPos = Math.Max(lastPolyphoneIndex, beginPos);
                var endPos = Math.Min(pos + len, words.Length);
                var temp = words.Skip(beginPos).Take(endPos - beginPos).ToString();
                var index = -1;
                if ((index = temp.IndexOf(w)) > -1)
                {
                    if (len > maxMatchLen)
                    {
                        maxMatchLen = len;
                    }
                    //当前汉字在多音字词组的偏移位置，用于修正词组的替换
                    var offset = w.IndexOf(current);
                    var data = new PolysyllabicOptions { Polyphonic = PolysyllabicSet.Dictionary[w].Split(new[] { pinyinSeparator }, StringSplitOptions.RemoveEmptyEntries), Offset = offset, Length = len };
                    results.Add(data);
                }
            }
            if (results.Count == 1)
            {
                return results[0];
            }
            else if (results.Count > 1)
            {
                //如果存在多个匹配的多音字词组，以最大匹配项为最佳答案,例如词库中有'中国人'和'中国',最理想的答案应该是最大匹配
                for (var i = 0; i < results.Count; i++)
                {
                    if (results[i].Length == maxMatchLen)
                    {
                        return results[i];
                    }
                }
            }
            return null;
        }

        private static string GetSimpleWord(char word)
        {
            string complexWord = word.ToString();
            if (ComplexSet.Dictionary.ContainsKey(complexWord))
            {
                return ComplexSet.Dictionary[complexWord];
            }
            return complexWord;
        }
        private static string GetComplexWord(char word)
        {
            string simpleWord = word.ToString();
            if (ComplexSet.Dictionary.ContainsValue(simpleWord))
            {
                return ComplexSet.Dictionary.First(s => s.Value == simpleWord).Key;
            }
            return simpleWord;
        }
    }
}
