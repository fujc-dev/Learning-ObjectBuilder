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
    /// 表示可以配置生成器的对象
    /// </summary>
    /// <typeparam name="TStageEnum">这个枚举的泛型表示类型创建策略</typeparam>
    public interface IBuilderConfigurator<TStageEnum>
    {
        /// <summary>
        /// 将配置应用到IBuilder对象生成器
        /// </summary>
        /// <param name="builder">(生成器将配置应用到)Builder对象生成器</param>
        void ApplyConfiguration(IBuilder<TStageEnum> builder);
    }
}