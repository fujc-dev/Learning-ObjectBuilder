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
    /// 一个保存弱引用对象的字典结构，表示将值存储为弱引用而不是强引用的字典，支持空值
    /// </summary>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    public class WeakRefDictionary<TKey, TValue>
    {
        /// <summary>
        /// 弱引用类型集合，表示弱引用，即在引用对象的同时仍然允许垃圾回收来回收该对象
        /// </summary>
        private Dictionary<TKey, WeakReference> inner = new Dictionary<TKey, WeakReference>();

        /// <summary>
        /// 实例化 <see cref="WeakRefDictionary{K,V}"/> 类
        /// </summary>
        public WeakRefDictionary()
        {
        }

        /// <summary>
        /// 从字典中检索值
        /// </summary>
        /// <param name="key">Key唯一标识</param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue result;

                if (TryGet(key, out result))
                    return result;

                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// 返回字典中项数的计数
        /// </summary>
        /// <remarks>由于字典中的项由弱引用持有，所以不能依赖计数值来保证通过枚举将发现的对象的数目。只把这个总数当作一个估计值</remarks>
        public int Count
        {
            get
            {
                CleanAbandonedItems();
                return inner.Count;
            }
        }

        /// <summary>
        /// 向字典添加新项
        /// </summary>
        /// <param name="key">Key唯一标识</param>
        /// <param name="value">添加的值</param>
        public void Add(TKey key, TValue value)
        {
            TValue dummy;
            if (TryGet(key, out dummy)) { throw new ArgumentException(Properties.Resources.KeyAlreadyPresentInDictionary); }
            inner.Add(key, new WeakReference(EncodeNullObject(value)));
        }

        /// <summary>
        /// 确定字典是否包含键的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            TValue dummy;
            return TryGet(key, out dummy);
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, WeakReference> kvp in inner)
            {
                object innerValue = kvp.Value.Target;

                if (innerValue != null)
                    yield return new KeyValuePair<TKey, TValue>(kvp.Key, DecodeNullObject<TValue>(innerValue));
            }
        }

        /// <summary>
        /// 从字典中移除项
        /// </summary>
        /// <param name="key">The key of the item to be removed.</param>
        /// <returns>Returns true if the key was in the dictionary; return false otherwise.</returns>
        public bool Remove(TKey key)
        {
            return inner.Remove(key);
        }

        /// <summary>
        /// 试图从字典中得到一个值
        /// </summary>
        /// <param name="key">Key唯一标识</param>
        /// <param name="value">添加的值</param>
        /// <returns>果值存在，则返回true；否则false</returns>
        public bool TryGet(TKey key, out TValue value)
        {
            value = default(TValue);
            WeakReference wr;

            if (!inner.TryGetValue(key, out wr))
                return false;

            object result = wr.Target;

            if (result == null)
            {
                inner.Remove(key);
                return false;
            }

            value = DecodeNullObject<TValue>(result);
            return true;
        }

        private void CleanAbandonedItems()
        {
            List<TKey> deadKeys = new List<TKey>();

            foreach (KeyValuePair<TKey, WeakReference> kvp in inner)
                if (kvp.Value.Target == null)
                    deadKeys.Add(kvp.Key);

            foreach (TKey key in deadKeys)
                inner.Remove(key);
        }

        private TObject DecodeNullObject<TObject>(object innerValue)
        {
            if (innerValue == typeof(NullObject))
                return default(TObject);
            else
                return (TObject)innerValue;
        }

        private object EncodeNullObject(object value)
        {
            if (value == null)
                return typeof(NullObject);
            else
                return value;
        }

        private class NullObject
        {
        }
    }
}