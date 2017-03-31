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
using System.Globalization;

namespace Microsoft.Practices.ObjectBuilder
{
	/// <summary>
	/// 依赖解析器工作者对象
	/// </summary>
	public class DependencyResolver
	{
		IBuilderContext context;

		/// <summary>
		/// DependencyResolver构造函数.
		/// </summary>
		/// <param name="context">对象创建策略上下文</param>
		public DependencyResolver(IBuilderContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			this.context = context;
		}

		/// <summary>
		/// 解析依赖
		/// </summary>
		/// <param name="typeToResolve">The type to be resolved.</param>
		/// <param name="typeToCreate">The type to be created, if the type cannot be resolved
		/// (and notPresent is set to <see cref="NotPresentBehavior.CreateNew"/>).</param>
		/// <param name="id">The ID of the object to be resolved. Pass null for the unnamed object.</param>
		/// <param name="notPresent">Flag to describe how to behave if the dependency is not found.</param>
		/// <param name="searchMode">Flag to describe whether searches are local only, or local and up.</param>
		/// <returns>The dependent object. If the object is not found, and notPresent
		/// is set to <see cref="NotPresentBehavior.ReturnNull"/>, will return null.</returns>
		public object Resolve(Type typeToResolve, Type typeToCreate, string id, NotPresentBehavior notPresent, SearchMode searchMode)
		{
			if (typeToResolve == null)
				throw new ArgumentNullException("typeToResolve");
			if (!Enum.IsDefined(typeof(NotPresentBehavior), notPresent))
				throw new ArgumentException(Properties.Resources.InvalidEnumerationValue, "notPresent");

			if (typeToCreate == null)
				typeToCreate = typeToResolve;

			DependencyResolutionLocatorKey key = new DependencyResolutionLocatorKey(typeToResolve, id);

			if (context.Locator.Contains(key, searchMode))
				return context.Locator.Get(key, searchMode);

			switch (notPresent)
			{
				case NotPresentBehavior.CreateNew:
					return context.HeadOfChain.BuildUp(context, typeToCreate, null, key.ID);

				case NotPresentBehavior.ReturnNull:
					return null;

				default:
					throw new DependencyMissingException(
						string.Format(CultureInfo.CurrentCulture,
						Properties.Resources.DependencyMissing, typeToResolve.ToString()));
			}
		}
	}
}
