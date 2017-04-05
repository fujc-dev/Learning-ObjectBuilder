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
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 实现 <see cref="IPropertySetterInfo"/>，属性设置器信息
    /// </summary>
    public class PropertySetterInfo : IPropertySetterInfo
    {
        string name = null;
        PropertyInfo prop = null;
        IParameter value = null;

        /// <summary>
        /// 实例化 <see cref="PropertySetterInfo"/>类， 设置属性名称和设置的值
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        public PropertySetterInfo(string name, IParameter value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// 实例化 <see cref="PropertySetterInfo"/>类，通过<see cref="PropertyInfo"/>构建
        /// </summary>
        /// <param name="propInfo">PropertyInfo信息</param>
        /// <param name="value">参数信息</param>
        public PropertySetterInfo(PropertyInfo propInfo, IParameter value)
        {
            this.prop = propInfo;
            this.value = value;
        }

        /// <summary>
        /// 在 <see cref="IPropertySetterInfo.SelectProperty"/> 中查看更多
        /// </summary>
        public PropertyInfo SelectProperty(IBuilderContext context, Type type, string id)
        {
            if (prop != null)
                return prop;

            return type.GetProperty(name);
        }

        /// <summary>
        /// 在 <see cref="IPropertySetterInfo.GetValue"/> 中查看更多
        /// </summary>
        public object GetValue(IBuilderContext context, Type type, string id, PropertyInfo propInfo)
        {
            return value.GetValue(context);
        }
    }
}
