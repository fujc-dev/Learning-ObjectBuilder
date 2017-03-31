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
using System.Runtime.Serialization;
using System.Reflection;
using System.Globalization;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 使用依赖注入属性的无效组合
    /// </summary>
    [Serializable]
    public class InvalidAttributeException : Exception
    {
        /// <summary>
        /// 初始化异常<see cref="InvalidAttributeException"/>类
        /// </summary>
        public InvalidAttributeException()
        {
        }

        /// <summary>
        /// 初始化异常<see cref="InvalidAttributeException"/>类
        /// </summary>
        /// <param name="message">解释异常原因的错误消息。</param>
        public InvalidAttributeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 初始化异常<see cref="InvalidAttributeException"/>类
        /// </summary>
        /// <param name="message">解释异常原因的错误消息。</param>
        /// <param name="exception">导致当前异常的异常；如果未指定内部异常，则是一个 null 引用（在 Visual Basic 中为 Nothing）。</param>
        public InvalidAttributeException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>
        /// 初始化异常<see cref="InvalidAttributeException"/>类
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="memberName">成员名称</param>
        public InvalidAttributeException(Type type, string memberName)
            : base(String.Format(CultureInfo.CurrentCulture, Properties.Resources.InvalidAttributeCombination, type, memberName))
        {
        }

        /// <summary>
        /// 初始化异常<see cref="InvalidAttributeException"/>类
        /// </summary>
        protected InvalidAttributeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
