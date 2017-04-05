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
    /// 需要依赖的方法的信息
    /// </summary>
    public interface IMethodCallInfo
	{
        /// <summary>
        /// 获取调用方法的参数列表
        /// </summary>
        /// <param name="context">The builder context.</param>
        /// <param name="type">The type of object requested.</param>
        /// <param name="id">The ID of the object requested.</param>
        /// <param name="method">The method that will be used.</param>
        /// <returns>An array of parameters to pass to the method.</returns>
        object[] GetParameters(IBuilderContext context, Type type, string id, MethodInfo method);

        /// <summary>
        /// 获取注入的方法
        /// </summary>
        /// <param name="context">The builder context.</param>
        /// <param name="type">The type of object requested.</param>
        /// <param name="id">The ID of the object requested.</param>
        /// <returns>The method to use; return null if no suitable method can be found.</returns>
        MethodInfo SelectMethod(IBuilderContext context, Type type, string id);
	}
}
