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
    /// 实现 <see cref="ITypeMappingPolicy"/>
    /// </summary>
    public class TypeMappingPolicy : ITypeMappingPolicy
    {
        private DependencyResolutionLocatorKey pair;

        /// <summary>
        /// 实例化 <see cref="TypeMappingPolicy"/> 类
        /// </summary>
        /// <param name="type">被创建对象的类型</param>
        /// <param name="id">被创建对象的唯一标识符</param>
        public TypeMappingPolicy(Type type, string id)
        {
            pair = new DependencyResolutionLocatorKey(type, id);
        }

        /// <summary>
        /// 映射一个[类型/ID]
        /// </summary>
        /// <param name="incomingTypeIDPair">传入类[型/ID]</param>
        /// <returns>新的[型/ID]</returns>
		public DependencyResolutionLocatorKey Map(DependencyResolutionLocatorKey incomingTypeIDPair)
        {
            return pair;
        }
    }
}
