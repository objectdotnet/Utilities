/*
 * @version   : 2.5.0
 * @author    : Object.NET, Inc. http://object.net/
 * @date      : 2014-10-20
 * @copyright : Copyright (c) 2008-2015, Object.NET, Inc. (http://object.net/). All rights reserved.
 * @license   : Apache License, Version 2.0, See LICENSE in root 
 * @website   : http://object.net/
 */

using System;

namespace Object.Net.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Verify
    {
        /// <summary>
        /// Checks if parameter is not null. Throws ArgumentNullException with name of parameter if null
        /// </summary>
        /// <param name="parameter">The parameter value to check.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        public static void IsNotNull(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName, parameterName);
            }
        }

        /// <summary>
        /// Checks if the value is a type of String object. Throws ArgumentException if value is not a String type object.  
        /// </summary>
        /// <param name="value">The object to check.</param>
        /// <param name="paramterName">The name of the parameter.</param>
        public static void IsString(object value, string paramterName)
        {
            if (!(value is string))
            {
                throw new ArgumentException(paramterName, paramterName);
            }
        }
    }
}
