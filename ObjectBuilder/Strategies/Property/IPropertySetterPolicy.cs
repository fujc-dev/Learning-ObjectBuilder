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
    /// 属性设置政策。存储需要设置的属性和值
    /// </summary>
    public interface IPropertySetterPolicy : IBuilderPolicy
    {
        /// <summary>
        /// 获取存储需要设置的属性和值
        /// </summary>
        Dictionary<string, IPropertySetterInfo> Properties { get; }
    }
}