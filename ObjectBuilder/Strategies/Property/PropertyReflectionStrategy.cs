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
    /// 描述需要解决依赖注入的属性
    /// </summary>
    public class PropertyReflectionStrategy : ReflectionStrategy<PropertyInfo>
    {
        /// <summary>
        /// 在 <see cref="ReflectionStrategy{T}.GetMembers"/> 中查看更多，获取所有的属性信息。
        /// </summary>
        protected override IEnumerable<IReflectionMemberInfo<PropertyInfo>> GetMembers(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            foreach (PropertyInfo propInfo in typeToBuild.GetProperties())
            {
                yield return new PropertyReflectionMemberInfo(propInfo);
            }
        }

        /// <summary>
        /// 将属性和值序列插入到属性设置政策中
        /// </summary>
        protected override void AddParametersToPolicy(IBuilderContext context, Type typeToBuild, string idToBuild, IReflectionMemberInfo<PropertyInfo> member, IEnumerable<IParameter> parameters)
        {
            //获取属性政策
            PropertySetterPolicy result = context.Policies.Get<IPropertySetterPolicy>(typeToBuild, idToBuild) as PropertySetterPolicy;
            //若不存在，则新建一个。
            if (result == null)
            {
                result = new PropertySetterPolicy();
                context.Policies.Set<IPropertySetterPolicy>(result, typeToBuild, idToBuild);
            }
            //将每一个属性和值序列添加到属性政策中。
            foreach (IParameter parameter in parameters)
            {
                if (!result.Properties.ContainsKey(member.Name))
                {
                    result.Properties.Add(member.Name, new PropertySetterInfo(member.MemberInfo, parameter));
                }
            }
        }

        /// <summary>
        /// 判断一个属性是否需要解决依赖注入
        /// </summary>
        protected override bool MemberRequiresProcessing(IReflectionMemberInfo<PropertyInfo> member)
        {
            return (member.GetCustomAttributes(typeof(ParameterAttribute), true).Length > 0);
        }

        /// <summary>
        /// 内联类，属性反射信息
        /// </summary>
        private class PropertyReflectionMemberInfo : IReflectionMemberInfo<PropertyInfo>
        {
            PropertyInfo prop;

            public PropertyReflectionMemberInfo(PropertyInfo prop)
            {
                this.prop = prop;
            }

            public PropertyInfo MemberInfo
            {
                get { return prop; }
            }

            public string Name
            {
                get { return prop.Name; }
            }

            public object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                return prop.GetCustomAttributes(attributeType, inherit);
            }

            public ParameterInfo[] GetParameters()
            {
                return new ParameterInfo[] { new CustomPropertyParameterInfo(prop) };
            }
        }


        /// <summary>
        /// 内联类，自定义属性参数信息
        /// </summary>
        private class CustomPropertyParameterInfo : ParameterInfo
        {
            PropertyInfo prop;

            public CustomPropertyParameterInfo(PropertyInfo prop)
            {
                this.prop = prop;
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                return prop.GetCustomAttributes(attributeType, inherit);
            }

            public override Type ParameterType
            {
                get { return prop.PropertyType; }
            }
        }
    }
}
