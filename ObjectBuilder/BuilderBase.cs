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
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 实现IBuilder接口的辅助类
    /// </summary>
    /// <typeparam name="TStageEnum">这个枚举的泛型表示类型创建策略</typeparam>
    public class BuilderBase<TStageEnum> : IBuilder<TStageEnum>
    {
        /// <summary>
        /// 创建对象时所需要的一系列围绕创建对象时所需要的附加策略信息，所有的政策方针
        /// </summary>
        private PolicyList policies = new PolicyList();
        /// <summary>
        /// 策略集合，用于存储对象创建时所需要的一系列策略
        /// </summary>
        private StrategyList<TStageEnum> strategies = new StrategyList<TStageEnum>();
        /// <summary>
        /// 存储器的锁集合，即存储器对象和锁映射对。
        /// </summary>
        private Dictionary<object, object> lockObjects = new Dictionary<object, object>();

        /// <summary>
        ///实例化一个 <see cref="BuilderBase{T}"/> 类.
        /// </summary>
        public BuilderBase()
        {
        }

        /// <summary>
        /// 通过<see cref="IBuilderConfigurator{BuilderStage}"/>配置实例化一个 <see cref="BuilderBase{T}"/> 类.
        /// </summary>
        /// <param name="configurator">生成器配置对象接口</param>
        public BuilderBase(IBuilderConfigurator<TStageEnum> configurator)
        {
            configurator.ApplyConfiguration(this);
        }

        /// <summary>
        /// 在 <see cref="IBuilder{TStageEnum}.Policies"/> 中查看更多信息
        /// </summary>
        public PolicyList Policies
        {
            get { return policies; }
        }

        /// <summary>
        /// 在 <see cref="IBuilder{TStageEnum}.Strategies"/> 中查看更多信息
        /// </summary>
        public StrategyList<TStageEnum> Strategies
        {
            get { return strategies; }
        }

        /// <summary>
        /// 在 <see cref="IBuilder{TStageEnum}.BuildUp{T}"/> 中查看更多信息
        /// </summary>
        public TTypeToBuild BuildUp<TTypeToBuild>(IReadWriteLocator locator, string idToBuild, object existing, params PolicyList[] transientPolicies)
        {
            return (TTypeToBuild)BuildUp(locator, typeof(TTypeToBuild), idToBuild, existing, transientPolicies);
        }

        /// <summary>
        /// 在 <see cref="IBuilder{TStageEnum}.BuildUp"/> 中查看更多信息
        /// </summary>
        public virtual object BuildUp(IReadWriteLocator locator, Type typeToBuild, string idToBuild, object existing, params PolicyList[] transientPolicies)
        {
            if (locator != null)
            {
                //获取存储器的锁，并锁定
                lock (GetLock(locator))
                {
                    return DoBuildUp(locator, typeToBuild, idToBuild, existing, transientPolicies);
                }
            }
            else
            {
                return DoBuildUp(locator, typeToBuild, idToBuild, existing, transientPolicies);
            }

        }
        //执行构建。获取构建策略链，构建上下文，利用策略链头构建对象。
        private object DoBuildUp(IReadWriteLocator locator, Type typeToBuild, string idToBuild, object existing, PolicyList[] transientPolicies)
        {
            ////将策略列表构建成一个策略链条。
            IBuilderStrategyChain chain = strategies.MakeStrategyChain();  //获取责任链中所有的策略后
            //检查策略链构建是否成功。
            ThrowIfNoStrategiesInChain(chain); //检测当前策略责任链中是否包含策略，不包含就抛出一个异常
            //创建构建上下文。将临时政策与构建器政策整合。
            IBuilderContext context = MakeContext(chain, locator, transientPolicies);

            IBuilderTracePolicy trace = context.Policies.Get<IBuilderTracePolicy>(null, null);

            if (trace != null) trace.Trace(Properties.Resources.BuildUpStarting, typeToBuild, idToBuild ?? "(null)");
            //获取一级策略集合中的索引为0的策略执行，取第一个策略进行执行
            object result = null;
            //开始构建对象，从此处开始会将策略链上所有的策略都执行一遍，除非单例策略
            if (chain.Head != null) //默认是不会出现Head为null的情况，因为初始化时框架会初始化一些默认的策略
            {
                result = chain.Head.BuildUp(context, typeToBuild, existing, idToBuild);  //执行对象创建，
            }
            if (trace != null) trace.Trace(Properties.Resources.BuildUpFinished, typeToBuild, idToBuild ?? "(null)");

            return result;
        }

        /// <summary>
        /// ObjectBuilder对象创建上下文，可以理解为每一个策略执行时的一个上下文信息，通过这个上下文可以在策略责任链中传递执行
        /// </summary>
        /// <param name="chain">策略责任链，默认的策略或自定义的策略</param>
        /// <param name="locator">生成对象的定位器，当对象已存在时直接在定位器中获取</param>
        /// <param name="transientPolicies">当前附加的二级策略</param>
        /// <returns></returns>
        private IBuilderContext MakeContext(IBuilderStrategyChain chain, IReadWriteLocator locator, params PolicyList[] transientPolicies)
        {
            PolicyList policies = new PolicyList(this.policies); //将当前的临时策略添加到策略集合中，这里面包含默认二级策略和临时二级策略
            foreach (PolicyList policyList in transientPolicies)
            {
                policies.AddPolicies(policyList);
            }
            return new BuilderContext(chain, locator, policies);
        }

        private static void ThrowIfNoStrategiesInChain(IBuilderStrategyChain chain)
        {
            if (chain.Head == null)
                throw new InvalidOperationException(Properties.Resources.BuilderHasNoStrategies);
        }

        /// <summary>
        /// 在 <see cref="IBuilder{TStageEnum}.TearDown{T}"/> 中查看更多信息
        /// </summary>
        public TItem TearDown<TItem>(IReadWriteLocator locator, TItem item)
        {
            if (typeof(TItem).IsValueType == false && item == null)
                throw new ArgumentNullException("item");

            if (locator != null)
            {
                lock (GetLock(locator))
                {
                    return DoTearDown<TItem>(locator, item);
                }
            }
            else
            {
                return DoTearDown<TItem>(locator, item);
            }
        }

        private TItem DoTearDown<TItem>(IReadWriteLocator locator, TItem item)
        {
            //构建反转策略链。
            IBuilderStrategyChain chain = strategies.MakeReverseStrategyChain();
            ThrowIfNoStrategiesInChain(chain);

            Type type = item.GetType();
            IBuilderContext context = MakeContext(chain, locator);
            IBuilderTracePolicy trace = context.Policies.Get<IBuilderTracePolicy>(null, null);

            if (trace != null)
                trace.Trace(Properties.Resources.TearDownStarting, type);
            //执行清理。
            TItem result = (TItem)chain.Head.TearDown(context, item);

            if (trace != null)
                trace.Trace(Properties.Resources.TearDownFinished, type);

            return result;
        }
        //获取存储器的锁。
        private object GetLock(object locator)
        {
            lock (lockObjects)
            {
                if (lockObjects.ContainsKey(locator))
                    return lockObjects[locator];

                object newLock = new object();
                lockObjects[locator] = newLock;
                return newLock;
            }
        }
    }
}
