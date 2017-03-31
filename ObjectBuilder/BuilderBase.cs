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
        /// 创建对象时所需要的一系列围绕创建对象时所需要的附加策略信息
        /// </summary>
        private PolicyList policies = new PolicyList();
        /// <summary>
        /// 策略集合，用于存储对象创建时所需要的一系列策略
        /// </summary>
        private StrategyList<TStageEnum> strategies = new StrategyList<TStageEnum>();
        /// <summary>
        /// 
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

        private object DoBuildUp(IReadWriteLocator locator, Type typeToBuild, string idToBuild, object existing, PolicyList[] transientPolicies)
        {
            IBuilderStrategyChain chain = strategies.MakeStrategyChain();
            ThrowIfNoStrategiesInChain(chain);

            IBuilderContext context = MakeContext(chain, locator, transientPolicies);
            IBuilderTracePolicy trace = context.Policies.Get<IBuilderTracePolicy>(null, null);

            if (trace != null)
                trace.Trace(Properties.Resources.BuildUpStarting, typeToBuild, idToBuild ?? "(null)");

            object result = chain.Head.BuildUp(context, typeToBuild, existing, idToBuild);

            if (trace != null)
                trace.Trace(Properties.Resources.BuildUpFinished, typeToBuild, idToBuild ?? "(null)");

            return result;
        }

        private IBuilderContext MakeContext(IBuilderStrategyChain chain, IReadWriteLocator locator, params PolicyList[] transientPolicies)
        {
            PolicyList policies = new PolicyList(this.policies);

            foreach (PolicyList policyList in transientPolicies)
                policies.AddPolicies(policyList);

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
            IBuilderStrategyChain chain = strategies.MakeReverseStrategyChain();
            ThrowIfNoStrategiesInChain(chain);

            Type type = item.GetType();
            IBuilderContext context = MakeContext(chain, locator);
            IBuilderTracePolicy trace = context.Policies.Get<IBuilderTracePolicy>(null, null);

            if (trace != null)
                trace.Trace(Properties.Resources.TearDownStarting, type);

            TItem result = (TItem)chain.Head.TearDown(context, item);

            if (trace != null)
                trace.Trace(Properties.Resources.TearDownFinished, type);

            return result;
        }

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
