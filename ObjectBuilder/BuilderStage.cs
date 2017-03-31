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
	/// ObjectBuilder对象创建顺序
	/// </summary>
	public enum BuilderStage
	{
        /// <summary>
        /// 这个阶段的策略在创造之前运行。在此阶段所做的典型工作可能包括使用反射将策略设置到其他策略稍后使用的上下文中的策略。
        /// </summary>
        PreCreation,

        /// <summary>
        /// 在这个阶段中只有一个策略驱动的创建策略
        /// </summary>
        Creation,

        /// <summary>
        /// 在这个阶段的工作对创建对象的策略。在此阶段所做的典型工作可能包括设置注入和方法调用。
        /// </summary>
        Initialization,

        /// <summary>
        /// 这个阶段的策略是对已经初始化的对象进行工作。在这个阶段中完成的典型工作可能包括查找对象是否实现了一些通知接口，以便在初始化阶段完成时发现。
        /// </summary>
        PostInitialization
    }
}