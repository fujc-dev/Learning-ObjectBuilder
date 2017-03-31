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

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    ///实现<see cref="IBuilderStrategyChain"/>接口
    /// </summary>
    public class BuilderStrategyChain : IBuilderStrategyChain
    {
        private List<IBuilderStrategy> strategies;

        /// <summary>
        /// 实例化策略责任链 <see cref="BuilderStrategyChain"/> 类.
        /// </summary>
        public BuilderStrategyChain()
        {
            strategies = new List<IBuilderStrategy>();
        }

        /// <summary>
        /// 在 <see cref="IBuilderStrategyChain.Head"/> 中查看更多
        /// </summary>
        public IBuilderStrategy Head
        {
            get
            {
                if (strategies.Count > 0)
                    return strategies[0];
                else
                    return null;
            }
        }

        /// <summary>
        /// 在 <see cref="IBuilderStrategyChain.Add"/> 中查看更多
        /// </summary>
        public void Add(IBuilderStrategy strategy)
        {
            strategies.Add(strategy);
        }

        /// <summary>
        /// 在 <see cref="IBuilderStrategyChain.AddRange"/>中查看更多
        /// </summary>
        public void AddRange(IEnumerable strategies)
        {
            foreach (IBuilderStrategy strategy in strategies)
                Add(strategy);
        }

        /// <summary>
        /// 在 <see cref="IBuilderStrategyChain.GetNext"/> 中查看更多
        /// </summary>
        public IBuilderStrategy GetNext(IBuilderStrategy currentStrategy)
        {
            for (int idx = 0; idx < strategies.Count - 1; idx++)
                if (ReferenceEquals(currentStrategy, strategies[idx])) //检测是否是相同的对象实例
                    return strategies[idx + 1];

            return null;
        }
    }
}
