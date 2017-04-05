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
    /// 代表的策略为 <see cref="TypeMappingStrategy"/>，派生自 <see cref="IBuilderPolicy"/> 的映射处理二级策略，
    /// 每一个构建的对象都是存储在Locator中，Locator缓存中每一个对象的主键都是Type/ID。<see cref="ITypeMappingPolicy"/>用于解决将要求解决依赖的对象映射到正确的Type/ID中；
    /// </summary>
    public interface ITypeMappingPolicy : IBuilderPolicy
    {
        /// <summary>
        /// 将一个<see cref="DependencyResolutionLocatorKey"/> 映射到另一个 Key
        /// </summary>
        /// <param name="incomingTypeIDPair">传入类[型/ID]</param>
        /// <returns>新的[型/ID]</returns>
        DependencyResolutionLocatorKey Map(DependencyResolutionLocatorKey incomingTypeIDPair);
    }
}