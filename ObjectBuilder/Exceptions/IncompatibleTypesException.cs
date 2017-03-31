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
    /// 无法解决依赖项，因为解析类型与注入类型不兼容
    /// </summary>
    [Serializable]
    public class IncompatibleTypesException : Exception
    {
        /// <summary>
        /// 初始化异常<see cref="IncompatibleTypesException"/>类
        /// </summary>
        public IncompatibleTypesException()
        {
        }

        /// <summary>
        /// 初始化异常<see cref="IncompatibleTypesException"/>类
        /// </summary>
        /// <param name="message">解释异常原因的错误消息。</param>
        public IncompatibleTypesException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 初始化异常<see cref="IncompatibleTypesException"/>类
        /// </summary>
        /// <param name="message">解释异常原因的错误消息。</param>
        /// <param name="exception">导致当前异常的异常；如果未指定内部异常，则是一个 null 引用（在 Visual Basic 中为 Nothing）。</param>
        public IncompatibleTypesException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>
        /// 初始化异常<see cref="IncompatibleTypesException"/>类
        /// </summary>
        protected IncompatibleTypesException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
