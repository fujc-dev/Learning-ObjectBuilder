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
    /// 实现 <see cref="IParameter"/> 的帮助类， 当你提前知道参数值的类型时，可以使用它，用于实现那些你已经预先知道参数值类型的参数的助手类 
    /// </summary>
    public abstract class KnownTypeParameter : IParameter
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        protected Type type;

        /// <summary>
        ///  使用给定类型实例化 <see cref="KnownTypeParameter"/> 类
        /// </summary>
        /// <param name="type">参数类型</param>
        protected KnownTypeParameter(Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// 获取参数值的类型
        /// </summary>
        public Type GetParameterType(IBuilderContext context)
        {
            return type;
        }

        /// <summary>
        /// 抽象方法r <see cref="IParameter.GetValue"/> 派生类必须必须实现
        /// </summary>
        /// <param name="context">对象创建策略上下文</param>
        /// <returns>参数值</returns>
        public abstract object GetValue(IBuilderContext context);
    }
}