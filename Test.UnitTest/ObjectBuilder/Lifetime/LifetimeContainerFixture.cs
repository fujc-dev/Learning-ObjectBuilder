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
using NUnit.Framework;

namespace Microsoft.Practices.ObjectBuilder
{
	[TestFixture]
	public class LifetimeContainerFixture
	{
		[Test]
		public void ContainerEnsuresObjectsWontBeCollected()
		{
			ILifetimeContainer container = new LifetimeContainer();
			MockDisposableObject mdo = new MockDisposableObject();
			WeakReference wref = new WeakReference(mdo);

			container.Add(mdo);
			mdo = null;
			GC.Collect();

			Assert.AreEqual(1, container.Count);
			mdo = wref.Target as MockDisposableObject;
			Assert.IsNotNull(mdo);
			Assert.IsFalse(mdo.WasDisposed);
		}

		[Test]
		public void DisposingContainerDisposesOwnedObjects()
		{
			ILifetimeContainer container = new LifetimeContainer();
			MockDisposableObject mdo = new MockDisposableObject();

			container.Add(mdo);
			container.Dispose();

			Assert.IsTrue(mdo.WasDisposed);
		}

		[Test]
		public void RemovingItemsFromContainerDoesNotDisposeThem()
		{
			ILifetimeContainer container = new LifetimeContainer();
			MockDisposableObject mdo = new MockDisposableObject();

			container.Add(mdo);
			container.Remove(mdo);
			container.Dispose();

			Assert.IsFalse(mdo.WasDisposed);
		}

		[Test]
		public void CanEnumerateItemsInContainer()
		{
			ILifetimeContainer container = new LifetimeContainer();
			MockDisposableObject mdo = new MockDisposableObject();

			container.Add(mdo);

			int count = 0;
			bool foundMdo = false;

			foreach (object obj in container)
			{
				count++;

				if (ReferenceEquals(mdo, obj))
					foundMdo = true;
			}

			Assert.AreEqual(1, count);
			Assert.IsTrue(foundMdo);
		}

		[Test]
		public void CanDetermineIfLifetimeContainerContainsObject()
		{
			ILifetimeContainer container = new LifetimeContainer();
			object obj = new object();

			container.Add(obj);

			Assert.IsTrue(container.Contains(obj));
		}

		[Test]
		public void RemovingNonContainedItemDoesNotThrow()
		{
			ILifetimeContainer container = new LifetimeContainer();

			container.Remove(new object());
		}

		[Test]
		public void DisposingItemsFromContainerDisposesInReverseOrderAdded()
		{
			ILifetimeContainer container = new LifetimeContainer();
			DisposeOrderCounter obj1 = new DisposeOrderCounter();
			DisposeOrderCounter obj2 = new DisposeOrderCounter();
			DisposeOrderCounter obj3 = new DisposeOrderCounter();

			container.Add(obj1);
			container.Add(obj2);
			container.Add(obj3);

			container.Dispose();

			Assert.AreEqual(1, obj3.DisposePosition);
			Assert.AreEqual(2, obj2.DisposePosition);
			Assert.AreEqual(3, obj1.DisposePosition);
		}

		private class DisposeOrderCounter : IDisposable
		{
			private static int count = 0;
			public int DisposePosition;

			public void Dispose()
			{
				DisposePosition = ++count;
			}
		}

		private class MockDisposableObject : IDisposable
		{
			public bool WasDisposed = false;

			public void Dispose()
			{
				WasDisposed = true;
			}
		}
	}
}
