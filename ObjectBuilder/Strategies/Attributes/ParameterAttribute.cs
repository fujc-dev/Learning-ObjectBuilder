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
    /// 属性表示的依赖注入的成员，其值在编译时将由 <see cref="IParameter"/> 返回属性 <see cref="ParameterAttribute.CreateParameter"/> 工厂方法.
    /// </summary>
    public abstract class ParameterAttribute : Attribute
    {
        /// <summary>
        /// 实例化 <see cref="ParameterAttribute"/> 类
        /// </summary>
        protected ParameterAttribute() { }

        /// <summary>
        /// 创建一个参数用于各种 <see cref="IBuilderPolicy"/> 实现可以处理 <see cref="IParameter"/>
        /// </summary>
        /// <param name="memberType">成员的类型，如属性或构造函数参数</param>
        /// <returns>依赖项的值的参数实例</returns>
        public abstract IParameter CreateParameter(Type memberType);
    }
}
