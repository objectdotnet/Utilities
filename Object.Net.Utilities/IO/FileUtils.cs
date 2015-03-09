/*
 * @version   : 2.5.0
 * @author    : Object.NET, Inc. http://object.net/
 * @date      : 2014-10-20
 * @copyright : Copyright (c) 2008-2015, Object.NET, Inc. (http://object.net/). All rights reserved.
 * @license   : Apache License, Version 2.0, See LICENSE in root 
 * @website   : http://object.net/
 */

using System.IO;

namespace Object.Net.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class FileUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string text = sr.ReadToEnd();
                sr.Close();

                return text;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        public static void WriteFile(string path, string value)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.Write(value);
            sw.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        public static void WriteToStart(string path, string value)
        {
            string temp = ReadFile(path);
            WriteFile(path, value.Trim() + temp.Trim());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        public static void WriteToEnd(string path, string value)
        {
            string temp = ReadFile(path);
            WriteFile(path, temp.Trim() + value.Trim());
        }
    }
}
