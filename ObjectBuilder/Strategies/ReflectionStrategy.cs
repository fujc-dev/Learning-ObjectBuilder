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
        /// 查找类型中实施了依赖注入特性的成员（属性或者构造器的参数）
        /// </summary>
        /// <param name="context">对象创建上下文</param>
        /// <param name="typeToBuild">被创建的对象类型</param>
        /// <param name="existing">Existing object being built, if available.</param>
        /// <param name="idToBuild">The ID being built.</param>
        /// <returns>An enumerable wrapper around the IReflectionMemberInfo{T} interfaces that represent the members to be inspected for reflection.</returns>
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
                    IEnumerable<IParameter> parameters = GenerateIParametersFromParameterInfos(member.GetParameters());
                    AddParametersToPolicy(context, typeToBuild, idToBuild, member, parameters);
                }
            }

            return base.BuildUp(context, typeToBuild, existing, idToBuild);
        }

        /// <summary>
        /// 用于派生类实现增加参数信息到二级策略中的方法。
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <param name="typeToBuild">The type being built.</param>
        /// <param name="idToBuild">The ID being built.</param>
        /// <param name="member">The member that's being reflected over.</param>
        /// <param name="parameters">The parameters used to satisfy the member call.</param>
        protected abstract void AddParametersToPolicy(IBuilderContext context, Type typeToBuild, string idToBuild, IReflectionMemberInfo<TMemberInfo> member, IEnumerable<IParameter> parameters);

        //从参数信息中生成参数
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
        //获取参数特性的方法
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
        /// 派生类决定一个成员是否需要处理
        /// </summary>
        /// <param name="member">The member being reflected over.</param>
        /// <returns>Returns true if the member should get injection; false otherwise.</returns>
        protected abstract bool MemberRequiresProcessing(IReflectionMemberInfo<TMemberInfo> member);
    }
}
