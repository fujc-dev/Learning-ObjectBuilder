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
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 一个实现 <see cref="IBuilderStrategy"/>接口的抽象类
    /// </summary>
    public abstract class BuilderStrategy : IBuilderStrategy
    {
        /// <summary>
        /// 通用版本的对象创建，以帮助单元测试
        /// </summary>
        public TItem BuildUp<TItem>(IBuilderContext context, TItem existing, string idToBuild)
        {
            return (TItem)BuildUp(context, typeof(TItem), existing, idToBuild);
        }

        /// <summary>
        /// 用于构建对象在责任链中调用
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <param name="typeToBuild">需要创建的对象的类型</param>
        /// <param name="existing">一般默认传null对象创建器会在生成链中创建一个新的对象实例，如果不为null则将运行生成链的现有对象</param>
        /// <param name="idToBuild">需要创建的对象的唯一编号</param>
        /// <returns>创建的对象</returns>
        public virtual object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            IBuilderStrategy next = context.GetNextInChain(this);

            if (next != null)
                return next.BuildUp(context, typeToBuild, existing, idToBuild);
            else
                return existing;
        }

        /// <summary>
        /// 用于摧毁对象在责任链中调用
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <param name="item">需要销毁的对象实例</param>
        /// <returns>返回当前销毁的对象实例</returns>
        public virtual object TearDown(IBuilderContext context, object item)
        {
            IBuilderStrategy next = context.GetNextInChain(this);

            if (next != null)
                return next.TearDown(context, item);
            else
                return item;
        }

        /// <summary>
        /// 从<see cref="IParameter"/>对象列表创建参数类型的跟踪列表
        /// </summary>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回参数集合类型的串联字符串</returns>
        protected string ParametersToTypeList(params object[] parameters)
        {
            List<string> types = new List<string>();
            foreach (object parameter in parameters)
            {
                types.Add(parameter.GetType().Name);
            }
            return string.Join(", ", types.ToArray());
        }

        /// <summary>
        /// 跟踪调试信息，如果有适当的策略
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <param name="typeToBuild">The type being built.</param>
        /// <param name="idToBuild">The ID being built.</param>
        /// <param name="format">The format of the message.</param>
        /// <param name="args">The message arguments.</param>
        protected void TraceBuildUp(IBuilderContext context, Type typeToBuild, string idToBuild, string format, params object[] args)
        {
            IBuilderTracePolicy policy = context.Policies.Get<IBuilderTracePolicy>(null, null);

            if (policy != null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, format, args);
                policy.Trace(Properties.Resources.BuilderStrategyTraceBuildUp, GetType().Name, typeToBuild.Name, idToBuild ?? "(null)", message);
            }
        }

        /// <summary>
        /// Traces debugging information, if there is an appropriate policy.
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <param name="item">Item being torn down.</param>
        /// <param name="format">The format of the message.</param>
        /// <param name="args">The message arguments.</param>
        protected void TraceTearDown(IBuilderContext context, object item, string format, params object[] args)
        {
            IBuilderTracePolicy policy = context.Policies.Get<IBuilderTracePolicy>(null, null);

            if (policy != null)
            {
                string message = string.Format(CultureInfo.CurrentCulture, format, args);
                policy.Trace(Properties.Resources.BuilderStrategyTraceTearDown, GetType().Name, item.GetType().Name, message);
            }
        }

        /// <summary>
        /// Determines if tracing is enabled
        /// </summary>
        /// <param name="context">The build context.</param>
        /// <returns>Returns true if tracing is enabled; false otherwise.</returns>
        protected bool TraceEnabled(IBuilderContext context)
        {
            return context.Policies.Get<IBuilderTracePolicy>(null, null) != null;
        }
    }
}
