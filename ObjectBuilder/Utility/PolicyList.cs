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
    /// 自定义 <see cref="IBuilderPolicy"/> 集合对象，用于存储ObjectBuilder对象创建框架中二级策略，
    /// 政策列表，由政策主键和政策序列组成，政策主键是政策类型包含政策实例的类型以及ID。
    /// </summary>
    public class PolicyList
    {
        private Dictionary<BuilderPolicyKey, IBuilderPolicy> policies = new Dictionary<BuilderPolicyKey, IBuilderPolicy>();
        private object lockObject = new object();

        /// <summary>
        /// 创建新的 <see cref="PolicyList"/> 类实例
        /// </summary>
        /// <param name="policiesToCopy">要复制到策略列表中的策略</param>
        public PolicyList(params PolicyList[] policiesToCopy)
        {
            if (policiesToCopy != null)
                foreach (PolicyList policyList in policiesToCopy)
                {
                    AddPolicies(policyList);
                }
        }

        /// <summary>
        /// 返回列表中的策略总数
        /// </summary>
        public int Count
        {
            get
            {
                lock (lockObject)
                {
                    return policies.Count;
                }
            }
        }

        /// <summary>
        /// 将二级策略添加到策略列表中。此列表中的任何策略将覆盖已存在于策略列表中的策略。
        /// </summary>
        /// <param name="policiesToCopy">要复制到策略列表中的策略</param>
        public void AddPolicies(PolicyList policiesToCopy)
        {
            lock (lockObject)
            {
                if (policiesToCopy != null)
                    foreach (KeyValuePair<BuilderPolicyKey, IBuilderPolicy> kvp in policiesToCopy.policies)
                    {
                        policies[kvp.Key] = kvp.Value;
                    }
            }
        }

        /// <summary>
        /// 移除单个策略
        /// </summary>
        /// <typeparam name="TPolicyInterface">策略类型(派生自IBuilderPolicy的接口)</typeparam>
        /// <param name="typePolicyAppliesTo"></param>
        /// <param name="idPolicyAppliesTo">当前策略的唯一标识符，可以传null</param>
        public void Clear<TPolicyInterface>(Type typePolicyAppliesTo, string idPolicyAppliesTo)
        {
            Clear(typeof(TPolicyInterface), typePolicyAppliesTo, idPolicyAppliesTo);
        }

        /// <summary>
        /// 移除单个策略
        /// </summary>
        /// <param name="policyInterface">策略类型(派生自IBuilderPolicy的接口)</param>
        /// <param name="typePolicyAppliesTo">需要ObjectBuilder创建的对象的类型</param>
        /// <param name="idPolicyAppliesTo">当前策略的唯一标识符，可以传null</param>
        public void Clear(Type policyInterface, Type typePolicyAppliesTo, string idPolicyAppliesTo)
        {
            lock (lockObject)
            {
                policies.Remove(new BuilderPolicyKey(policyInterface, typePolicyAppliesTo, idPolicyAppliesTo));
            }
        }

        /// <summary>
        /// 移除所有策略
        /// </summary>
        public void ClearAll()
        {
            lock (lockObject)
            {
                policies.Clear();
            }
        }

        /// <summary>
        /// 移除一个默认策略
        /// </summary>
        /// <typeparam name="TPolicyInterface">已注册策略的类型</typeparam>
        public void ClearDefault<TPolicyInterface>()
        {
            ClearDefault(typeof(TPolicyInterface));
        }

        /// <summary>
        /// 移除一个默认策略
        /// </summary>
        /// <param name="policyInterface">已注册策略的类型</param>
        public void ClearDefault(Type policyInterface)
        {
            Clear(policyInterface, null, null);
        }

        /// <summary>
        /// 获取一个二级策略
        /// </summary>
        /// <typeparam name="TPolicyInterface">策略类型(派生自IBuilderPolicy的接口)</typeparam>
        /// <param name="typePolicyAppliesTo">需要ObjectBuilder创建的对象的类型</param>
        /// <param name="idPolicyAppliesTo">当前策略的唯一标识符，可以传null.</param>
        /// <returns>该二级策略在列表中，如果存在返回该策略；否则返回null</returns>
        public TPolicyInterface Get<TPolicyInterface>(Type typePolicyAppliesTo, string idPolicyAppliesTo)
            where TPolicyInterface : IBuilderPolicy
        {
            return (TPolicyInterface)Get(typeof(TPolicyInterface), typePolicyAppliesTo, idPolicyAppliesTo);
        }

        /// <summary>
        /// 获取一个二级策略
        /// </summary>
        /// <param name="policyInterface">策略类型(派生自IBuilderPolicy的接口)</param>
        /// <param name="typePolicyAppliesTo">需要ObjectBuilder创建的对象的类型</param>
        /// <param name="idPolicyAppliesTo">当前策略的唯一标识符，可以传null.</param>
        /// <returns>该二级策略在列表中，如果存在返回该策略；否则返回null</returns>
        public IBuilderPolicy Get(Type policyInterface, Type typePolicyAppliesTo, string idPolicyAppliesTo)
        {
            BuilderPolicyKey key = new BuilderPolicyKey(policyInterface, typePolicyAppliesTo, idPolicyAppliesTo);
            lock (lockObject)
            {
                IBuilderPolicy policy;
                if (policies.TryGetValue(key, out policy))
                {
                    return policy;
                }
                BuilderPolicyKey defaultKey = new BuilderPolicyKey(policyInterface, null, null);
                if (policies.TryGetValue(defaultKey, out policy))
                {
                    return policy;
                }
                return null;
            }
        }

        /// <summary>
        /// 设置一个二级策略
        /// </summary>
        /// <typeparam name="TPolicyInterface">策略类型(派生自IBuilderPolicy的接口)</typeparam>
        /// <param name="policy">策略类型实例(实现派生自IBuilderPolicy的接口的接口)</param>
        /// <param name="typePolicyAppliesTo">需要ObjectBuilder创建的对象的类型</param>
        /// <param name="idPolicyAppliesTo">当前策略的唯一标识符，可以传null.</param>
        public void Set<TPolicyInterface>(TPolicyInterface policy, Type typePolicyAppliesTo, string idPolicyAppliesTo)
            where TPolicyInterface : IBuilderPolicy
        {
            Set(typeof(TPolicyInterface), policy, typePolicyAppliesTo, idPolicyAppliesTo);
        }

        /// <summary>
        ///  设置一个二级策略
        /// </summary>
        /// <param name="policyInterface">策略类型(派生自IBuilderPolicy的接口)</param>
        /// <param name="policy">策略类型实例(实现派生自IBuilderPolicy的接口的接口)</param>
        /// <param name="typePolicyAppliesTo">需要ObjectBuilder创建的对象的类型</param>
        /// <param name="idPolicyAppliesTo">当前策略的唯一标识符，可以传null.</param>
        public void Set(Type policyInterface, IBuilderPolicy policy, Type typePolicyAppliesTo, string idPolicyAppliesTo)
        {
            BuilderPolicyKey key = new BuilderPolicyKey(policyInterface, typePolicyAppliesTo, idPolicyAppliesTo);
            lock (lockObject)
            {
                policies[key] = policy;
            }
        }

        /// <summary>
        /// 设置默认策略。检查策略时，如果没有特定的个人策略可用，则将使用默认值
        /// </summary>
        /// <typeparam name="TPolicyInterface">策略类型(派生自IBuilderPolicy的接口)</typeparam>
        /// <param name="policy">策略类型实例(实现派生自IBuilderPolicy的接口的接口)</param>
        public void SetDefault<TPolicyInterface>(TPolicyInterface policy)
            where TPolicyInterface : IBuilderPolicy
        {
            SetDefault(typeof(TPolicyInterface), policy);
        }

        /// <summary>
        /// 设置默认策略。检查策略时，如果没有特定的个人策略可用，则将使用默认值
        /// </summary>
        /// <param name="policyInterface">策略类型(派生自IBuilderPolicy的接口).</param>
        /// <param name="policy">策略类型实例(实现派生自IBuilderPolicy的接口的接口)</param>
        public void SetDefault(Type policyInterface, IBuilderPolicy policy)
        {
            Set(policyInterface, policy, null, null);
        }
    }
}