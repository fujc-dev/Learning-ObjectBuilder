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
    /// 方法反射策略
    /// </summary>
    public class MethodReflectionStrategy : ReflectionStrategy<MethodInfo>
    {
        /// <summary>
        /// 获取所有方法信息。
        /// </summary>
        protected override IEnumerable<IReflectionMemberInfo<MethodInfo>> GetMembers(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            foreach (MethodInfo method in typeToBuild.GetMethods())
                yield return new ReflectionMemberInfo<MethodInfo>(method);
        }

        /// <summary>
        /// 将方法信息和参数值集合序列添加到方法执行政策中。
        /// </summary>
        protected override void AddParametersToPolicy(IBuilderContext context, Type typeToBuild, string idToBuild, IReflectionMemberInfo<MethodInfo> member, IEnumerable<IParameter> parameters)
        {
            //获取方法政策。
            MethodPolicy result = context.Policies.Get<IMethodPolicy>(typeToBuild, idToBuild) as MethodPolicy;

            if (result == null)
            {
                result = new MethodPolicy();
                context.Policies.Set<IMethodPolicy>(result, typeToBuild, idToBuild);
            }

            result.Methods.Add(member.Name, new MethodCallInfo(member.MemberInfo, parameters));
        }

        /// <summary>
        /// 在 <see cref="ReflectionStrategy{T}.MemberRequiresProcessing"/> 中查看更多，//该方法是否需要执行处理。
        /// </summary>
        protected override bool MemberRequiresProcessing(IReflectionMemberInfo<MethodInfo> member)
        {
            return (member.GetCustomAttributes(typeof(InjectionMethodAttribute), true).Length > 0);
        }
    }
}
