/*
 * @version   : 2.5.0
 * @author    : Object.NET, Inc. http://object.net/
 * @date      : 2014-10-20
 * @copyright : Copyright (c) 2008-2015, Object.NET, Inc. (http://object.net/). All rights reserved.
 * @license   : Apache License, Version 2.0, See LICENSE in root 
 * @website   : http://object.net/
 */

using System.Text.RegularExpressions;

namespace Object.Net.Utilities.Inflatr
{
    /// <summary>
    /// 
    /// </summary>
    public class Javascript : Base
    {
        /// <summary>
        /// 
        /// </summary>
        public const string KEYWORDS = "(function|continue|default|finally|export|return|switch|import|delete|throw|const|while|catch|break|void|case|with|this|else|for|try|var|new|do|in|if)\\W";

        /// <summary>
        /// 
        /// </summary>
        public const string OPERATORS = "(instanceof|typeof|>>>=|===|<<=|>>=|>>>|!==|!=|>=|\\*=|<=|\\+\\+|\\-=|==|&&|>>|<<|/=|\\|\\||\\+=|\\^=|\\|=|&=|\\-\\-|\\||&|\\^|>|<|!|=|\\?|:|%|/|\\*|\\-|\\+)";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override string Inflate(string input)
        {
            this.input = input;

            while ( this.index < this.input.Length ) 
            {
              bool r = this.Comment() || this.String() || this.Regex() || this.Operator() || 
              this.Keyword() ||this.OpenBlock() || this.CloseBlock() || this.Comma() || 
              this.Parens() || this.Eos() || this.Eol() || this.NextChar();
            }

            return new Regex("\\s*$").Replace(this.r.ToString(), "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool Comment()
        {
            string m = this.Between("//", "\n");

            if (m.IsNotEmpty())
            {
                this.Append("//", m, this.NewLine());

                return false;
            }
            else
            {
                m = this.Between(this.Escape("/*"), this.Escape("*/"));

                if (m.IsNotEmpty())
                {
                    this.Append("/*", m, "*/");

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool String()
        {
            string m = this.Between("'");

            if (m.IsNotEmpty())
            {
                this.Append("'" + m + "'");

                return true;
            }
            else
            {
                m = this.Between("\"");

                if (m.IsNotEmpty())
                {
                    this.Append("\"" + m + "\"");

                    return true;
                }
            }

            return false;
        }

        private Regex afterPatternRegex = new Regex("^(\\d|\\w|\\$|_\\))", RegexOptions.Compiled);
        private Regex afterStartRegex = new Regex("(\\:|\\S)", RegexOptions.Compiled);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool Regex()
        {
            if (!this.After(this.afterPatternRegex, this.afterStartRegex))
            {
                string m = this.Between("/");

                if (m.IsNotEmpty())
                {
                    this.Append("/"+m+"/");

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool Operator()
        {
            string m = this.Scan(Javascript.OPERATORS);

            if (m.IsNotEmpty())
            {
                this.Append(" " + m + " ");

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool Keyword()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool OpenBlock()
        {
            if (this.Scan("\\{").IsNotEmpty())
            {
                this.Options.Level += 1; 
                this.Append(" {", this.NewLine() );

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool CloseBlock()
        {
            if(this.Scan("\\}").IsNotEmpty())
            {
                this.Options.Level -= 1; 
                this.Append( this.NewLine(), "}" );

                if (this.Peek("[;,\\}]").IsNotEmpty()) 
                { 
                    this.Append( this.NewLine() ); 
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool Comma()
        {
            if (this.Scan(",").IsNotEmpty())
            {
                if (this.Peek("(?!\\{\\})*;").IsNotEmpty() || (this.c < this.Options.Wrap))
                {
                    this.Append(", ");
                } 
                else 
                {
                    this.Append(",", this.NewLine() );
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool Parens()
        {
            if (this.Scan("\\(\\s*\\)").IsNotEmpty())
            {
                this.Append("()");

                return true;
            }
            else if(this.Scan("\\(").IsNotEmpty())
            {
                this.Append(" ( ");

                return true;
            }
            else if (this.Scan("\\)").IsNotEmpty())
            {
                this.Append(" ) ");

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool Eos()
        {
            if (this.Scan(";").IsNotEmpty())
            {
                this.Append(";");

                if (this.Peek("\\s*\\}").IsNotEmpty())
                {
                    this.Append(this.NewLine());
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool Eol()
        {
            return this.Scan("\n").IsNotEmpty();
        }
    }
}
