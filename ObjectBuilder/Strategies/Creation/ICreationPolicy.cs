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
    /// 二级策略的构建策略，创建一个对象的二级策略 <see cref="CreationStrategy"/>.
    /// </summary>
    public interface ICreationPolicy : IBuilderPolicy
    {
        /// <summary>
        /// 选择用于创建对象的构造函数
        /// </summary>
        /// <param name="context">策略执行上下文</param>
        /// <param name="type">需要创建的对象的类型</param>
        /// <param name="id">需要创建的对象的唯一编号</param>
        /// <returns>要使用的构造函数；如果找不到合适的构造函数，则返回null</returns>
        ConstructorInfo SelectConstructor(IBuilderContext context, Type type, string id);

        /// <summary>
        /// 获取要传递给构造函数的参数值
        /// </summary>
        /// <param name="context">策略执行上下文</param>
        /// <param name="type">需要创建的对象的类型</param>
        /// <param name="id">需要创建的对象的唯一编号</param>
        /// <param name="constructor">发现类构造函数的属性并提供对构造函数元数据的访问权</param>
        /// <returns>传递给构造函数的参数数组</returns>
        object[] GetParameters(IBuilderContext context, Type type, string id, ConstructorInfo constructor);
    }
}