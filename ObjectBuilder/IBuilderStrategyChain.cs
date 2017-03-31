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
    /// ObjectBuilder创建对象策略的责任链
    /// </summary>
    public interface IBuilderStrategyChain
    {
        /// <summary>
        /// 策略责任链中的第一策略；如果链中没有策略，则返回null
        /// </summary>
        IBuilderStrategy Head { get; }

        /// <summary>
        /// 添加策略
        /// </summary>
        /// <param name="strategy">策略对象.</param>
        void Add(IBuilderStrategy strategy);

        /// <summary>
        /// 批量添加策略
        /// </summary>
        /// <param name="strategies">策略对象集合</param>
        void AddRange(IEnumerable strategies);

        /// <summary>
        /// 取链中的下一个策略，相对于给定的策略
        /// </summary>
        /// <param name="currentStrategy">当前执行的策略</param>
        /// <returns>返回下一个策略，如果返回null那么当前对象的策略对象为最后一个策略</returns>
        IBuilderStrategy GetNext(IBuilderStrategy currentStrategy);
    }
}