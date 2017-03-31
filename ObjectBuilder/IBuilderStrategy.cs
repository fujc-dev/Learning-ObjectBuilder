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

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 代表一个策略。策略需要同时支持建立和销毁，策略需要同时支持建立和拆除。虽然你可以直接实现这个接口，你也可以选择使用<see cref="BuilderStrategy"/> 作为基类，你的策略，因为这类提供了有用的辅助方法，使得支持建立和拆卸
    /// </summary>
    public interface IBuilderStrategy
    {
        /// <summary>
        /// 用于构建对象在责任链中调用
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <param name="typeToBuild">需要创建的对象的类型</param>
        /// <param name="existing">一般默认传null对象创建器会在生成链中创建一个新的对象实例，如果不为null则将运行生成链的现有对象</param>
        /// <param name="idToBuild">需要创建的对象的唯一编号</param>
        /// <returns>创建的对象</returns>
        object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild);

        /// <summary>
        /// 用于摧毁对象在责任链中调用
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <param name="item">需要销毁的对象实例</param>
        /// <returns>返回当前销毁的对象实例</returns>
        object TearDown(IBuilderContext context, object item);
    }
}