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
 * 文件名称: PinyinOptions.cs
 * 文件说明: 控件基类
 * 当前版本: V1.0.0
 * 创建日期: 2021-10-13 17:36:47
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chan.Pinyin
{
    public class PinyinOptions
    {
        private static readonly PinyinOptions pinyinOptions;
#pragma warning disable S3963 // "static" fields should be initialized inline
        static PinyinOptions()
        {
            pinyinOptions = new PinyinOptions();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline
        public static PinyinOptions Default
        {
            get
            {
                return pinyinOptions;
            }
        }
        /// <summary>
        /// 拼音转换后，显示声调方式
        /// </summary>
        public PinyinEnum PinyinEnum { get; set; } = PinyinEnum.None;
        /// <summary>
        /// 大小写，默认值为<code>false</code>,即小写
        /// </summary>
        public bool CaseSensitive { get; set; } = false;
        /// <summary>
        /// 在<code>CaseSensitive</code>表示为<code>false</code>时，首字母是否大写,默认小写
        /// </summary>
        public bool FirstCapitalized { get; set; } = false;
        /// <summary>
        /// 分隔符
        /// <para>
        /// 默认在拼音转换后，以空格方式作为拼音之间的间隔
        /// </para>
        /// </summary>
        public string Separator { get; set; } = " ";
        /// <summary>
        /// 仅显示首字母
        /// </summary>
        public bool Initials { get; set; } = false;
    }

    public class PolysyllabicOptions
    { 
        public string[] Polyphonic { get; set; }

        public int Offset { get; set; }

        public int Length { get; set; }
    }
}
