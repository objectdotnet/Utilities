/*
 * @version   : 2.5.0
 * @author    : Object.NET, Inc. http://object.net/
 * @date      : 2014-10-20
 * @copyright : Copyright (c) 2008-2015, Object.NET, Inc. (http://object.net/). All rights reserved.
 * @license   : Apache License, Version 2.0, See LICENSE in root 
 * @website   : http://object.net/
 */

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Object.Net.Utilities.Inflatr
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Base
    {
        private Options options;

        /// <summary>
        /// 
        /// </summary>
        protected int index = 0;
        
        /// <summary>
        /// 
        /// </summary>
        protected string input = "";
        
        /// <summary>
        /// 
        /// </summary>
        protected int c = 0;
        
        /// <summary>
        /// 
        /// </summary>
        protected StringBuilder r;

        /// <summary>
        /// 
        /// </summary>
        public Base()
        {
            this.options = new Options();
            this.r = this.Indent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public Base(Options options) : this()
        {
            if (options != null)
            {
                this.options = options.Clone();
                this.r = this.Indent();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Options Options
        {
            get
            {
                return this.options;
            }
        }

        /// <summary>
        /// Inflate the string
        /// </summary>
        /// <param name="input">compressed string</param>
        /// <returns>Inflated string</returns>
        public abstract string Inflate(string input);

        private Regex escapeRe = new Regex(@"([.*+?^${}()|[\]\/\\])", RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        protected string Escape(string pattern)
        {
            return escapeRe.Replace(pattern, "\\$1");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected StringBuilder Indent()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < this.options.Level; i++)
            {
                sb.Append(this.options.Indent);
            }

            return sb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        protected int LastIndexOf(Regex pattern)
        {
            return this.LastIndexOf(pattern, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected int LastIndexOf(Regex pattern, string input)
        {
            if (input.IsEmpty())
            {
                input = this.input;
            }

            for (int i = this.index; i >= 0; i--)
            {
                bool match = pattern.IsMatch(input, i);

                if (match)
                { 
                    return i; 
                }
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        protected bool After(Regex pattern, Regex start)
        {
            int i = this.LastIndexOf(start);
            
            if (i > 0)
            {
                return pattern.IsMatch(this.input, i);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strings"></param>
        protected void Append(params string[] strings)
        {
            foreach (string item in strings)
            {
                if (item != null)
                {
                    this.r.Append(item);
                    this.c += item.Length;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pat"></param>
        /// <returns></returns>
        protected string Peek(string pat)
        {
            Match m = new Regex("^" + pat, RegexOptions.Multiline).Match(this.input, this.index);

            return m.Success ? m.Groups[0].Value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pat"></param>
        /// <returns></returns>
        protected string Scan(string pat)
        {
            string m = this.Peek(pat);

            if (m.IsNotEmpty())
            {
                this.index += m.Length;
             
                return m;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool NextChar()
        {
            this.Append(this.Scan("(\n|.)"));

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool WhiteSpace()
        {
            if (this.Scan("\\s+").IsNotEmpty())
            {
                this.Append(" ");

                return true;
            }

            return false;
        }

        private Regex newLineRe1 = new Regex("\\Z", RegexOptions.Compiled);
        private Regex newLineRe2 = new Regex("\\S", RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string NewLine()
        {
            int x = this.LastIndexOf(this.newLineRe1, this.r.ToString());
            int y = this.LastIndexOf(this.newLineRe2, this.r.ToString());

            if ( x < 0 || y < 0 || x < y )
            {
                this.c = 0; 

                return "\n" + this.Indent().ToString();      
            } 
            else 
            { 
                return ""; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pat"></param>
        /// <returns></returns>
        protected string ScanUntil(string pat)
        {
            Regex re = new Regex("^((?:(?!"+pat+").)*)"+pat, RegexOptions.Multiline);
            Match m = re.Match(this.input, this.index);

            if (m.Success)
            {
                this.index += m.Groups[0].Length;

                return m.Groups[1].Value;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected string Between(string start)
        {
            return this.Between(start, start);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        protected string Between(string start, string finish)
        {
            string m = this.Scan(start);

            if (m.IsNotEmpty())
            {
                m = this.ScanUntil(finish);

                if (m.IsNotEmpty())
                {
                    return m;
                }
                else
                {
                    throw new Exception("Between: unmatched " + finish + " after " + start + ".");
                }
            }

            return null;
        }
    }
}
