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
    /// 派生自 <see cref="IBuilderStrategy"/> 重新映射类型和ID
    /// </summary>
    public class TypeMappingStrategy : BuilderStrategy
    {

        /// <summary>
        ///  重写 <see cref="BuilderStrategy.BuildUp"/>.
        /// </summary>
        /// <param name="context">当前策略的执行上下文</param>
        /// <param name="t">需要创建的对象类型</param>
        /// <param name="existing">当前需要创建的对象的实例，一般传null</param>
        /// <param name="id">需要创建的对象的唯一编号</param>
        /// <returns></returns>
        public override object BuildUp(IBuilderContext context, Type t, object existing, string id)
        {
            //当前这个一级策略要做的事情
            DependencyResolutionLocatorKey result = new DependencyResolutionLocatorKey(t, id);
            //context.Policies存储了当前管道中所有的二级策略
            //获取二级策略ITypeMappingPolicy
            ITypeMappingPolicy policy = context.Policies.Get<ITypeMappingPolicy>(t, id);

            if (policy != null)
            {
                result = policy.Map(result);
                TraceBuildUp(context, t, id, Properties.Resources.TypeMapped, result.Type, result.ID ?? "(null)");
                Guard.TypeIsAssignableFromType(t, result.Type, t);
            }

            return base.BuildUp(context, result.Type, existing, result.ID);
        }
    }
}