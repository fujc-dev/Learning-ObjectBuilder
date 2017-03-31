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

using System.Diagnostics;
namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 派生自 <see cref="IBuilder{TStageEnum}"/> 使用 <see cref="BuilderStage"/>
    /// 作为构建对象的策略，其中包含了所有默认的ObjectBuidler策略
    /// </summary>
    public class Builder : BuilderBase<BuilderStage>
    {
        /// <summary>
        /// 实例化一个 <see cref="Builder"/> 类.
        /// </summary>
        public Builder()
            : this(null)
        {
        }

        /// <summary>
        /// 通过<see cref="IBuilderConfigurator{BuilderStage}"/>配置实例化一个 <see cref="Builder"/> 类.
        /// </summary>
        /// <param name="configurator">生成器配置对象接口</param>
        public Builder(IBuilderConfigurator<BuilderStage> configurator)
        {
            Strategies.AddNew<TypeMappingStrategy>(BuilderStage.PreCreation);
            Strategies.AddNew<SingletonStrategy>(BuilderStage.PreCreation);
            Strategies.AddNew<ConstructorReflectionStrategy>(BuilderStage.PreCreation);
            Strategies.AddNew<PropertyReflectionStrategy>(BuilderStage.PreCreation);
            Strategies.AddNew<MethodReflectionStrategy>(BuilderStage.PreCreation);
            Strategies.AddNew<CreationStrategy>(BuilderStage.Creation);
            Strategies.AddNew<PropertySetterStrategy>(BuilderStage.Initialization);
            Strategies.AddNew<MethodExecutionStrategy>(BuilderStage.Initialization);
            Strategies.AddNew<BuilderAwareStrategy>(BuilderStage.PostInitialization);

            Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());

            if (configurator != null)
                configurator.ApplyConfiguration(this);
        }
    }
}