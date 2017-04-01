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

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 对象未找到时的行为定义
    /// </summary>
    public enum NotPresentBehavior
    {
        /// <summary>
        /// 创建该对象
        /// </summary>
        CreateNew,

        /// <summary>
        /// 返回一个null值
        /// </summary>
        ReturnNull,

        /// <summary>
        /// 抛一个<see cref="DependencyMissingException"/>异常
        /// </summary>
        Throw,
    }
}
