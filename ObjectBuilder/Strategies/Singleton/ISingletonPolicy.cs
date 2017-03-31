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

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 派生自<see cref="IBuilderPolicy"/>的接口，单例策略(派生自<see cref="IBuilderPolicy"/>接口的策略为了便于理解我将其称为二级策略)
    /// </summary>
    public interface ISingletonPolicy : IBuilderPolicy
    {
        /// <summary>
        /// 如果对象应该是单例，返回true
        /// </summary>
        bool IsSingleton { get; }
    }
}