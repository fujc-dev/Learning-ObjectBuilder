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
    /// 表示需要执行方法执行策略的方法的信息
    /// </summary>
    public class MethodCallInfo : IMethodCallInfo
    {
        private MethodInfo method;
        private string methodName;
        private List<IParameter> parameters;

        /// <summary>
        /// 实例化 <see cref="MethodCallInfo"/>类
        /// </summary>
        /// <param name="methodName">需要执行的方法名称</param>
        public MethodCallInfo(string methodName)
            : this(methodName, (MethodInfo)null, null)
        {
        }

        /// <summary>
        /// 实例化 <see cref="MethodCallInfo"/>类
        /// </summary>
        /// <param name="methodName">需要执行的方法名称</param>
        /// <param name="parameters">参数列表</param>
        public MethodCallInfo(string methodName, params object[] parameters)
            : this(methodName, null, ObjectsToIParameters(parameters))
        {
        }

        /// <summary>
        /// 实例化 <see cref="MethodCallInfo"/>类
        /// </summary>
        /// <param name="methodName">需要执行的方法名称</param>
        /// <param name="parameters">参数列表</param>
        public MethodCallInfo(string methodName, params IParameter[] parameters)
            : this(methodName, null, parameters)
        {
        }

        /// <summary>
        /// 实例化 <see cref="MethodCallInfo"/>类
        /// </summary>
        /// <param name="methodName">需要执行的方法名称</param>
        /// <param name="parameters">参数列表</param>
        public MethodCallInfo(string methodName, IEnumerable<IParameter> parameters)
            : this(methodName, null, parameters)
        {
        }

        /// <summary>
        /// 实例化 <see cref="MethodCallInfo"/>类
        /// </summary>
        /// <param name="method">需要执行的方法信息</param>
        public MethodCallInfo(MethodInfo method)
            : this(null, method, null)
        {
        }

        /// <summary>
        /// 实例化 <see cref="MethodCallInfo"/>类
        /// </summary>
        /// <param name="method">需要执行的方法信息</param>
        /// <param name="parameters">参数列表</param>
        public MethodCallInfo(MethodInfo method, params IParameter[] parameters)
            : this(null, method, parameters)
        {
        }

        /// <summary>
        /// 实例化 <see cref="MethodCallInfo"/>类
        /// </summary>
        /// <param name="method">需要执行的方法信息</param>
        /// <param name="parameters">参数列表</param>
        public MethodCallInfo(MethodInfo method, IEnumerable<IParameter> parameters)
            : this(null, method, parameters)
        {
        }

        //方法名称，信息和参数列表。
        private MethodCallInfo(string methodName, MethodInfo method, IEnumerable<IParameter> parameters)
        {
            this.methodName = methodName;
            this.method = method;
            this.parameters = new List<IParameter>();

            if (parameters != null)
                foreach (IParameter param in parameters)
                    this.parameters.Add(param);
        }

        /// <summary>
        /// 获取注入的方法、选择与参数相符合的的方法
        /// </summary>
        public MethodInfo SelectMethod(IBuilderContext context, Type type, string id)
        {
            if (method != null)
                return method;

            List<Type> types = new List<Type>();

            foreach (IParameter param in parameters)
                types.Add(param.GetParameterType(context));

            return type.GetMethod(methodName, types.ToArray());
        }

        /// <summary>
        /// 获取调用方法的参数列表
        /// </summary>
        public object[] GetParameters(IBuilderContext context, Type type, string id, MethodInfo method)
        {
            List<object> values = new List<object>();

            foreach (IParameter param in parameters)
                values.Add(param.GetValue(context));

            return values.ToArray();
        }

        private static IEnumerable<IParameter> ObjectsToIParameters(object[] parameters)
        {
            List<IParameter> results = new List<IParameter>();

            if (parameters != null)
                foreach (object parameter in parameters)
                    results.Add(new ValueParameter(parameter.GetType(), parameter));

            return results.ToArray();
        }
    }
}
