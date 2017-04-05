//===============================================================================
// Microsoft patterns & practices
// ObjectBuilder Application Block
//===============================================================================
// Copyright ?Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 验证类
    /// </summary>
	internal static class Guard
    {

        /// <summary>
        /// 类型是否兼容， 确定当前的 System.Type 的实例是否可以从指定 Type 的实例分配。
        /// </summary>
        /// <param name="assignee"></param>
        /// <param name="providedType"></param>
        /// <param name="classBeingBuilt"></param>
		public static void TypeIsAssignableFromType(Type assignee, Type providedType, Type classBeingBuilt)
        {
            if (!assignee.IsAssignableFrom(providedType))
                throw new IncompatibleTypesException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.TypeNotCompatible, assignee, providedType, classBeingBuilt));
        }

        /// <summary>
        /// 方法参数是否相等
        /// </summary>
        /// <param name="methodInfo">提供有关方法和构造函数的信息。</param>
        /// <param name="parameters">参数集合</param>
        /// <param name="typeBeingBuilt"></param>
        public static void ValidateMethodParameters(MethodBase methodInfo, object[] parameters, Type typeBeingBuilt)
        {
            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                if (parameters[i] != null)
                {
                    Guard.TypeIsAssignableFromType(paramInfos[i].ParameterType, parameters[i].GetType(), typeBeingBuilt);
                }
            }
        }
    }
}
