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
using System.Runtime.Serialization;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 无法解决依赖项自定义异常
    /// </summary>
    [Serializable]
    public class DependencyMissingException : Exception
    {
        /// <summary>
        /// 实例化 <see cref="DependencyMissingException"/> 类
        /// </summary>
        public DependencyMissingException()
        {
        }

        /// <summary>
        /// 实例化 <see cref="DependencyMissingException"/> 类
        /// </summary>
        /// <param name="message">解释异常原因的错误消息。</param>
        public DependencyMissingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 实例化 <see cref="DependencyMissingException"/> 类
        /// </summary>
        /// <param name="message">解释异常原因的错误消息。</param>
        /// <param name="exception">导致当前异常的异常；如果未指定内部异常，则是一个 null 引用（在 Visual Basic 中为 Nothing）。</param>
        public DependencyMissingException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>
        /// 实例化 <see cref="DependencyMissingException"/> 类
        /// </summary>
        protected DependencyMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
