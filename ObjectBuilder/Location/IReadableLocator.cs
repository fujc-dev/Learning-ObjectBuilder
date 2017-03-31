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
    /// 可读取的定位器
    /// </summary>
    /// <remarks>
    /// <para>定位器是一个字典的键值，但它保持值与弱引用，所以定位一个对象不保持它活着。如果你想保持对象活着，你应该考虑使用<see cref="ILifetimeContainer"/>.</para>
    /// </remarks>
    public interface IReadableLocator : IEnumerable<KeyValuePair<object, object>>
    {
        /// <summary>
        /// 返回定位器中的项数
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 父定位器
        /// </summary>
        IReadableLocator ParentLocator { get; }

        /// <summary>
        /// 如果定位器是只读的，返回true
        /// </summary>
        bool ReadOnly { get; }

        /// <summary>
        /// 确定定位器是否包含给定键的对象
        /// </summary>
        /// <param name="key">Key，唯一标识</param>
        /// <returns>如果该定位器包含用于该键的对象，则返回true；否则返回false</returns>
        /// <exception cref="ArgumentNullException">key为null时</exception>
        bool Contains(object key);

        /// <summary>
        /// 确定定位器是否包含给定键的对象
        /// </summary>
        /// <param name="key">Key，唯一标识</param>
        /// <param name="options">查找选项(一个枚举)</param>
        /// <returns>如果该定位器包含用于该键的对象，则返回true；否则返回false</returns>
        /// <exception cref="ArgumentNullException">key为null时</exception>
        /// <exception cref="ArgumentException">SearchMode选项不是有效枚举值</exception>
        bool Contains(object key, SearchMode options);

        /// <summary>
        /// 自定义搜索条件，并返回一个临时对象
        /// </summary>
        /// <param name="predicate">自定义的条件检测是否包含<see cref="IReadableLocator"/>对象</param>
        /// <returns>返回一个新的定位器</returns>
        /// <exception cref="ArgumentNullException">Predicate为null</exception>
        IReadableLocator FindBy(Predicate<KeyValuePair<object, object>> predicate);

        /// <summary>
        /// 自定义搜索条件，并返回一个临时对象
        /// </summary>
        /// <param name="options">查找选项(一个枚举)</param>
        /// <param name="predicate">自定义的条件检测是否包含<see cref="IReadableLocator"/>对象</param>
        /// <returns>返回一个新的定位器</returns>
        /// <exception cref="ArgumentNullException">Predicate为null</exception>
        /// <exception cref="ArgumentException">SearchMode选项不是有效枚举值</exception>
        IReadableLocator FindBy(SearchMode options, Predicate<KeyValuePair<object, object>> predicate);

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        /// <typeparam name="TItem">要查找的对象的类型</typeparam>
        /// <returns>如果找到对象直接返回，则为NULL</returns>
        TItem Get<TItem>();

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        /// <typeparam name="TItem">要查找的对象的类型</typeparam>
        /// <param name="key">Key，唯一标识</param>
        /// <returns>如果找到对象直接返回，则为NULL</returns>
        /// <exception cref="ArgumentNullException">Key为null.</exception>
        TItem Get<TItem>(object key);

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        /// <typeparam name="TItem">要查找的对象的类型</typeparam>
        /// <param name="key">Key，唯一标识</param>
        /// <param name="options">查找选项(一个枚举)</param>
        /// <returns>如果找到对象直接返回，则为NULL</returns>
        /// <exception cref="ArgumentNullException">Key为null.</exception>
        /// <exception cref="ArgumentException">SearchMode选项不是有效枚举值</exception>
        TItem Get<TItem>(object key, SearchMode options);

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        /// <param name="key">Key，唯一标识</param>
        /// <returns>如果找到对象直接返回，则为NULL</returns>
        /// <exception cref="ArgumentNullException">Key 为 null.</exception>
        object Get(object key);

        /// <summary>
        /// 从定位器中获取一个对象
        /// </summary>
        /// <param name="key">Key，唯一标识</param>
        /// <param name="options">查找选项(一个枚举)</param>
        /// <returns>如果找到对象直接返回，则为NULL</returns>
        /// <exception cref="ArgumentNullException">Key 为 null.</exception>
        /// <exception cref="ArgumentException">SearchMode选项不是有效枚举值</exception>
        object Get(object key, SearchMode options);
    }
}