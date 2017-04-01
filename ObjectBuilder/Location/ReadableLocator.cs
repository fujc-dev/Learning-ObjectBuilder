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
    /// 一个抽象类<see cref="ReadableLocator"/>用来实现<see cref="IReadableLocator"/>接口的公共方法
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
        /// 使用谓词操作来查找包含给定对象的定位器
        /// </summary>
        /// <param name="predicate">自定义的条件检测是否包含<see cref="IReadableLocator"/>对象</param>
        /// <returns>返回一个新的定位器</returns>
        /// <exception cref="ArgumentNullException">Predicate为null</exception>
        public IReadableLocator FindBy(Predicate<KeyValuePair<object, object>> predicate)
        {
            return FindBy(SearchMode.Up, predicate);
        }

        /// <summary>
        /// 根据是否回溯的选项，使用谓词操作来查找包含对象的定位器
        /// </summary>
        /// <param name="options">查找选项(一个枚举)</param>
        /// <param name="predicate">自定义的条件检测是否包含<see cref="IReadableLocator"/>对象</param>
        /// <returns>返回一个新的定位器</returns>
        /// <exception cref="ArgumentNullException">Predicate为null</exception>
        /// <exception cref="ArgumentException">SearchMode选项不是有效枚举值</exception>
        public IReadableLocator FindBy(SearchMode options, Predicate<KeyValuePair<object, object>> predicate)
        {
            //没有指定predicate委托，直接则抛出异常
            if (predicate == null) throw new ArgumentNullException("predicate");
            //没有指定的搜索选项，也抛出异常
            if (!Enum.IsDefined(typeof(SearchMode), options)) throw new ArgumentException(Properties.Resources.InvalidEnumerationValue, "options");
            //
            Locator results = new Locator(); //依赖子类？很多牛逼的代码都会这样做，为什么？有点意思啊~~ 组合模式？
            IReadableLocator currentLocator = this;
            //一个循环调用FindInLocator私有方法，如果查询选项是只搜索当前定位器，那么直接中断循环
            while (currentLocator != null)
            {
                //遍历定位，查找对象是否在定位器中存在
                FindInLocator(predicate, results, currentLocator);
                //获取下一次遍历的定位器(要么为null，要么就去当前定位器的父级定位器)
                currentLocator = options == SearchMode.Local ? null : currentLocator.ParentLocator;
            }
            //返回一个只读的定位器新实例
            return new ReadOnlyLocator(results);
        }

        /// <summary>
        /// 遍历定位器，将找到的对象存入一个临时的定位器
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="results"></param>
        /// <param name="currentLocator"></param>
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

        /// <summary>
        /// 设置此定位器的父定位器
        /// </summary>
        /// <param name="parentLocator">实现 <see cref="IReadableLocator"/>接口的对象实例.</param>
        protected void SetParentLocator(IReadableLocator parentLocator)
        {
            this.parentLocator = parentLocator;
        }

        #region IEnumerable<KeyValuePair<object, object>>

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


    }
}
