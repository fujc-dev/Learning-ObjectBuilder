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

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
	/// 表示用于构造函数和方法调用的单个参数，以及属性设置，这个接口用于在创建对象是需要传递的参数，在三种注入模式中，有一个共通的规则，就是需要有参数来注入，
	/// Constructor Injection是透过构造函数参数注入，
	/// 而Interface Injection则是透过函数参数注入，
	/// Setter Injection则是透过属性注入，因此参数是这三种注入模式都会用到的观念，所以ObjectBuilder定义了IParameter接口，并提供一组实现此接口的参数对象，于注入时期由这些参数对象来取得参数值
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// 获取参数值的类型
        /// </summary>
        /// <param name="context">对象创建策略上下文</param>
        /// <returns>参数类型</returns>
        Type GetParameterType(IBuilderContext context);

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="context">对象创建策略上下文</param>
        /// <returns>参数值</returns>
        object GetValue(IBuilderContext context);
    }
}