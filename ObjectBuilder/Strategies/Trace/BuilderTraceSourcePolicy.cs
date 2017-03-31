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
using System.Diagnostics;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 实现 <see cref="IBuilderTracePolicy"/> 通过<see cref="TraceSource"/>记录跟踪消息 
    /// </summary>
    public class BuilderTraceSourcePolicy : IBuilderTracePolicy
    {
        TraceSource traceSource;

        /// <summary>
        /// 实例化 <see cref="BuilderTraceSourcePolicy"/> 类
        /// </summary>
        public BuilderTraceSourcePolicy(TraceSource traceSource)
        {
            this.traceSource = traceSource;
        }

        /// <summary>
        ///  使用指定的对象数组和格式化信息，将信息性消息写入 System.Diagnostics.TraceSource.Listeners 集合中的跟踪侦听器中。
        /// </summary>
        public void Trace(string format, params object[] args)
        {
            traceSource.TraceInformation(format, args);
        }
    }
}
