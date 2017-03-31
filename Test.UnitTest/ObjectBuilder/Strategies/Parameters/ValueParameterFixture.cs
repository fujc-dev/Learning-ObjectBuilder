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
	public class ValueParameterFixture
	{
		[Test]
		public void ValueParameterReturnsStoredTypeAndValue()
		{
			ValueParameter<int> x = new ValueParameter<int>(12);

			Assert.AreEqual(typeof (int), x.GetParameterType(null));
			Assert.AreEqual(12, x.GetValue(null));
		}
	}
}
