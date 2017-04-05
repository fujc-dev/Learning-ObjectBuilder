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
    /// 属性应用于属性和构造函数参数，以描述依赖注入系统何时注入对象
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class DependencyAttribute : ParameterAttribute
    {
        private string name;
        private Type createType;
        private NotPresentBehavior notPresentBehavior = NotPresentBehavior.CreateNew;//默认是CreateNew。
        private SearchMode searchMode;

        /// <summary>
        ///实例化 <see cref="DependencyAttribute"/> 类.
        /// </summary>
        public DependencyAttribute()
        {
        }

        /// <summary>
        /// 所要注入的对象名称。可选
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 当依赖的对象没有找到，如果指定创建一个新的依赖对象，指定它的类型。可选
        /// </summary>
        public Type CreateType
        {
            get { return createType; }
            set { createType = value; }
        }

        /// <summary>
        /// 指定在定位器中搜索对象的模式
        /// </summary>
        public SearchMode SearchMode
        {
            get { return searchMode; }
            set { searchMode = value; }
        }


        /// <summary>
        /// 依赖对象为找到时的行为，默认为 CreateNew<see cref="Microsoft.Practices.ObjectBuilder.NotPresentBehavior.CreateNew"/>.
        /// </summary>
        public NotPresentBehavior NotPresentBehavior
        {
            get { return notPresentBehavior; }
            set { notPresentBehavior = value; }
        }

        /// <summary>
        /// 创建需要的参数，在 <see cref="ParameterAttribute.CreateParameter"/> 中查看更多
        /// </summary>
        public override IParameter CreateParameter(Type annotatedMemberType)
        {
            return new DependencyParameter(annotatedMemberType, name, createType, notPresentBehavior, searchMode);
        }
    }
}
