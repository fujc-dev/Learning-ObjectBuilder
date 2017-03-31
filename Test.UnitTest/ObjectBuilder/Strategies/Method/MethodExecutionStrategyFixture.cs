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
using System.Reflection;
using NUnit.Framework;

namespace Microsoft.Practices.ObjectBuilder
{
	[TestFixture]
	public class MethodExecutionStrategyFixture
	{
		#region Success Cases

		[Test]
		public void StrategyCallsParameterlessMethod()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("ParameterlessMethod", new MethodCallInfo("ParameterlessMethod"));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.IsTrue(obj.ParameterlessWasCalled);
		}

		[Test]
		public void StrategyDoesWorkBasedOnConcreteTypeInsteadOfPassedType()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("ParameterlessMethod", new MethodCallInfo("ParameterlessMethod"));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof(MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (object), obj, null);

			Assert.IsTrue(obj.ParameterlessWasCalled);
		}

		[Test]
		public void StrategyCallsMethodWithDirectValues()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("IntMethod", new MethodCallInfo("IntMethod", 32));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.AreEqual(32, obj.IntValue);
		}

		[Test]
		public void StrategyCallsMethodsUsingIParameterValues()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("IntMethod", new MethodCallInfo("IntMethod", new ValueParameter<int>(32)));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.AreEqual(32, obj.IntValue);
		}

		[Test]
		public void CanCallMultiParameterMethods()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("MultiParamMethod", new MethodCallInfo("MultiParamMethod", 1.0, "foo"));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.AreEqual(1.0, obj.MultiDouble);
			Assert.AreEqual("foo", obj.MultiString);
		}

		[Test]
		public void StrategyCallsMultipleMethodsAndCallsThemInOrder()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("ParameterlessMethod", new MethodCallInfo("ParameterlessMethod"));
			policy.Methods.Add("IntMethod", new MethodCallInfo("IntMethod", new ValueParameter<int>(32)));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.AreEqual(1, obj.CallOrderParameterless);
			Assert.AreEqual(2, obj.CallOrderInt);
		}

		#endregion

		#region Failure Cases

		[Test]
		public void StrategyWithNoObjectDoesntThrow()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			ctx.InnerChain.Add(strategy);

			ctx.HeadOfChain.BuildUp(ctx, typeof (object), null, null);
		}

		[Test]
		public void StrategyDoesNothingWithNoPolicy()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);

			Assert.IsFalse(obj.ParameterlessWasCalled);
		}

		[Test]
		public void SettingPolicyForMissingMethodDoesntThrow()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("NonExistantMethod", new MethodCallInfo("NonExistantMethod"));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);
		}

		[Test]
		public void SettingPolicyForWrongParameterCountDoesntThrow()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			policy.Methods.Add("ParameterlessMethod", new MethodCallInfo("ParameterlessMethod", 123));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof (MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof (MockObject), obj, null);
		}

		[Test]
		[ExpectedException(typeof(IncompatibleTypesException))]
		public void IncompatibleTypesThrows()
		{
			MethodExecutionStrategy strategy = new MethodExecutionStrategy();
			MockBuilderContext ctx = new MockBuilderContext();
			MockObject obj = new MockObject();
			ctx.InnerChain.Add(strategy);

			MethodPolicy policy = new MethodPolicy();
			MethodInfo mi = typeof(MockObject).GetMethod("IntMethod");
			policy.Methods.Add("IntMethod", new MethodCallInfo( mi, new ValueParameter<string>(String.Empty)));
			ctx.Policies.Set<IMethodPolicy>(policy, typeof(MockObject), null);

			ctx.HeadOfChain.BuildUp(ctx, typeof(MockObject), obj, null);
		}


		#endregion

		// ---------------------------------------------------------------------
		// Test List
		// ---------------------------------------------------------------------
		// TODO: Call method with non-void return values, and do something with the value
		// TODO: Testing with ref & out parameters
		// TODO: Statics

		#region Support Classes

		public interface IFoo
		{
		}

		public interface IBar
		{
		}

		public class FooBar : IFoo, IBar
		{
		}

		public class MockObject
		{
			private int currentOrder = 0;

			public bool ParameterlessWasCalled = false;
			public bool AmbiguousWasCalled = false;
			public int IntValue = 0;
			public int CallOrderParameterless = 0;
			public int CallOrderInt = 0;
			public double MultiDouble = 0.0;
			public string MultiString = null;

			public void ParameterlessMethod()
			{
				CallOrderParameterless = ++currentOrder;
				ParameterlessWasCalled = true;
			}

			public void IntMethod(int intValue)
			{
				CallOrderInt = ++currentOrder;
				IntValue = intValue;
			}

			public void AmbiguousMethod(IFoo foo)
			{
				AmbiguousWasCalled = true;
			}

			public void AmbiguousMethod(IBar bar)
			{
				AmbiguousWasCalled = true;
			}

			public void MultiParamMethod(double d, string s)
			{
				MultiDouble = d;
				MultiString = s;
			}
		}

		#endregion
	}
}
