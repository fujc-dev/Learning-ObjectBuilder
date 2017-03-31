//===============================================================================
// Microsoft patterns & practices
// ObjectBuilder Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using NUnit.Framework;

namespace Microsoft.Practices.ObjectBuilder
{
	[TestFixture]
	public class DependencyResolverFixture
	{
		[Test]
		public void CanResolveDependencyByType()
		{
			MockBuilderContext context = new MockBuilderContext();
			object obj = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), null), obj);
			DependencyResolver resolver = new DependencyResolver(context);

			Assert.AreSame(obj, resolver.Resolve(typeof(object), null, null, NotPresentBehavior.ReturnNull, SearchMode.Local));
		}

		[Test]
		public void CanResolveDependencyByTypeAndName()
		{
			MockBuilderContext context = new MockBuilderContext();
			object obj = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), obj);
			DependencyResolver resolver = new DependencyResolver(context);

			Assert.AreSame(obj, resolver.Resolve(typeof(object), null, "Foo", NotPresentBehavior.ReturnNull, SearchMode.Local));
		}

		[Test]
		public void ResolverUsesNamesToResolveDifferences()
		{
			MockBuilderContext context = new MockBuilderContext();
			DependencyResolver resolver = new DependencyResolver(context);

			object obj1 = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), null), obj1);

			object obj2 = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), "Foo"), obj2);

			object obj3 = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), "Bar"), obj3);

			Assert.AreSame(obj1, resolver.Resolve(typeof(object), null, null, NotPresentBehavior.ReturnNull, SearchMode.Local));
			Assert.AreSame(obj2, resolver.Resolve(typeof(object), null, "Foo", NotPresentBehavior.ReturnNull, SearchMode.Local));
			Assert.AreSame(obj3, resolver.Resolve(typeof(object), null, "Bar", NotPresentBehavior.ReturnNull, SearchMode.Local));
		}

		[Test]
		public void ResolverCanCreateObjectWhenNotPresent()
		{
			MockBuilderContext context = CreateContext();
			DependencyResolver resolver = new DependencyResolver(context);

			object obj = resolver.Resolve(typeof(object), null, null, NotPresentBehavior.CreateNew, SearchMode.Local);

			Assert.IsNotNull(obj);
			Assert.IsTrue(obj is object);
		}

		[Test]
		public void ResolverWillUseExistingObjectWhenPresentEvenForCreateFlag()
		{
			MockBuilderContext context = CreateContext();
			DependencyResolver resolver = new DependencyResolver(context);
			object obj = new object();
			context.Locator.Add(new DependencyResolutionLocatorKey(typeof(object), null), obj);

			Assert.AreSame(obj, resolver.Resolve(typeof(object), null, null, NotPresentBehavior.CreateNew, SearchMode.Local));
		}

		[Test]
		public void ResolverWillCreateOnceAndReturnExistingSubsequentlyForCreateFlag()
		{
			MockBuilderContext context = CreateContext();
			DependencyResolver resolver = new DependencyResolver(context);

			object obj1 = resolver.Resolve(typeof(object), null, null, NotPresentBehavior.CreateNew, SearchMode.Local);
			object obj2 = resolver.Resolve(typeof(object), null, null, NotPresentBehavior.CreateNew, SearchMode.Local);

			Assert.IsNotNull(obj1);
			Assert.AreSame(obj1, obj2);
		}

		[Test]
		public void ResolverWillCreateOnceAndReturnExistingWithNameSubsequentlyForCreateFlag()
		{
			MockBuilderContext context = CreateContext();
			DependencyResolver resolver = new DependencyResolver(context);

			object obj1 = resolver.Resolve(typeof(object), null, "Foo", NotPresentBehavior.CreateNew, SearchMode.Local);
			object obj2 = resolver.Resolve(typeof(object), null, "Foo", NotPresentBehavior.CreateNew, SearchMode.Local);

			Assert.IsNotNull(obj1);
			Assert.AreSame(obj1, obj2);
		}

		[Test]
		public void ResolverCanReturnNullWhenObjectNotPresent()
		{
			MockBuilderContext context = CreateContext();
			DependencyResolver resolver = new DependencyResolver(context);

			Assert.IsNull(resolver.Resolve(typeof(object), null, null, NotPresentBehavior.ReturnNull, SearchMode.Local));
		}

		[Test]
		public void ResolveCanCreateDifferentTypeThanAskedToResolve()
		{
			MockBuilderContext context = CreateContext();
			DependencyResolver resolver = new DependencyResolver(context);

			object obj = resolver.Resolve(typeof(IMockObject), typeof(MockObject), null, NotPresentBehavior.CreateNew, SearchMode.Local);
			object retrieved = resolver.Resolve(typeof(MockObject), null, null, NotPresentBehavior.ReturnNull, SearchMode.Local);

			Assert.IsNotNull(obj);
			Assert.IsTrue(obj is MockObject);
			Assert.AreSame(obj, retrieved);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullTypeToResolveThrows()
		{
			MockBuilderContext context = CreateContext();
			DependencyResolver resolver = new DependencyResolver(context);

			resolver.Resolve(null, typeof(object), null, NotPresentBehavior.ReturnNull, SearchMode.Local);
		}

		[Test]
		public void NullTypeToCreateUsesTypeToResolve()
		{
			MockBuilderContext context = CreateContext();
			DependencyResolver resolver = new DependencyResolver(context);

			object obj = resolver.Resolve(typeof(MockObject), null, null, NotPresentBehavior.CreateNew, SearchMode.Local);

			Assert.IsNotNull(obj);
			Assert.IsTrue(obj is MockObject);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void InvalidNotPresentValueThrows()
		{
			MockBuilderContext context = CreateContext();
			DependencyResolver resolver = new DependencyResolver(context);

			resolver.Resolve(typeof(object), typeof(object), null, (NotPresentBehavior)254, SearchMode.Local);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullContextThrows()
		{
			new DependencyResolver(null);
		}

		private MockBuilderContext CreateContext()
		{
			MockBuilderContext result = new MockBuilderContext();
			result.InnerChain.Add(new SingletonStrategy());
			result.InnerChain.Add(new CreationStrategy());
			result.Policies.SetDefault<ICreationPolicy>(new DefaultCreationPolicy());
			result.Policies.SetDefault<ISingletonPolicy>(new SingletonPolicy(true));
			return result;
		}

		interface IMockObject { }

		class MockObject : IMockObject { }
	}
}
