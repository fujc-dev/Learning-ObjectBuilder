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
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder
{
    /// <summary>
    /// 创建策略，其中选择的构造函数是由用户提供的参数派生的， ObjectBuilder提供的构造构造函数处理二级策略
    /// </summary>
    public class ConstructorPolicy : ICreationPolicy
    {
        private ConstructorInfo constructor;  //发现类构造函数的属性并提供对构造函数元数据的访问权
        private List<IParameter> parameters = new List<IParameter>();  //当前对象构造函数的参数列表

        /// <summary>
        /// 实例化 <see cref="ConstructorPolicy"/> 类.
        /// </summary>
        public ConstructorPolicy() { }

        /// <summary>
        /// 实例化 <see cref="ConstructorPolicy"/> 类，用所提供的参数。将使用反射来发现要调用的构造函数
        /// </summary>
        /// <param name="parameters">传递给构造函数的参数</param>
        public ConstructorPolicy(params IParameter[] parameters)
        {
            foreach (IParameter parameter in parameters)
            {
                AddParameter(parameter);
            }
        }

        /// <summary>
        /// 实例化 <see cref="ConstructorPolicy"/> 类， 使用提供的 <see cref="ConstructorInfo"/> 和构造函数参数.
        /// </summary>
        /// <param name="constructor">发现类构造函数的属性并提供对构造函数元数据的访问权</param>
        /// <param name="parameters">传递给构造函数的参数</param>
        public ConstructorPolicy(ConstructorInfo constructor, params IParameter[] parameters)
            : this(parameters)
        {
            this.constructor = constructor;
        }

        /// <summary>
        /// 将参数添加到用于查找构造函数的参数列表中
        /// </summary>
        /// <param name="parameter">需要添加的参数</param>
        public void AddParameter(IParameter parameter)
        {
            parameters.Add(parameter);
        }

        /// <summary>
        /// 反射的形式创建对象的构造函数ConstructorInfo，对象
        /// </summary>
        /// <param name="context">策略执行上下文</param>
        /// <param name="type">需要创建的对象的类型</param>
        /// <param name="id">需要创建的对象的唯一编号</param>
        /// <returns>要使用的构造函数；如果找不到合适的构造函数，则返回null</returns>
        public ConstructorInfo SelectConstructor(IBuilderContext context, Type type, string id)
        {
            if (constructor != null)
            {
                return constructor;
            }
            List<Type> types = new List<Type>();

            foreach (IParameter parm in parameters)
            {
                types.Add(parm.GetParameterType(context));
            }
            return type.GetConstructor(types.ToArray());
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
            List<object> results = new List<object>();

            foreach (IParameter parm in parameters)
                results.Add(parm.GetValue(context));

            return results.ToArray();
        }
    }
}
