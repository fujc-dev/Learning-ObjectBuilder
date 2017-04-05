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
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 构造器反射策略，此策略主要包含两个任务，一个是选择构造器，二是创建该构造函数参数列表，然后设置<see cref="ConstructorPolicy"/>政策方法(二级策略)
    /// </summary>
    public class ConstructorReflectionStrategy : ReflectionStrategy<ConstructorInfo>
    {
        /// <summary>
        /// 获取需要反射的成员信息
        /// </summary>
        /// <param name="context">对象创建上下文</param>
        /// <param name="typeToBuild">被创建的对象类型</param>
        /// <param name="existing">已存在的对象，如果是null，则将在CreationStrategy创建。</param>
        /// <param name="idToBuild">需要创建的对象的唯一编号</param>
        /// <returns></returns>
        protected override IEnumerable<IReflectionMemberInfo<ConstructorInfo>> GetMembers(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            //新建一个构造函数集合
            List<IReflectionMemberInfo<ConstructorInfo>> result = new List<IReflectionMemberInfo<ConstructorInfo>>();
            //获取创建策略。
            ICreationPolicy existingPolicy = context.Policies.Get<ICreationPolicy>(typeToBuild, idToBuild);
            //如果existingPolicy是DefaultCreationPolicy政策，existing为null并且existingPolicy不为null
            if (existing == null && (existingPolicy == null || existingPolicy is DefaultCreationPolicy))
            {
                ConstructorInfo injectionCtor = null; //变量，为当前默认注入的构造函数
                ConstructorInfo[] ctors = typeToBuild.GetConstructors();  //被构建的对象的构造函数集合
                //当只包含一个构造函数时，指定当前构造函数为默认注入构造函数
                if (ctors.Length == 1)
                {
                    injectionCtor = ctors[0];
                }
                else
                {
                    //当包含多个构造函数是，
                    foreach (ConstructorInfo ctor in ctors)
                    {
                        if (Attribute.IsDefined(ctor, typeof(InjectionConstructorAttribute)))  //通过循环检测构造函数是否被设置了InjectionConstructorAttribute，意思是当包含多个构造函数时，只有标记InjectionConstructor属性才是有效的构造函数
                        {
                            //多个修饰，抛出异常，需要注入的类型的构造函数只能标记一个为InjectionConstructorAttribute
                            if (injectionCtor != null)
                            {
                                throw new InvalidAttributeException();
                            }
                            injectionCtor = ctor;
                        }
                    }
                }

                if (injectionCtor != null)
                {
                    result.Add(new ReflectionMemberInfo<ConstructorInfo>(injectionCtor));
                }

            }

            return result;
        }

        /// <summary>
        /// 在 <see cref="ReflectionStrategy{T}.AddParametersToPolicy"/> 中查看更多，设置ICreationPolicy为ConstructorPolicy。
        /// </summary>
        protected override void AddParametersToPolicy(IBuilderContext context, Type typeToBuild, string idToBuild, IReflectionMemberInfo<ConstructorInfo> member, IEnumerable<IParameter> parameters)
        {
            ConstructorPolicy policy = new ConstructorPolicy();

            foreach (IParameter parameter in parameters)
                policy.AddParameter(parameter);

            context.Policies.Set<ICreationPolicy>(policy, typeToBuild, idToBuild);
        }

        /// <summary>
        /// 在 <see cref="ReflectionStrategy{T}.MemberRequiresProcessing"/> 中查看更多，只有一个，直接返回true
        /// </summary>
        protected override bool MemberRequiresProcessing(IReflectionMemberInfo<ConstructorInfo> member)
        {
            return true;
        }
    }
}
