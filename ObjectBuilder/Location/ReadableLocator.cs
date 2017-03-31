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
using System.Collections;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 实现 <see cref="IReadableLocator"/>接口，
    /// </summary>
    public abstract class ReadableLocator : IReadableLocator
    {
        private IReadableLocator parentLocator;

        /// <summary>
        /// 返回定位器中的项数
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// 父定位器
        /// </summary>
        public virtual IReadableLocator ParentLocator
        {
            get { return parentLocator; }
        }

        /// <summary>
        /// 如果定位器是只读的，返回true
        /// </summary>
        public abstract bool ReadOnly { get; }

        /// <summary>
        /// 确定定位器是否包含给定键的对象
        /// </summary>
        public bool Contains(object key)
        {
            return Contains(key, SearchMode.Up);
        }

        /// <summary>
        /// 确定定位器是否包含给定键的对象
        /// </summary>
        public abstract bool Contains(object key, SearchMode options);

        /// <summary>
        /// 自定义搜索条件，并返回一个临时对象
        /// </summary>
        public IReadableLocator FindBy(Predicate<KeyValuePair<object, object>> predicate)
        {
            return FindBy(SearchMode.Up, predicate);
        }

        /// <summary>
        /// 自定义搜索条件，并返回一个临时只读定位器对象
        /// </summary>
        public IReadableLocator FindBy(SearchMode options, Predicate<KeyValuePair<object, object>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            if (!Enum.IsDefined(typeof(SearchMode), options)) throw new ArgumentException(Properties.Resources.InvalidEnumerationValue, "options");

            Locator results = new Locator();
            IReadableLocator currentLocator = this;

            while (currentLocator != null)
            {
                FindInLocator(predicate, results, currentLocator);
                currentLocator = options == SearchMode.Local ? null : currentLocator.ParentLocator;
            }
            return new ReadOnlyLocator(results);
        }

        private void FindInLocator(Predicate<KeyValuePair<object, object>> predicate, Locator results, IReadableLocator currentLocator)
        {
            foreach (KeyValuePair<object, object> kvp in currentLocator)
            {
                if (!results.Contains(kvp.Key) && predicate(kvp))
                {
                    results.Add(kvp.Key, kvp.Value);
                }
            }
        }

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        public TItem Get<TItem>()
        {
            return (TItem)Get(typeof(TItem));
        }

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        public TItem Get<TItem>(object key)
        {
            return (TItem)Get(key);
        }

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        public TItem Get<TItem>(object key, SearchMode options)
        {
            return (TItem)Get(key, options);
        }

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        public object Get(object key)
        {
            return Get(key, SearchMode.Up);
        }

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        public abstract object Get(object key, SearchMode options);

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        public abstract IEnumerator<KeyValuePair<object, object>> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 设置此定位器的父定位器
        /// </summary>
        /// <param name="parentLocator">实现 <see cref="IReadableLocator"/>接口的对象实例.</param>
        protected void SetParentLocator(IReadableLocator parentLocator)
        {
            this.parentLocator = parentLocator;
        }
    }
}
