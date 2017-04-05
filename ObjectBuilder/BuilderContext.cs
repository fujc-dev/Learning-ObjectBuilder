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
    /// 实现 <see cref="IBuilderContext"/>的类，<see cref="BuilderContext"/> 类，默认的构建上下文。存储了策略链，存储器和政策，作为创建过程的信息载体，它包含了一个策略链表，一个策略使用的方针表以及一个用来保存创建对象的定位器
    /// </summary>
    public class BuilderContext : IBuilderContext
    {
        /// <summary>
        /// 策略责任链
        /// </summary>
        private IBuilderStrategyChain chain;
        /// <summary>
        /// 生成对象的定位器，当对象已存在时直接在定位器中获取
        /// </summary>
        private IReadWriteLocator locator;
        /// <summary>
        /// 将当前的临时策略添加到策略集合中，这里面包含默认二级策略和临时二级策略
        /// </summary>
        private PolicyList policies;

        /// <summary>
        /// 禁止使用默认构造函数初始化，实例化<see cref="BuilderContext"/> 类
        /// </summary>
        protected BuilderContext()
        {
        }

        /// <summary>
        /// 实例化 <see cref="BuilderContext"/> 类
        /// </summary>
        /// <param name="chain">策略责任链，默认的策略或自定义的策略</param>
        /// <param name="locator">生成对象的定位器，当对象已存在时直接在定位器中获取</param>
        /// <param name="policies">二级策略集合，这里面包含默认二级策略和临时二级策略</param>
        public BuilderContext(IBuilderStrategyChain chain, IReadWriteLocator locator, PolicyList policies)
        {
            this.chain = chain;
            this.locator = locator;
            this.policies = new PolicyList(policies);
        }

        /// <summary>
        /// 在 <see cref="IBuilderContext.HeadOfChain"/> 中查看更多
        /// </summary>
        public IBuilderStrategy HeadOfChain
        {
            get { return chain.Head; }
        }

        /// <summary>
        /// 在 <see cref="IBuilderContext.Locator"/> 中查看更多
        /// </summary>
        public IReadWriteLocator Locator
        {
            get { return locator; }
        }



        /// <summary>
        /// 在 <see cref="IBuilderContext.Policies"/> 中查看更多
        /// </summary>
        public PolicyList Policies
        {
            get { return policies; }
        }

        /// <summary>
        /// 设置生成对象的定位器，当对象已存在时直接在定位器中获取
        /// </summary>
        protected void SetLocator(IReadWriteLocator locator)
        {
            this.locator = locator;
        }

        /// <summary>
        /// 设置二级策略集合，这里面包含默认二级策略和临时二级策略
        /// </summary>
        protected void SetPolicies(PolicyList policies)
        {
            this.policies = policies;
        }

        /// <summary>
        /// 测了责任链
        /// </summary>
        protected IBuilderStrategyChain StrategyChain
        {
            get { return chain; }
            set { chain = value; }
        }

        /// <summary>
        /// 在 <see cref="IBuilderContext.GetNextInChain"/> 中查看更多
        /// 检索策略链中的下一个项目，相对于即将运行策略项
        /// </summary>
        public IBuilderStrategy GetNextInChain(IBuilderStrategy currentStrategy)
        {
            return chain.GetNext(currentStrategy);
        }
    }
}
