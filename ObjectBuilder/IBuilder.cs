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
    /// 对象构建器，对象创建框架的主接口
    /// </summary>
    /// <typeparam name="TStageEnum">ObjectBuilder创建对象实例的策略的执行顺序</typeparam>
    public interface IBuilder<TStageEnum>
    {
        /// <summary>
        /// 创建对象时的其他临时策略/政策(包含属性、构造函数、方法等，当然这些都是通过注入的方式去创建对象，但是这个对象需要提供给IBuilder，然后给使用者创建一个符合规则的对象实例)
        /// </summary>
        PolicyList Policies { get; }

        /// <summary>
        /// 创建对象和释放对象操作的策略(这个是一个策略的集合，所有的策略组成一个责任链，我们通常叫它策略责任链)
        /// </summary>
        StrategyList<TStageEnum> Strategies { get; }


        /// <summary>
        /// 执行创建对象操作
        /// </summary>
        /// <param name="locator">生成对象的定位器，当对象已存在时直接在定位器中获取</param>
        /// <param name="typeToBuild">需要创建的对象的类型</param>
        /// <param name="idToBuild">需要创建的对象的唯一编号</param>
        /// <param name="existing">已存在的对象，如果是null，则将在CreationStrategy创建。</param>
        /// <param name="transientPolicies">临时构建对象政策</param>
        /// <returns>创建的对象</returns>
        object BuildUp(IReadWriteLocator locator, Type typeToBuild, string idToBuild, object existing, params PolicyList[] transientPolicies);


        /// <summary>
        /// 执行创建对象操作
        /// </summary>
        /// <typeparam name="TTypeToBuild">对象创建的泛型类型，需要创建的对象的类型</typeparam>
        /// <param name="locator">生成对象的定位器，当对象已存在时直接在定位器中获取</param>
        /// <param name="idToBuild">需要创建的对象的唯一编号</param>
        /// <param name="existing">已存在的对象，如果是null，则将在CreationStrategy创建。</param>
        /// <param name="transientPolicies">临时构建对象政策</param>
        /// <returns>创建的对象</returns>
        TTypeToBuild BuildUp<TTypeToBuild>(IReadWriteLocator locator, string idToBuild, object existing, params PolicyList[] transientPolicies);

        /// <summary>
        /// 执行对象的销毁操作
        /// </summary>
        /// <typeparam name="TItem">对象创建的泛型类型，需要销毁的对象的类型</typeparam>
        /// <param name="locator">生成对象的定位器，当对象已存在时直接在定位器中获取</param>
        /// <param name="item">需要销毁的对象实例</param>
        /// <returns>返回当前销毁的对象实例</returns>
        TItem TearDown<TItem>(IReadWriteLocator locator, TItem item);
    }
}