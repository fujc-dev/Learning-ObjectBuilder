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
	public class LookupParameterFixture
	{
		[Test]
		public void ConstructorPolicyCanUseLookupToFindAnObject()
		{
			MockBuilderContext ctx = new MockBuilderContext();
			ctx.InnerLocator.Add("foo", 11);

			LookupParameter param = new LookupParameter("foo");

			Assert.AreEqual(11, param.GetValue(ctx));
			Assert.AreSame(typeof (int), param.GetParameterType(ctx));
		}
	}
}
