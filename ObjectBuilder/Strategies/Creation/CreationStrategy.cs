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
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 实现 <see cref="IBuilderStrategy"/> 用于创建类实例，一级策略就是政府规定的职能部分，政府的不同的职能部门做不同的事情，所做的事情就是二级策略
    /// </summary>
    public class CreationStrategy : BuilderStrategy
    {
        /// <summary>
        /// 重写 <see cref="IBuilderStrategy.BuildUp"/>. 用于构建对象在责任链中调用
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <param name="typeToBuild">需要创建的对象的类型</param>
        /// <param name="existing">一般默认传null对象创建器会在生成链中创建一个新的对象实例，如果不为null则将运行生成链的现有对象</param>
        /// <param name="idToBuild">需要创建的对象的唯一编号</param>
        /// <returns>创建的对象</returns>
        public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            if (existing != null)  //已存在对象。
            {
                BuildUpExistingObject(context, typeToBuild, existing, idToBuild);
            }
            else
            {
                existing = BuildUpNewObject(context, typeToBuild, existing, idToBuild); //existing 创建的对象实例，整个框架都是围绕给他赋值的
            }
            //执行下一个策略
            return base.BuildUp(context, typeToBuild, existing, idToBuild);
        }

        private void BuildUpExistingObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            RegisterObject(context, typeToBuild, existing, idToBuild);
        }

        /// <summary>
        /// 创建新对象
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <param name="typeToBuild">需要创建的对象的类型</param>
        /// <param name="existing">一般默认传null对象创建器会在生成链中创建一个新的对象实例，如果不为null则将运行生成链的现有对象</param>
        /// <param name="idToBuild">需要创建的对象的唯一编号</param>
        /// <returns></returns>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        private object BuildUpNewObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            //CreationStrategy创建对象时，会从Policies中去除对应的ICreationPolicy对象，利用ICreationPolicy对象取得
            //获取ICreationPolicy。在ConstuctorReflectionStrategy处理过了。
            ICreationPolicy policy = context.Policies.Get<ICreationPolicy>(typeToBuild, idToBuild); //ConstructorReflectionStrategy

            if (policy == null)
            {
                if (idToBuild == null)
                    throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
                        Properties.Resources.MissingPolicyUnnamed, typeToBuild));
                else
                    throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
                        Properties.Resources.MissingPolicyNamed, typeToBuild, idToBuild));
            }

            try
            {
                //创建指定对象类型的新实例，系统API，可以获取一个没有构造函数的对象实例。
                existing = FormatterServices.GetSafeUninitializedObject(typeToBuild);
            }
            catch (MemberAccessException exception)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Properties.Resources.CannotCreateInstanceOfType, typeToBuild), exception);
            }
            //在Locator注册实例，如果是单件模式，将在LifetimeContainer中注册
            RegisterObject(context, typeToBuild, existing, idToBuild);
            //调用合适构造器进行初始化对象
            InitializeObject(context, existing, idToBuild, policy);
            return existing;
        }


        /// <summary>
        /// 注册对象，单例以及包含生命周期的对象
        /// </summary>
        /// <param name="context"></param>
        /// <param name="typeToBuild"></param>
        /// <param name="existing"></param>
        /// <param name="idToBuild"></param>
        private void RegisterObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            if (context.Locator != null)
            {
                //获取生命周期实例
                ILifetimeContainer lifetime = context.Locator.Get<ILifetimeContainer>(typeof(ILifetimeContainer), SearchMode.Local);
                //如果包含生命周期
                if (lifetime != null)
                {
                    //获取单例政策
                    ISingletonPolicy singletonPolicy = context.Policies.Get<ISingletonPolicy>(typeToBuild, idToBuild);
                    //如果包含，并且是单例
                    if (singletonPolicy != null && singletonPolicy.IsSingleton)
                    {
                        //向定位器(存储器中存储对象实例)
                        context.Locator.Add(new DependencyResolutionLocatorKey(typeToBuild, idToBuild), existing);
                        lifetime.Add(existing);

                        if (TraceEnabled(context))
                            TraceBuildUp(context, typeToBuild, idToBuild, Properties.Resources.SingletonRegistered);
                    }
                }
            }
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <param name="existing">一般默认传null对象创建器会在生成链中创建一个新的对象实例，如果不为null则将运行生成链的现有对象</param>
        /// <param name="id">需要创建的对象的唯一编号</param>
        /// <param name="policy">构建构造函数方法的策略</param>
        private void InitializeObject(IBuilderContext context, object existing, string id, ICreationPolicy policy)
        {
            //需要创建的对象的默认构造函数实例
            Type type = existing.GetType();
            //获取当前对象实例的构造函数
            ConstructorInfo constructor = policy.SelectConstructor(context, type, id);

            if (constructor == null)
            {
                if (type.IsValueType)
                    return;
                throw new ArgumentException(Properties.Resources.NoAppropriateConstructor);
            }
            //获取当前构造函数中的参数
            object[] parms = policy.GetParameters(context, type, id, constructor);
            //
            MethodBase method = (MethodBase)constructor;
            Guard.ValidateMethodParameters(method, parms, existing.GetType());
            if (TraceEnabled(context))
            {
                TraceBuildUp(context, type, id, Properties.Resources.CallingConstructor, ParametersToTypeList(parms));
            }
            //使用指定的参数调用当前实例所表示的方法或构造函数。
            method.Invoke(existing, parms);
        }

        private void ValidateCtorParameters(MethodBase methodInfo, object[] parameters, Type typeBeingBuilt)
        {
            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                Guard.TypeIsAssignableFromType(paramInfos[i].ParameterType, parameters[i].GetType(), typeBeingBuilt);
            }
        }
    }
}