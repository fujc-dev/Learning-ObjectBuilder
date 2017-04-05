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
using System.Text;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 事件数据发送事件<see cref="ILifetimeContainer"/>.
    /// </summary>
    public class LifetimeEventArgs : EventArgs
    {
        private object item;

        /// <summary>
        ///  生命周期内实例
        /// </summary>
        public object Item
        {
            get { return item; }
        }

        /// <summary>
        /// 实例化 <see cref="LifetimeEventArgs"/> 类
        /// </summary>
        /// <param name="item">生命周期内实例</param>
        public LifetimeEventArgs(object item)
        {
            this.item = item;
        }
    }
}
