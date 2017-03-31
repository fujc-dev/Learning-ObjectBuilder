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
    /// 可读取和写入的定位器
    /// </summary>
    public interface IReadWriteLocator : IReadableLocator
    {
        /// <summary>
        /// 添加一个对象到定位器，与给定的键
        /// </summary>
        /// <param name="key">Key，唯一标识</param>
        /// <param name="value">要注册的对象</param>
        /// <exception cref="ArgumentNullException">Key或value为null.</exception>
        void Add(object key, object value);

        /// <summary>
        /// 从定位器移除对象
        /// </summary>
        /// <param name="key">Key，唯一标识</param>
        /// <returns>如果在定位器中找到该对象，则返回true；否则返回false</returns>
        /// <exception cref="ArgumentNullException">Key为null.</exception>
        bool Remove(object key);
    }
}