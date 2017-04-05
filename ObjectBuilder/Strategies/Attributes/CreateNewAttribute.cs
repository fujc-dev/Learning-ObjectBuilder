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
    /// 声明一个参数总是新建实例
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class CreateNewAttribute : ParameterAttribute
    {
        /// <summary>
        /// 创建一个参数用于各种 <see cref="IBuilderPolicy"/> 实现可以处理 <see cref="IParameter"/>
        /// </summary>
        /// <param name="annotatedMemberType">成员的类型，如属性或构造函数参数</param>
        /// <returns>依赖项的值的参数实例</returns>
        public override IParameter CreateParameter(Type annotatedMemberType)
        {
            return new CreationParameter(annotatedMemberType, Guid.NewGuid().ToString());
        }
    }
}
