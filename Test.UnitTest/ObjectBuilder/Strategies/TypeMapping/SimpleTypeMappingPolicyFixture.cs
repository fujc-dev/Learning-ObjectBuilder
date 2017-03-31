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

using NUnit.Framework;

namespace Microsoft.Practices.ObjectBuilder
{
	[TestFixture]
	public class SimpleTypeMappingPolicyFixture
	{
		[Test]
		public void PolicyReturnsGivenType()
		{
			TypeMappingPolicy policy = new TypeMappingPolicy(typeof (Foo), null);

			Assert.AreEqual(new DependencyResolutionLocatorKey(typeof (Foo), null), policy.Map(new DependencyResolutionLocatorKey()));
		}

		private class Foo
		{
		}
	}
}
