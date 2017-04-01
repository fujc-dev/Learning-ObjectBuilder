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
    /// 用于属性和构造函数参数，以描述依赖注入系统何时应创建所请求对象的新实例
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
	public sealed class CreateNewAttribute : ParameterAttribute
	{
		/// <summary>
		/// See <see cref="ParameterAttribute.CreateParameter"/> for more information.
		/// </summary>
		public override IParameter CreateParameter(Type annotatedMemberType)
		{
			return new CreationParameter(annotatedMemberType, Guid.NewGuid().ToString());
		}
	}
}
