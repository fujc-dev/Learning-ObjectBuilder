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

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 实现接口<see cref="ISingletonPolicy"/>的类
    /// </summary>
    public class SingletonPolicy : ISingletonPolicy
    {
        private bool isSingleton;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isSingleton">对象是否为单例</param>
        public SingletonPolicy(bool isSingleton)
        {
            this.isSingleton = isSingleton;
        }

        /// <summary>
        /// 在 <see cref="ISingletonPolicy.IsSingleton"/> 中查看更多
        /// </summary>
        public bool IsSingleton
        {
            get { return isSingleton; }
        }
    }
}
