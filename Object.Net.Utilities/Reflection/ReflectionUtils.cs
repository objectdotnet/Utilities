/*
 * @version   : 2.5.0
 * @author    : Object.NET, Inc. http://object.net/
 * @date      : 2014-10-20
 * @copyright : Copyright (c) 2008-2015, Object.NET, Inc. (http://object.net/). All rights reserved.
 * @license   : Apache License, Version 2.0, See LICENSE in root 
 * @website   : http://object.net/
 */

using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.UI;
using Object.Net;

namespace Object.Net.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class ReflectionUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetDefaultValue(PropertyDescriptor property)
        {
            DefaultValueAttribute attr = property.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;

            return attr != null ? attr.Value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetDefaultValue(PropertyInfo property)
        {
            object[] att = property.GetCustomAttributes(typeof(DefaultValueAttribute), false);

            return att.Length > 0 ? ((DefaultValueAttribute)att[0]).Value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTypeOf(object obj, Type type)
        {
            return IsTypeOf(obj, type.FullName, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="shallow"></param>
        /// <returns></returns>
        public static bool IsTypeOf(object obj, Type type, bool shallow)
        {
            return IsTypeOf(obj, type.FullName, shallow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static bool IsTypeOf(object obj, string typeFullName)
        {
            return IsTypeOf(obj, typeFullName, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="typeFullName"></param>
        /// <param name="shallow"></param>
        /// <returns></returns>
        public static bool IsTypeOf(object obj, string typeFullName, bool shallow)
        {
            if (obj != null)
            {
                if (shallow)
                {
                    return obj.GetType().FullName.Equals(typeFullName);
                }
                else
                {
                    Type type = obj.GetType();
                    string fullName = type.FullName;

                    while (!fullName.Equals("System.Object"))
                    {
                        if (fullName.Equals(typeFullName))
                        {
                            return true;
                        }

                        type = type.BaseType;
                        fullName = type.FullName;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInTypeOf(Control control, Type type)
        {
            return IsInTypeOf(control, type.FullName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static bool IsInTypeOf(Control control, string typeFullName)
        {
            Control temp = GetTypeOfParent(control, typeFullName);

            return (temp != null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Control GetTypeOfParent(Control control, Type type)
        {
            return GetTypeOfParent(control, type.FullName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static Control GetTypeOfParent(Control control, string typeFullName)
        {
            for (Control parent = control.Parent; parent != null; parent = parent.Parent)
            {
                if (ReflectionUtils.IsTypeOf(parent, typeFullName))
                {
                    return parent;
                }
            }

            return null;
        }
    }
}