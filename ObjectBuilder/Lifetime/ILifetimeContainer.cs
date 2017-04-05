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
    /// 生命周期容器
    /// </summary>
    /// <remarks>
    /// 所有存储在Locator的对象都仅存储对象的弱引用，即Locator不直接与存储的对象发生关系，
    /// 因此，在Locator存储的对象过一段时间就会被GC自动收集，只有当其对象存储在生命周期容器中时，
    /// 该对象才不能被GC收集，应为生命周期存储器存储了对象的实例。
    /// </remarks>
    public interface ILifetimeContainer : IEnumerable<object>, IDisposable
    {
        /// <summary>
        /// 返回生命周期容器中的引用数
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 添加一个对象实例到生命周期容器中
        /// </summary>
        /// <param name="item">要添加的项目</param>
        void Add(object item);

        /// <summary>
        /// 检测给定对象是否在生命周期容器中
        /// </summary>
        /// <param name="item">要检测的项目</param>
        /// <returns>如果对象包含在生命周期容器中，则返回true；否则返回false。</returns>
        bool Contains(object item);

        /// <summary>
        /// 从生命周期容器中移除给定对象
        /// </summary>
        /// <param name="item">要删除的项目</param>
        void Remove(object item);
    }
}