/*
 * @version   : 2.5.0
 * @author    : Object.NET, Inc. http://object.net/
 * @date      : 2014-10-20
 * @copyright : Copyright (c) 2008-2015, Object.NET, Inc. (http://object.net/). All rights reserved.
 * @license   : Apache License, Version 2.0, See LICENSE in root 
 * @website   : http://object.net/
 */

namespace Object.Net.Utilities.Inflatr
{
    /// <summary>
    /// 
    /// </summary>
    public class Options
    {
        private int wrap = 80;

        /// <summary>
        /// 
        /// </summary>
        public int Wrap
        {
            get
            {
                return this.wrap;
            }
            set
            {
                this.wrap = value;
            }
        }

        private string indent = "  ";
        
        /// <summary>
        /// 
        /// </summary>
        public string Indent
        {
            get
            {
                return this.indent;
            }
            set
            {
                this.indent = value;
            }
        }

        private int level = 0;
        
        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Options Clone()
        {
            return new Options
            {
                Indent = this.Indent,
                Wrap = this.Wrap,
                Level = this.Level
            };
        }
    }
}
