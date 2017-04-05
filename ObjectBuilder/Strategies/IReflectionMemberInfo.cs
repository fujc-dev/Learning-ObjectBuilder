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
    /// <see cref="IReflectionMemberInfo{TInfo}"/> 类型的枚举集合
    /// </summary>
    public interface IReflectionMemberInfo<TInfo>
    {
        /// <summary>
        /// 成员的信息，如 MethodInfo， ContructorInfo 等
        /// </summary>
        TInfo MemberInfo { get; }

        /// <summary>
        /// 成员名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 搜索成员指定的自定义属性
        /// </summary>
        /// <returns></returns>
        object[] GetCustomAttributes(Type attributeType, bool inherit);

        /// <summary>
        /// 获取传递给成员的参数
        /// </summary>
        /// <returns></returns>
        ParameterInfo[] GetParameters();
    }
}
