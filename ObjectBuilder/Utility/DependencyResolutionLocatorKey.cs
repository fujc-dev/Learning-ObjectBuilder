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

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 两个对象如果它们类型和字符串标识一致，就认为是同一个对象，依赖解析定位器的唯一标识，当前需要创建对象的类型和标记当前创建类型的唯一标识，
    /// 存储器是对象主键和对象的序列集合，对象主键由对象的 Type 和 ID 构成，由<see cref="DependencyResolutionLocatorKey"/> 对象表示
    /// </summary>
    public sealed class DependencyResolutionLocatorKey
    {
        private Type type;
        private string id;

        /// <summary>
        /// 实例化 <see cref="DependencyResolutionLocatorKey"/> 类， 类型和ID都为null
        /// </summary>
        public DependencyResolutionLocatorKey()
            : this(null, null)
        {
        }

        /// <summary>
        /// 实例化 <see cref="DependencyResolutionLocatorKey"/> 类
        /// </summary>
        /// <param name="type">当前创建对象的类型</param>
        /// <param name="id">当前创建对象的唯一编号</param>
        public DependencyResolutionLocatorKey(Type type, string id)
        {
            this.type = type;
            this.id = id;
        }

        /// <summary>
        /// 返回当前创建对象的编号
        /// </summary>
        public string ID
        {
            get { return id; }
        }

        /// <summary>
        /// 返回当前创建对象的类型
        /// </summary>
        public Type Type
        {
            get { return type; }
        }

        /// <summary>
        /// 重写Equals比较函数
        /// </summary>
        public override bool Equals(object obj)
        {
            DependencyResolutionLocatorKey other = obj as DependencyResolutionLocatorKey;

            if (other == null)
                return false;

            return (Equals(type, other.type) && Equals(id, other.id));//两个对象的类型相同并且id相等时，默认他们相等
        }

        /// <summary>
        /// 重写GetHashCode函数
        /// </summary>
        public override int GetHashCode()
        {
            int hashForType = type == null ? 0 : type.GetHashCode();
            int hashForID = id == null ? 0 : id.GetHashCode();
            return hashForType ^ hashForID;
        }
    }
}