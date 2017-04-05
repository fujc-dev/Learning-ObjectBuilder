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

using System.Collections.Generic;
namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 实现 <see cref="IMethodPolicy"/>.
    /// </summary>
    public class MethodPolicy : IMethodPolicy
    {
        private Dictionary<string, IMethodCallInfo> methods = new Dictionary<string, IMethodCallInfo>();

        /// <summary>
        /// 需要执行的方法集合
        /// </summary>
        public Dictionary<string, IMethodCallInfo> Methods
        {
            get { return methods; }
        }
    }
}
