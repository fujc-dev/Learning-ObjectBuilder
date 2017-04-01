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
    /// 任何实现了IBuiderAware接口的对象，在这个阶段会得到一个OnBuilltUp的事件通知。同时在对象被卸载的时候会得到OnTearingDown的通知。激活通知事件就是BuilderAwareStrategy的工作
    /// </summary>
    public interface IBuilderAware
	{
		/// <summary>
		/// Called by the <see cref="BuilderAwareStrategy"/> when the object is being built up.
		/// </summary>
		/// <param name="id">The ID of the object that was just built up.</param>
		void OnBuiltUp(string id);

		/// <summary>
		/// Called by the <see cref="BuilderAwareStrategy"/> when the object is being torn down.
		/// </summary>
		void OnTearingDown();
	}
}
