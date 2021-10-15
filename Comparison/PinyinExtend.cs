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
 * 文件名称: IPinyinSet.cs
 * 文件说明: 控件基类
 * 当前版本: V1.0.0
 * 创建日期: 2021-10-13 17:24:31
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chan.Pinyin.Comparison
{
    public abstract class PinyinExtend
    {
        /// <summary>
        /// 获取拼音字典扩展集合
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, string> GetPinyinSetExtend();
        /// <summary>
        /// 获取繁简字典扩展集合
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, string> GetComplexSetExtend();
        /// <summary>
        /// 获取多音词典扩展集合
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, string> GetPolysyllabicSetExtend();
    }
}
