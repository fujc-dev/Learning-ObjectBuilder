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
    /// 实现 <see cref="IParameter"/> 直接持有用于参数的值
    /// </summary>
    /// <typeparam name="TValue">参数值类型</typeparam>
    public class ValueParameter<TValue> : KnownTypeParameter
    {
        private TValue value;

        /// <summary>
        /// 实例化 <see cref="ValueParameter{T}"/> 类
        /// </summary>
        /// <param name="value">参数值</param>
        public ValueParameter(TValue value)
            : base(typeof(TValue))
        {
            this.value = value;
        }

        /// <summary>
        /// 重写<see cref="IParameter.GetValue"/>.
        /// </summary>
        /// <param name="context">对象创建策略上下文</param>
        /// <returns>参数值</returns>
        public override object GetValue(IBuilderContext context)
        {
            return value;
        }
    }

    /// <summary>
    /// 实现 <see cref="KnownTypeParameter"/> 直接持有用于参数的值
    /// </summary>
    public class ValueParameter : KnownTypeParameter
    {
        private object value;

        /// <summary>
        /// 实例化 <see cref="ValueParameter"/> 类
        /// </summary>
        /// <param name="valueType">参数值的类型</param>
        /// <param name="value">参数值</param>
        public ValueParameter(Type valueType, object value)
            : base(valueType)
        {
            this.value = value;
        }

        /// <summary>
        /// 重写<see cref="IParameter.GetValue"/>.
        /// </summary>
        /// <param name="context">对象创建策略上下文</param>
        /// <returns>参数值</returns>
        public override object GetValue(IBuilderContext context)
        {
            return value;
        }
    }
}