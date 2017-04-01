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
    /// 派生自 <see cref="IBuilderStrategy"/> ，单例策略所指的单例概念不同，单例策略在这里的作用是充当短路器，它查看当前的定位器中是否已经存在要创建的对象，如果有，它就把对象返回，否则它把控制权移交给下一个策略
    /// </summary>
    public class SingletonStrategy : BuilderStrategy
    {
        /// <summary>
        /// 用于构建对象在责任链中调用
        /// </summary>
        /// <param name="context">操作上下文</param>
        /// <param name="typeToBuild">需要创建的对象的类型</param>
        /// <param name="existing">一般默认传null对象创建器会在生成链中创建一个新的对象实例，如果不为null则将运行生成链的现有对象</param>
        /// <param name="idToBuild">需要创建的对象的唯一编号</param>
        /// <returns>创建的对象</returns>
		public override object BuildUp(IBuilderContext context, Type typeToBuild, object existing, string idToBuild)
        {
            DependencyResolutionLocatorKey key = new DependencyResolutionLocatorKey(typeToBuild, idToBuild);  //

            if (context.Locator != null && context.Locator.Contains(key, SearchMode.Local)) //
            {
                TraceBuildUp(context, typeToBuild, idToBuild, "");
                return context.Locator.Get(key);
            }

            return base.BuildUp(context, typeToBuild, existing, idToBuild);
        }
    }
}