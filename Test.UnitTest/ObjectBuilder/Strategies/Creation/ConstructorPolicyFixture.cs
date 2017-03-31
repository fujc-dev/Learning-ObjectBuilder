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
	public class ConstructorPolicyFixture
	{
		[Test]
		public void GetConstructorReturnsTheCorrectOneWhenParamsPassedThruAddParameter()
		{
			ConstructorPolicy policy = new ConstructorPolicy();

			policy.AddParameter(new ValueParameter<int>(5));
			ConstructorInfo actual = policy.SelectConstructor(new MockBuilderContext(), typeof(MockObject), null);
			ConstructorInfo expected = typeof(MockObject).GetConstructors()[1];

			Assert.AreSame(expected, actual);
		}

		[Test]
		public void GetConstructorReturnsTheCorrectOneWhenParamsPassedThruCtor()
		{
			ConstructorPolicy policy = new ConstructorPolicy(new ValueParameter<int>(5));

			ConstructorInfo actual = policy.SelectConstructor(new MockBuilderContext(), typeof(MockObject), null);
			ConstructorInfo expected = typeof(MockObject).GetConstructors()[1];

			Assert.AreSame(expected, actual);
		}

		private class MockObject
		{
			public MockObject()
			{
			}

			public MockObject(int val)
			{
			}
		}
	}
}
