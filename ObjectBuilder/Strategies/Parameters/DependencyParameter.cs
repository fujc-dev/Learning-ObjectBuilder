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
    ///  派生自 <see cref="KnownTypeParameter"/>，依赖的参数
    /// </summary>
    public class DependencyParameter : KnownTypeParameter
    {
        private string name;
        private Type createType;
        private NotPresentBehavior notPresentBehavior;
        private SearchMode searchMode;

        /// <summary>
        /// 实例化 <see cref="DependencyParameter"/> 类
        /// </summary>
        /// <param name="parameterType"></param>
        /// <param name="name"></param>
        /// <param name="createType"></param>
        /// <param name="notPresentBehavior"></param>
        /// <param name="searchMode"></param>
        public DependencyParameter(Type parameterType, string name, Type createType, NotPresentBehavior notPresentBehavior, SearchMode searchMode)
            : base(parameterType)
        {
            this.name = name;
            this.createType = createType;
            this.notPresentBehavior = notPresentBehavior;
            this.searchMode = searchMode;
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="context">对象创建策略上下文</param>
        /// <returns>参数值</returns>
        public override object GetValue(IBuilderContext context)
        {
            return new DependencyResolver(context).Resolve(base.type, createType, name, notPresentBehavior, searchMode);
        }
    }
}
