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
    /// 实现 <see cref="IReadWriteLocator"/>接口，可读写的定位器
    /// </summary>
    public abstract class ReadWriteLocator : ReadableLocator, IReadWriteLocator
    {
        /// <summary>
        /// 是否只读，默认为false，不可读
        /// </summary>
        public override bool ReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// 添加一个对象到定位器，与给定的键
        /// </summary>
        public abstract void Add(object key, object value);

        /// <summary>
        /// 从定位器移除对象
        /// </summary>
        public abstract bool Remove(object key);
    }
}
