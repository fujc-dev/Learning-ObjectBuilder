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
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 封装属性设置器
    /// </summary>
    public interface IPropertySetterInfo
	{
        /// <summary>
        /// 获取设置该属性的值
        /// </summary>
        /// <param name="context">构建上下文</param>
        /// <param name="type">The type being built.</param>
        /// <param name="id">The ID being built.</param>
        /// <param name="propInfo">The property being set.</param>
        /// <returns>要设置为属性的值</returns>
        object GetValue(IBuilderContext context, Type type, string id, PropertyInfo propInfo);

        /// <summary>
        /// 获取设置的属性信息
        /// </summary>
        /// <param name="context">构建上下文</param>
        /// <param name="type">The type being built.</param>
        /// <param name="id">The ID being built.</param>
        /// <returns>要设置的属性；如果找不到属性，则返回null</returns>
        PropertyInfo SelectProperty(IBuilderContext context, Type type, string id);
	}
}
