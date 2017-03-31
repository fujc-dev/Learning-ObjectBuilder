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

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 派生自 <see cref="IReadableLocator"/>类型，只读的定位器.
    /// </summary>
    public class ReadOnlyLocator : ReadableLocator
    {
        private IReadableLocator innerLocator;

        /// <summary>
        /// 构造函数，创建一个只读的定位器
        /// </summary>
        /// <param name="innerLocator">默认定位器</param>
        public ReadOnlyLocator(IReadableLocator innerLocator)
        {
            if (innerLocator == null)
                throw new ArgumentNullException("innerLocator");

            this.innerLocator = innerLocator;
        }

        /// <summary>
        /// 返回定位器中的项数
        /// </summary>
        public override int Count
        {
            get { return innerLocator.Count; }
        }

        /// <summary>
        /// 父定位器
        /// </summary>
        public override IReadableLocator ParentLocator
        {
            get
            {
                return new ReadOnlyLocator(innerLocator.ParentLocator);
            }
        }

        /// <summary>
        /// 如果定位器是只读的，返回true
        /// </summary>
        public override bool ReadOnly
        {
            get { return true; }
        }

        /// <summary>
        ///  确定定位器是否包含给定键的对象
        /// </summary>
        public override bool Contains(object key, SearchMode options)
        {
            return innerLocator.Contains(key, options);
        }

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        public override object Get(object key, SearchMode options)
        {
            return innerLocator.Get(key, options);
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        public override IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            return innerLocator.GetEnumerator();
        }
    }
}
