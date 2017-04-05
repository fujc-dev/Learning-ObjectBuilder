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
    /// 派生自 <see cref="BuilderStrategy"/>，类型映射策略，首先构建的是存储器主键，类型映射策略，用于解决正确的依赖类型
    /// 即新建一个<see cref="DependencyResolutionLocatorKey"/>对象，
    /// 然后获取该对象对应的<see cref="ITypeMappingPolicy"/>政策方针(二级策略)，
    /// 调用该对象的Map方法，获取正确的类型、ID和主键，并将正确的类型传递给下一个策略(单例策略)。
    /// </summary>
    public class TypeMappingStrategy : BuilderStrategy
    {

        /// <summary>
        ///  重写 <see cref="BuilderStrategy.BuildUp"/>.
        /// </summary>
        /// <param name="context">当前策略的执行上下文</param>
        /// <param name="t">被创建对象的类型</param>
        /// <param name="existing">当前需要创建的对象的实例，一般传null，已存在的对象实例</param>
        /// <param name="id">需要创建的对象的唯一编号</param>
        /// <returns></returns>
        public override object BuildUp(IBuilderContext context, Type t, object existing, string id)
        {
            //方法中t参数是要被创建的对象类型，existing是已存在的对象实例，id是要被创建对象的字符串标识
            //首先使用类型和字符串标识创建一个 DependencyResolutionLocatorKey 对象，通常是作为定位器中的对象的标识键
            DependencyResolutionLocatorKey result = new DependencyResolutionLocatorKey(t, id);
            //context.Policies存储了当前管道中所有的二级策略
            //获取二级策略ITypeMappingPolicy
            ITypeMappingPolicy policy = context.Policies.Get<ITypeMappingPolicy>(t, id);           
            if (policy != null)
            {
                result = policy.Map(result); //映射策略的作用是将当前将要被创建的类型与二级策略的映射关系做一些微调，一般是指接口，派生类之间
                TraceBuildUp(context, t, id, Properties.Resources.TypeMapped, result.Type, result.ID ?? "(null)");
                //类型result.Type是否派生自t，或者本身result.Type与t是相同类型，满足这两个条件映射才有效，否则抛出异常
                Guard.TypeIsAssignableFromType(t, result.Type, t);
            }
            //将正确的类型传递给下一个策略执行构建。
            return base.BuildUp(context, result.Type, existing, result.ID);
        }
    }
}