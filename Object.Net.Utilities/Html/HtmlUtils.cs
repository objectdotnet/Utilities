/*
 * @version   : 2.5.0
 * @author    : Object.NET, Inc. http://object.net/
 * @date      : 2014-10-20
 * @copyright : Copyright (c) 2008-2015, Object.NET, Inc. (http://object.net/). All rights reserved.
 * @license   : Apache License, Version 2.0, See LICENSE in root 
 * @website   : http://object.net/
 */

using System.Text.RegularExpressions;

namespace Object.Net.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class HtmlUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string StripWhitespaceChars(this string html)
        {
            return Regex.Replace(html, "[\n\r\t]", "");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string StripExtraSpaces(this string html)
        {
            return Regex.Replace(html, @"\s+", " ");
        }
    }
}