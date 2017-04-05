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
    /// 实现 <see cref="ILifetimeContainer"/>.
    /// </summary>
    public class LifetimeContainer : ILifetimeContainer
    {
        private List<object> items = new List<object>();

        /// <summary>
        /// 返回生命周期容器中的引用数
        /// </summary>
        public int Count
        {
            get { return items.Count; }
        }

        /// <summary>
        /// 添加一个对象实例到生命周期容器中
        /// </summary>
        /// <param name="item">要添加的项目</param>
        public void Add(object item)
        {
            items.Add(item);
        }

        /// <summary>
        /// 检测给定对象是否在生命周期容器中
        /// </summary>
        /// <param name="item">要检测的项目</param>
        /// <returns>如果对象包含在生命周期容器中，则返回true；否则返回false。</returns>
        public bool Contains(object item)
        {
            return items.Contains(item);
        }

        /// <summary>
        /// 销毁容器，以及容器中包含的任何对象
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 销毁对象。
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                List<object> itemsCopy = new List<object>(items);
                itemsCopy.Reverse();

                foreach (object o in itemsCopy)
                {
                    IDisposable d = o as IDisposable;

                    if (d != null)
                        d.Dispose();
                }

                items.Clear();
            }
        }

        /// <summary>
        /// 从生命周期容器中移除给定对象
        /// </summary>
        /// <param name="item">要删除的项目</param>
        public void Remove(object item)
        {
            if (!items.Contains(item))
                return;

            items.Remove(item);
        }

        /// <summary>
        /// 返回循环访问<see cref="List{T}"/> 的枚举数。
        /// </summary>
        /// <returns>用于 <see cref="List{T}"/> 的 <see cref="List{T}.Enumerator"/>。</returns>
        public IEnumerator<object> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
