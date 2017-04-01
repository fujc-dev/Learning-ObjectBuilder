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
using System.Text;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 派生自 <see cref="BuilderStrategy"/> ，<see cref="BuilderAwareStrategy"/>策略是初始化完成阶段最后一个缺省的策略，
    /// 创建器感知策略实际上是一个回调策略，一个IBuiderAware的接口被OB提供，任何实现了IBuiderAware接口的对象，
    /// 在这个阶段会得到一个OnBuilltUp的事件通知，同时在对象被卸载的时候会得到OnTearingDown的通知。激活通知事件就是BuilderAwareStrategy的工作
    /// </summary>
    public class BuilderAwareStrategy : BuilderStrategy
    {
        /// <summary>
        /// See <see cref="IBuilderStrategy.BuildUp"/> for more information.
        /// </summary>
        public override object BuildUp(IBuilderContext context, Type t, object existing, string id)
        {
            IBuilderAware awareObject = existing as IBuilderAware;

            if (awareObject != null)
            {
                TraceBuildUp(context, t, id, Properties.Resources.CallingOnBuiltUp);
                awareObject.OnBuiltUp(id);
            }

            return base.BuildUp(context, t, existing, id);
        }

        /// <summary>
        /// See <see cref="IBuilderStrategy.TearDown"/> for more information.
        /// </summary>
        public override object TearDown(IBuilderContext context, object item)
        {
            IBuilderAware awareObject = item as IBuilderAware;

            if (awareObject != null)
            {
                TraceTearDown(context, item, Properties.Resources.CallingOnTearingDown);
                awareObject.OnTearingDown();
            }

            return base.TearDown(context, item);
        }
    }
}
