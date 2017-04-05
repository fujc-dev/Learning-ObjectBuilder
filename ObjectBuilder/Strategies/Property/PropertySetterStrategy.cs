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
using System.Globalization;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 实现 <see cref="IBuilderStrategy"/> ，属性设置策略
    /// </summary>
    /// <remarks>
    public class PropertySetterStrategy : BuilderStrategy
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
            if (existing != null)
                InjectProperties(context, existing, idToBuild);

            return base.BuildUp(context, typeToBuild, existing, idToBuild);
        }

        private void InjectProperties(IBuilderContext context, object obj, string id)
        {
            if (obj == null)
                return;

            Type type = obj.GetType();
            //获取设置政策
            IPropertySetterPolicy policy = context.Policies.Get<IPropertySetterPolicy>(type, id);
            //不需要解决依赖
            if (policy == null)
                return;
            //为每一个属性解决依赖
            foreach (IPropertySetterInfo propSetterInfo in policy.Properties.Values)
            {
                //获取属性信息
                PropertyInfo propInfo = propSetterInfo.SelectProperty(context, type, id);

                if (propInfo != null)
                {
                    if (propInfo.CanWrite)
                    {
                        object value = propSetterInfo.GetValue(context, type, id, propInfo);

                        if (value != null)
                            Guard.TypeIsAssignableFromType(propInfo.PropertyType, value.GetType(), obj.GetType());

                        if (TraceEnabled(context))
                            TraceBuildUp(context, type, id, Properties.Resources.CallingProperty, propInfo.Name, propInfo.PropertyType.Name);
                        //设置属性值。
                        propInfo.SetValue(obj, value, null);
                    }
                    else
                    {
                        throw new ArgumentException(String.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resources.CannotInjectReadOnlyProperty,
                            type, propInfo.Name));
                    }
                }
            }
        }
    }
}