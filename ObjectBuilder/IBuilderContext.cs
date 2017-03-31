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
    /// 表示生成或分解操作运行的上下文，在对象创建策略中的创建上下文对象，可以对比HttpContent以及管道对象Content
    /// </summary>
    public interface IBuilderContext
    {
        /// <summary>
        /// 检索策略链的头
        /// </summary>
        /// <returns>链中的第一策略；如果链中没有策略，则返回null</returns>
        IBuilderStrategy HeadOfChain { get; }

        /// <summary>
        /// 表示一个生命周期的定位器
        /// </summary>
        IReadWriteLocator Locator { get; }

        /// <summary>
        /// 当前创建对象的外部策略，围绕对象创建的策略集合
        /// </summary>
        PolicyList Policies { get; }

        /// <summary>
        /// 检索策略链中的下一个项目，相对于即将运行策略项
        /// </summary>
        /// <param name="currentStrategy">当前的策略对象</param>
        /// <returns>返回下一个策略，如果返回null那么当前对象的策略对象为最后一个策略</returns>
        IBuilderStrategy GetNextInChain(IBuilderStrategy currentStrategy);
    }
}