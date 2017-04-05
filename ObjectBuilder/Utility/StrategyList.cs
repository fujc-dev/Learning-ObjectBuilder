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
    /// 一级策略的自定义集合类型，策略列表，由策略执行阶段和策略列表组成
    /// </summary>
    /// <typeparam name="TStageEnum">ObjectBuilder创建对象实例的策略的执行顺序</typeparam>
    public class StrategyList<TStageEnum>
    {
        /// <summary>
        /// ObjectBuilder创建对象实例的策略的执行顺序，存储执行顺序
        /// </summary>
		private readonly static Array stageValues = Enum.GetValues(typeof(TStageEnum));
        /// <summary>
        /// ObjectBuilder创建对象实例的策略的执行顺序的键值对集合，每一个执行策略执行顺序中包含多个一级策略
        /// </summary>
		private Dictionary<TStageEnum, List<IBuilderStrategy>> stages;
        /// <summary>
        /// 同步锁
        /// </summary>
		private object lockObject = new object();

        /// <summary>
        /// 实例化 <see cref="StrategyList{T}"/> 类
        /// </summary>
        public StrategyList()
        {
            stages = new Dictionary<TStageEnum, List<IBuilderStrategy>>();
            foreach (TStageEnum stage in stageValues)
            {
                stages[stage] = new List<IBuilderStrategy>();
            }
        }

        /// <summary>
        /// 添加一级策略
        /// </summary>
        /// <param name="strategy">一级策略的对象实例</param>
        /// <param name="stage">ObjectBuilder创建对象实例的策略的执行顺序</param>
        public void Add(IBuilderStrategy strategy, TStageEnum stage)
        {
            lock (lockObject)
            {
                stages[stage].Add(strategy);
            }
        }

        /// <summary>
        /// 创建新策略并将其添加到列表中
        /// </summary>
        /// <typeparam name="TStrategy">要创建的策略类型。必须有一个无参数的构造函数</typeparam>
        /// <param name="stage">ObjectBuilder创建对象实例的策略的执行顺序</param>
        public void AddNew<TStrategy>(TStageEnum stage)
            where TStrategy : IBuilderStrategy, new()
        {
            lock (lockObject)
            {
                stages[stage].Add(new TStrategy());
            }
        }

        /// <summary>
        /// 清空一级策略列表
        /// </summary>
        public void Clear()
        {
            lock (lockObject)
            {
                foreach (TStageEnum stage in stageValues)
                {
                    stages[stage].Clear();
                }
            }
        }

        /// <summary>
        /// 根据列表中的策略创建反向策略链。用于销毁策略操作（运行策略在建立操作反向），用于清理策略
        /// </summary>
        /// <returns>新的策略责任链</returns>
        public IBuilderStrategyChain MakeReverseStrategyChain()
        {
            lock (lockObject)
            {
                List<IBuilderStrategy> tempList = new List<IBuilderStrategy>();
                foreach (TStageEnum stage in stageValues)
                {
                    tempList.AddRange(stages[stage]);
                }
                tempList.Reverse();
                BuilderStrategyChain result = new BuilderStrategyChain();
                result.AddRange(tempList);
                return result;
            }
        }

        /// <summary>
        /// 根据清单中的策略创建战略链。用于构建对象
        /// </summary>
        /// <returns>新的策略责任链</returns>
        public IBuilderStrategyChain MakeStrategyChain()
        {
            lock (lockObject)
            {
                BuilderStrategyChain result = new BuilderStrategyChain();
                foreach (TStageEnum stage in stageValues)
                {
                    result.AddRange(stages[stage]);
                }
                return result;
            }
        }
    }
}
