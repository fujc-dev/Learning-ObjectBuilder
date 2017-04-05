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
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 派生自 <see cref="BuilderStrategy"/> 类，所有注入处理器的基本通用策略，实施反射策略的一个主要目的就是支持对象创建时的依赖注入
    /// </summary>
    public abstract class ReflectionStrategy<TMemberInfo> : BuilderStrategy
    {
        /// <summary>
        /// 获取需要反射的成员信息
        /// </summary>
        /// <param name="context">对象创建上下文</param>
        /// <param name="typeToBuild">被创建的对象类型</param>
        /// <param name="existing">已存在的对象，如果是null，则将在CreationStrategy创建。</param>
        /// <param name="idToBuild">需要创建的对象的唯一编号</param>
        /// <returns></returns>
        protected abstract IEnumerable<IReflectionMemberInfo<TMemberInfo>> GetMembers(IBuilderContext context, Type typeToBuild, object existing, string idToBuild);

        /// <summary>
        /// 对每一个找到的成员，如果需要处理的话，就生成相应的信息（参数类型、参数值等），并保存到上下文的对应的方针中，以便在创建阶段得到正确的创建，在<see cref="BuilderStrategy.BuildUp"/> 中查看更多
        /// </summary>
        public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            foreach (IReflectionMemberInfo<TMemberInfo> member in GetMembers(context, typeToBuild, existing, idToBuild))
            {
                if (MemberRequiresProcessing(member))
                {
                    //根据所有成员的成员信息，获取对应的IParameter集合
                    IEnumerable<IParameter> parameters = GenerateIParametersFromParameterInfos(member.GetParameters());
                    //将解决的参数保存的策略中。
                    AddParametersToPolicy(context, typeToBuild, idToBuild, member, parameters);
                }
            }
            //调用下一个策略，由下一个策略获取解决的参数执行构建。
            return base.BuildUp(context, typeToBuild, existing, idToBuild);
        }

        /// <summary>
        /// 将属性和值序列插入到属性设置政策中
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <param name="typeToBuild">The type being built.</param>
        /// <param name="idToBuild">The ID being built.</param>
        /// <param name="member">The member that's being reflected over.</param>
        /// <param name="parameters">The parameters used to satisfy the member call.</param>
        protected abstract void AddParametersToPolicy(IBuilderContext context, Type typeToBuild, string idToBuild, IReflectionMemberInfo<TMemberInfo> member, IEnumerable<IParameter> parameters);

        //获取IParameter集合。获取每一个参数的特性，调用特性的CreateParameter方法获取参数值
        private IEnumerable<IParameter> GenerateIParametersFromParameterInfos(ParameterInfo[] parameterInfos)
        {
            List<IParameter> result = new List<IParameter>();

            foreach (ParameterInfo parameterInfo in parameterInfos)
            {
                ParameterAttribute attribute = GetInjectionAttribute(parameterInfo);
                result.Add(attribute.CreateParameter(parameterInfo.ParameterType));
            }

            return result;
        }
        //获取参数特性的方法，获取注入参数特性。默认是DependencyAttribute
        private ParameterAttribute GetInjectionAttribute(ParameterInfo parameterInfo)
        {
            ParameterAttribute[] attributes = (ParameterAttribute[])parameterInfo.GetCustomAttributes(typeof(ParameterAttribute), true);

            switch (attributes.Length)
            {
                case 0:
                    return new DependencyAttribute();

                case 1:
                    return attributes[0];

                default:
                    throw new InvalidAttributeException();
            }
        }

        /// <summary>
        /// 判断一个属性是否需要解决依赖注入
        /// </summary>
        /// <param name="member">The member being reflected over.</param>
        /// <returns>Returns true if the member should get injection; false otherwise.</returns>
        protected abstract bool MemberRequiresProcessing(IReflectionMemberInfo<TMemberInfo> member);
    }
}
