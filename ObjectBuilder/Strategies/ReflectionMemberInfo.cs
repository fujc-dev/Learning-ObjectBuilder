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
    /// 实现 <see cref="IReflectionMemberInfo{T}"/> 
    /// </summary>
    /// <typeparam name="TMemberInfo"></typeparam>
    public class ReflectionMemberInfo<TMemberInfo> : IReflectionMemberInfo<TMemberInfo>
        where TMemberInfo : MethodBase
    {
        private TMemberInfo memberInfo;

        /// <summary>
        /// 实例化 <see cref="ReflectionMemberInfo{T}"/> 类.
        /// </summary>
        /// <param name="memberInfo">The member used to satisfy the interface calls.</param>
        public ReflectionMemberInfo(TMemberInfo memberInfo)
        {
            this.memberInfo = memberInfo;
        }

        /// <summary>
        /// 在 <see cref="IReflectionMemberInfo{T}.MemberInfo"/> 中查看更多
        /// </summary>
        public TMemberInfo MemberInfo
        {
            get { return memberInfo; }
        }

        /// <summary>
        /// 在 <see cref="IReflectionMemberInfo{T}.Name"/> 中查看更多
        /// </summary>
        public string Name
        {
            get { return memberInfo.Name; }
        }

        /// <summary>
        /// 在 <see cref="IReflectionMemberInfo{T}.GetCustomAttributes"/> 中查看更多
        /// </summary>
        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return memberInfo.GetCustomAttributes(attributeType, inherit);
        }

        /// <summary>
        /// 在 <see cref="IReflectionMemberInfo{T}.GetParameters"/> 中查看更多
        /// </summary>
        public ParameterInfo[] GetParameters()
        {
            return memberInfo.GetParameters();
        }
    }
}
