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
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 简单的默认创建策略，它选择对象的第一个公共构造函数，使用生成器来解析/创建构造函数所需的任何参数。
    /// ObjectBuilder提供的构造构造函数处理二级策略
    /// </summary>
    public class DefaultCreationPolicy : ICreationPolicy
    {
        /// <summary>
        /// 选择用于创建对象的构造函数
        /// </summary>
        /// <param name="context">策略执行上下文</param>
        /// <param name="typeToBuild">需要创建的对象的类型</param>
        /// <param name="idToBuild">需要创建的对象的唯一编号</param>
        /// <returns>要使用的构造函数；如果找不到合适的构造函数，则返回null</returns>
        public ConstructorInfo SelectConstructor(IBuilderContext context, Type typeToBuild, string idToBuild)
        {
            ConstructorInfo[] constructors = typeToBuild.GetConstructors();

            if (constructors.Length > 0)
                return constructors[0];

            return null;
        }

        /// <summary>
        /// 获取要传递给构造函数的参数值
        /// </summary>
        /// <param name="context">策略执行上下文</param>
        /// <param name="type">需要创建的对象的类型</param>
        /// <param name="id">需要创建的对象的唯一编号</param>
        /// <param name="constructor">发现类构造函数的属性并提供对构造函数元数据的访问权</param>
        /// <returns>传递给构造函数的参数数组</returns>
		public object[] GetParameters(IBuilderContext context, Type type, string id, ConstructorInfo constructor)
        {
            ParameterInfo[] parms = constructor.GetParameters();
            object[] parmsValueArray = new object[parms.Length];

            for (int i = 0; i < parms.Length; ++i)
                parmsValueArray[i] = context.HeadOfChain.BuildUp(context, parms[i].ParameterType, null, id);

            return parmsValueArray;
        }
    }
}
