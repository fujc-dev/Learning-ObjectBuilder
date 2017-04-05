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
    /// 实现 <see cref="IParameter"/> 另一参数的克隆的参数
    /// </summary>
    public class CloneParameter : IParameter
	{
		private IParameter param;

        /// <summary>
        /// 实例化 <see cref="CloneParameter"/> 类
        /// </summary>
        /// <param name="param">要克隆的参数.</param>
        public CloneParameter(IParameter param)
		{
			this.param = param;
		}

		/// <summary>
		/// 在 <see cref="IParameter.GetParameterType"/> 中查看更多
		/// </summary>
		public Type GetParameterType(IBuilderContext context)
		{
			return param.GetParameterType(context);
		}

        /// <summary>
        /// 在 <see cref="IParameter.GetValue"/> 中查看更多
        /// </summary>
        public object GetValue(IBuilderContext context)
		{
			object val = param.GetValue(context);

			if (val is ICloneable)
				val = ((ICloneable)val).Clone();

			return val;
		}
	}
}