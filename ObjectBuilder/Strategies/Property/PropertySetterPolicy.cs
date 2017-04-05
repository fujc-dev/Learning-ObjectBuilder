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

using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// 实现 <see cref="IPropertySetterPolicy"/>.
	/// </summary>
	public class PropertySetterPolicy : IPropertySetterPolicy
	{
		private Dictionary<string, IPropertySetterInfo> properties = new Dictionary<string, IPropertySetterInfo>();

        /// <summary>
        /// 获取存储需要设置的属性和值
        /// </summary>
        public Dictionary<string, IPropertySetterInfo> Properties
		{
			get { return properties; }
		}
	}
}
