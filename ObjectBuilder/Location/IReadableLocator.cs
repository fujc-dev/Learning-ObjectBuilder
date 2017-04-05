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
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// <see cref="IReadableLocator"/>定义了最上层的定位器接口方法，它扩展了<see cref="IEnumerable"/>接口，
    /// <see cref="IEnumerable"/>接口定义了一个获取<see cref="IEnumerator"/>迭代器的方法。
    /// <see cref="IReadableLocator"/>基本上具备了定位器的大部分功能。
    /// </summary>
    /// <remarks>
    /// <para>只读存储器、定位器，定位器是一个字典的键值，但它保持值与弱引用，所以定位一个对象不保持它活着。如果你想保持对象活着，你应该考虑使用<see cref="ILifetimeContainer"/>.</para>
    /// </remarks>
    public interface IReadableLocator : IEnumerable<KeyValuePair<object, object>>
    {
        /// <summary>
        /// 返回定位器中节点的数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 一个指向父节点的引用，父定位器
        /// </summary>
        IReadableLocator ParentLocator { get; }

        /// <summary>
        /// 如果定位器是只读的，返回true，表示定位器是否只读
        /// </summary>
        bool ReadOnly { get; }

        /// <summary>
        /// 查询定位器中是否已经存在指定键值的对象
        /// </summary>
        /// <param name="key">Key，唯一标识</param>
        /// <returns>如果该定位器包含用于该键的对象，则返回true；否则返回false</returns>
        /// <exception cref="ArgumentNullException">key为null时</exception>
        bool Contains(object key);

        /// <summary>
        /// 查询定位器中是否已经存在指定键值的对象，根据给出的搜索选项，表示是否要向上回溯继续寻找
        /// </summary>
        /// <param name="key">Key，唯一标识</param>
        /// <param name="options">查找选项(一个枚举)</param>
        /// <returns>如果该定位器包含用于该键的对象，则返回true；否则返回false</returns>
        /// <exception cref="ArgumentNullException">key为null时</exception>
        /// <exception cref="ArgumentException">SearchMode选项不是有效枚举值</exception>
        bool Contains(object key, SearchMode options);

        /// <summary>
        /// 使用谓词操作来查找包含给定对象的定位器
        /// </summary>
        /// <param name="predicate">自定义的条件检测是否包含<see cref="IReadableLocator"/>对象</param>
        /// <returns>返回一个新的定位器</returns>
        /// <exception cref="ArgumentNullException">Predicate为null</exception>
        IReadableLocator FindBy(Predicate<KeyValuePair<object, object>> predicate);

        /// <summary>
        /// 根据是否回溯的选项，使用谓词操作来查找包含对象的定位器
        /// </summary>
        /// <param name="options">查找选项(一个枚举)</param>
        /// <param name="predicate">自定义的条件检测是否包含<see cref="IReadableLocator"/>对象</param>
        /// <returns>返回一个新的定位器</returns>
        /// <exception cref="ArgumentNullException">Predicate为null</exception>
        /// <exception cref="ArgumentException">SearchMode选项不是有效枚举值</exception>
        IReadableLocator FindBy(SearchMode options, Predicate<KeyValuePair<object, object>> predicate);

        /// <summary>
        /// 从定位器中获取一个指定类型的对象
        /// </summary>
        /// <typeparam name="TItem">要查找的对象的类型</typeparam>
        /// <returns>如果找到对象直接返回，则为NULL</returns>
        TItem Get<TItem>();

        /// <summary>
        /// 从定位器中获取一个指定键值的对象
        /// </summary>
        /// <typeparam name="TItem">要查找的对象的类型</typeparam>
        /// <param name="key">Key，唯一标识</param>
        /// <returns>如果找到对象直接返回，则为NULL</returns>
        /// <exception cref="ArgumentNullException">Key为null.</exception>
        TItem Get<TItem>(object key);

        /// <summary>
        /// 根据选项条件，从定位器中获取一个指定类型的对象
        /// </summary>
        /// <typeparam name="TItem">要查找的对象的类型</typeparam>
        /// <param name="key">Key，唯一标识</param>
        /// <param name="options">查找选项(一个枚举)</param>
        /// <returns>如果找到对象直接返回，则为NULL</returns>
        /// <exception cref="ArgumentNullException">Key为null.</exception>
        /// <exception cref="ArgumentException">SearchMode选项不是有效枚举值</exception>
        TItem Get<TItem>(object key, SearchMode options);

        /// <summary>
        /// 给定对象键值获取对象的非泛型重载方法
        /// </summary>
        /// <param name="key">Key，唯一标识</param>
        /// <returns>如果找到对象直接返回，则为NULL</returns>
        /// <exception cref="ArgumentNullException">Key 为 null.</exception>
        object Get(object key);

        /// <summary>
        /// 给定对象键值带搜索条件的非泛型重载方法
        /// </summary>
        /// <param name="key">Key，唯一标识</param>
        /// <param name="options">查找选项(一个枚举)</param>
        /// <returns>如果找到对象直接返回，则为NULL</returns>
        /// <exception cref="ArgumentNullException">Key 为 null.</exception>
        /// <exception cref="ArgumentException">SearchMode选项不是有效枚举值</exception>
        object Get(object key, SearchMode options);
    }
}