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
    /// 派生自 <see cref="BuilderStrategy"/> 方法执行策略
    /// </summary>
    public class MethodExecutionStrategy : BuilderStrategy
    {
        /// <summary>
        /// 用于构建对象在责任链中调用
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <param name="typeToBuild">需要创建的对象的类型</param>
        /// <param name="existing">一般默认传null对象创建器会在生成链中创建一个新的对象实例，如果不为null则将运行生成链的现有对象</param>
        /// <param name="idToBuild">需要创建的对象的唯一编号</param>
        /// <returns>创建的对象</returns>
        public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            //执行政策
            ApplyPolicy(context, existing, idToBuild);
            return base.BuildUp(context, typeToBuild, existing, idToBuild);
        }

        /// <summary>
        /// 执行政策
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        private void ApplyPolicy(IBuilderContext context, object obj, string id)
        {
            if (obj == null)
                return;

            Type type = obj.GetType();
            //获取方法执行政策
            IMethodPolicy policy = context.Policies.Get<IMethodPolicy>(type, id);
            //直接返回。
            if (policy == null)
                return;
            //调用每一个方法。
            foreach (IMethodCallInfo methodCallInfo in policy.Methods.Values)
            {
                MethodInfo methodInfo = methodCallInfo.SelectMethod(context, type, id);

                if (methodInfo != null)
                {
                    object[] parameters = methodCallInfo.GetParameters(context, type, id, methodInfo);
                    Guard.ValidateMethodParameters(methodInfo, parameters, obj.GetType());
                    if (TraceEnabled(context))
                    {
                        TraceBuildUp(context, type, id, Properties.Resources.CallingMethod, methodInfo.Name, ParametersToTypeList(parameters));
                    }
                    //调用方法。
                    methodInfo.Invoke(obj, parameters);
                }
            }
        }
    }
}