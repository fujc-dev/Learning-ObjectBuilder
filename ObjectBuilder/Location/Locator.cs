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
    /// 派生自 <see cref="ReadWriteLocator"/>类，描述一个定位器可读写，ObjectBuilder内部默认的定位器，一般参数传递的定位器实例就的它的实例
    /// </summary>
    public class Locator : ReadWriteLocator
    {
        private WeakRefDictionary<object, object> references = new WeakRefDictionary<object, object>();

        /// <summary>
        /// 构造函数，创建根定位器
        /// </summary>
        public Locator()
            : this(null)
        {
        }

        /// <summary>
        /// 构造函数，创建子定位器
        /// </summary>
        /// <param name="parentLocator">设置父定位器</param>
        public Locator(IReadableLocator parentLocator)
        {
            SetParentLocator(parentLocator);
        }

        /// <summary>
        /// 返回定位器中的项数
        /// </summary>
        public override int Count
        {
            get { return references.Count; }
        }

        /// <summary>
        /// 添加一个对象到定位器，与给定的键
        /// </summary>
        public override void Add(object key, object value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            references.Add(key, value);
        }

        /// <summary>
        /// 确定定位器是否包含给定键的对象
        /// </summary>
        public override bool Contains(object key, SearchMode options)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (!Enum.IsDefined(typeof(SearchMode), options))
                throw new ArgumentException(Properties.Resources.InvalidEnumerationValue, "options");

            if (references.ContainsKey(key))
                return true;

            if (options == SearchMode.Up && ParentLocator != null)
                return ParentLocator.Contains(key, options);

            return false;
        }

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        public override object Get(object key, SearchMode options)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (!Enum.IsDefined(typeof(SearchMode), options))
                throw new ArgumentException(Properties.Resources.InvalidEnumerationValue, "options");

            if (references.ContainsKey(key))
                return references[key];

            if (options == SearchMode.Up && ParentLocator != null)
                return ParentLocator.Get(key, options);

            return null;
        }

        /// <summary>
        /// 从定位器移除对象
        /// </summary>
        public override bool Remove(object key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            return references.Remove(key);
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        public override IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            return references.GetEnumerator();
        }
    }
}
