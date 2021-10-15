/******************************************************************************
 * FineUI 开源控件库、工具类库、扩展类库、多页面开发框架。
 * CopyRight (C) 2021-2021 chenqiang(陈强).
 * QQ群： QQ：867118177 EMail：ch_lhp@163.Com
 *
 * Blog:   
 * Gitee:  
 * GitHub: 
 *
 * FineUI.dll can be used for free under the GPL-3.0 license.
 * If you use this code, please keep this note.
 * 如果您使用此代码，请保留此说明。
 ******************************************************************************
 * 文件名称: PinyinEnum.cs
 * 文件说明: 控件基类
 * 当前版本: V1.0.0
 * 创建日期: 2021-10-13 17:31:41
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chan.Pinyin
{
    /// <summary>
    /// 拼音输出类型
    /// </summary>
    public enum PinyinEnum
    {
        /// <summary>
        /// 输出的拼音带声调
        /// </summary>
        Tone = 0,
        /// <summary>
        /// 输出拼音以数字1、2、3、4表示音调，跟随在拼音之后
        /// </summary>
        ToneRightNum = 1,
        /// <summary>
        /// 单纯拼音
        /// </summary>
        None = 2
    }

    public enum PloysyType
    {
        /// <summary>
        /// 忽略处理的文字，如数字，英文等
        /// </summary>
        Ignore = 0,
        /// <summary>
        /// 单音节的汉字
        /// </summary>
        Monosyllable = 1,
        /// <summary>
        /// 多音字的汉字
        /// </summary>
        Polyphonic = 2

    }
}
