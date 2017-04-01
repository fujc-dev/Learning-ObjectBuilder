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
    /// <see cref="IReflectionMemberInfo{TMemberInfo}"/> 类型的枚举集合
    /// </summary>
    public interface IReflectionMemberInfo<TMemberInfo>
    {
        /// <summary>
        /// 原成员信息对象
        /// </summary>
        TMemberInfo MemberInfo { get; }

        /// <summary>
        /// 成员名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 成员的自定义特性
        /// </summary>
        /// <returns></returns>
        object[] GetCustomAttributes(Type attributeType, bool inherit);

        /// <summary>
        /// 传递给成员的参数
        /// </summary>
        /// <returns></returns>
        ParameterInfo[] GetParameters();
    }
}
