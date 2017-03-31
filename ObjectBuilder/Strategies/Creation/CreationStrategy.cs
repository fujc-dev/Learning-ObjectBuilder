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
            if (existing != null)
                BuildUpExistingObject(context, typeToBuild, existing, idToBuild);
            else
                existing = BuildUpNewObject(context, typeToBuild, existing, idToBuild);

            return base.BuildUp(context, typeToBuild, existing, idToBuild);
        }

        private void BuildUpExistingObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            RegisterObject(context, typeToBuild, existing, idToBuild);
        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        private object BuildUpNewObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            ICreationPolicy policy = context.Policies.Get<ICreationPolicy>(typeToBuild, idToBuild);

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
                existing = FormatterServices.GetSafeUninitializedObject(typeToBuild);
            }
            catch (MemberAccessException exception)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, Properties.Resources.CannotCreateInstanceOfType, typeToBuild), exception);
            }

            RegisterObject(context, typeToBuild, existing, idToBuild);
            InitializeObject(context, existing, idToBuild, policy);
            return existing;
        }

        private void RegisterObject(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            if (context.Locator != null)
            {
                ILifetimeContainer lifetime = context.Locator.Get<ILifetimeContainer>(typeof(ILifetimeContainer), SearchMode.Local);

                if (lifetime != null)
                {
                    ISingletonPolicy singletonPolicy = context.Policies.Get<ISingletonPolicy>(typeToBuild, idToBuild);

                    if (singletonPolicy != null && singletonPolicy.IsSingleton)
                    {
                        context.Locator.Add(new DependencyResolutionLocatorKey(typeToBuild, idToBuild), existing);
                        lifetime.Add(existing);

                        if (TraceEnabled(context))
                            TraceBuildUp(context, typeToBuild, idToBuild, Properties.Resources.SingletonRegistered);
                    }
                }
            }
        }

        private void InitializeObject(IBuilderContext context, object existing, string id, ICreationPolicy policy)
        {
            Type type = existing.GetType();
            ConstructorInfo constructor = policy.SelectConstructor(context, type, id);

            if (constructor == null)
            {
                if (type.IsValueType)
                    return;
                throw new ArgumentException(Properties.Resources.NoAppropriateConstructor);
            }

            object[] parms = policy.GetParameters(context, type, id, constructor);

            MethodBase method = (MethodBase)constructor;
            Guard.ValidateMethodParameters(method, parms, existing.GetType());

            if (TraceEnabled(context))
                TraceBuildUp(context, type, id, Properties.Resources.CallingConstructor, ParametersToTypeList(parms));

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