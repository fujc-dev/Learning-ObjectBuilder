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
            //Pre-Creation Strategy
            //Pre-Creation对象被建立之前的初始动作，参与此阶段的Strategy有TypeMappingStrategy、PropertyReflectionStrategy、ConstructorReflectionStrategy、MethodReflectionStrategy、SingletonStrategy
            Strategies.AddNew<TypeMappingStrategy>(BuilderStage.PreCreation);
            Strategies.AddNew<SingletonStrategy>(BuilderStage.PreCreation);
            Strategies.AddNew<ConstructorReflectionStrategy>(BuilderStage.PreCreation);
            Strategies.AddNew<PropertyReflectionStrategy>(BuilderStage.PreCreation);
            Strategies.AddNew<MethodReflectionStrategy>(BuilderStage.PreCreation);
            //Creation Strategy 
            //Creation类型的策略主要工作在于创建对象，它会利用Pre-Creation Strategy所准备的参数来建立对象，ObjectBuilder就是运用Pre-Creation的ConstructorReflectionStrategy和CreationStrategy来完成构造函数注入的
            Strategies.AddNew<CreationStrategy>(BuilderStage.Creation);
            //Initalization Strategy 
            //当对象创建后，会进入初始化阶段，这个就是Initalization Strategy阶段，在此阶段中PropertySetterStrategy和PropertyReflectionStrategy配合完成属性的注入，
            //而MethodExecutionStrategy和MethodReflectionStrategy配合完成方法的注入；
            Strategies.AddNew<PropertySetterStrategy>(BuilderStage.Initialization);
            Strategies.AddNew<MethodExecutionStrategy>(BuilderStage.Initialization);
            //Post-Initalization Strategy 
            //在建立对象和完成初始化之后，就进入了Post-Initalization Strategy阶段，在此阶段BuilderAwareStrategy会探询已建立的对象是否实现了IBuilderAware接口， 是的话就调用IBuilderAware.OnBuildUp方法。
            Strategies.AddNew<BuilderAwareStrategy>(BuilderStage.PostInitialization);

            Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());

            if (configurator != null)
                configurator.ApplyConfiguration(this);
        }
    }
}