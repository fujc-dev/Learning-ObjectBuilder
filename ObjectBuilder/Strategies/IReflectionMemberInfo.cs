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
        /// 对框架的搜索属性方法的封装 ，搜索成员指定的自定义属性，在派生类中重写时，返回由 <see cref="Type"/> 标识的自定义属性的数组。
        /// </summary>
        /// <param name="attributeType">要搜索的属性类型。只返回可分配给此类型的属性</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些属性</param>
        /// <returns></returns>
        object[] GetCustomAttributes(Type attributeType, bool inherit);

        /// <summary>
        /// 获取传递给成员的参数
        /// </summary>
        /// <returns></returns>
        ParameterInfo[] GetParameters();
    }
}
