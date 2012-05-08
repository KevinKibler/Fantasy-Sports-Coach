using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSquared.FantasySportsCoach.Common;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace KSquared.FantasySportsCoach.Common.Tests
{
	[TestFixture]
	public static class CommonTests
	{
		private static ObservableCollectionEx<int> observableCollectionEx = null;

		[SetUp]
		public static void SetupTests()
		{
			CommonTests.observableCollectionEx = new ObservableCollectionEx<int>(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
		}

		[Test]
		public static void TestAddRange()
		{
			CommonTests.observableCollectionEx.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action, "Action should be add");
				Assert.IsNull(e.OldItems, "old items should be null");
				Assert.AreEqual(-1, e.OldStartingIndex, "old items starting index should be -1");
				Assert.IsNotNull(e.NewItems, "new items should not be null");
				Assert.AreEqual(3, e.NewItems.Count, "there should be 3 new items");
				Assert.AreEqual(100, e.NewItems[0], "items[0] should be 100");
				Assert.AreEqual(101, e.NewItems[1], "items[1] should be 101");
				Assert.AreEqual(102, e.NewItems[2], "items[2] should be 102");
				Assert.AreEqual(-1, e.NewStartingIndex, "new starting index should be -1");
				Assert.AreEqual(13, CommonTests.observableCollectionEx.Count, "collection count should now be 13");
			};

			CommonTests.observableCollectionEx.AddRange(new[] { 100, 101, 102 });
		}

		[Test]
		public static void TestAddRangePreview()
		{
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action, "Action should be add");
				Assert.IsNull(e.OldItems, "old items should be null");
				Assert.AreEqual(-1, e.OldStartingIndex, "old items starting index should be -1");
				Assert.IsNotNull(e.NewItems, "new items should not be null");
				Assert.AreEqual(3, e.NewItems.Count, "there should be 3 new items");
				Assert.AreEqual(100, e.NewItems[0], "items[0] should be 100");
				Assert.AreEqual(101, e.NewItems[1], "items[1] should be 101");
				Assert.AreEqual(102, e.NewItems[2], "items[2] should be 102");
				Assert.AreEqual(-1, e.NewStartingIndex, "new starting index should be -1");
				Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should be 10");
			};

			bool collectionChangedEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
			{
				collectionChangedEventRaised = true;
			};

			CommonTests.observableCollectionEx.AddRange(new[] { 100, 101, 102 });
			Assert.IsTrue(collectionChangedEventRaised, "collection changed event was should have been raised");
			Assert.AreEqual(13, CommonTests.observableCollectionEx.Count, "collection count should now be 13");
		}

		[Test]
		public static void TestAddRangePreviewCancel()
		{
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action, "Action should be add");
				Assert.IsNull(e.OldItems, "old items should be null");
				Assert.AreEqual(-1, e.OldStartingIndex, "old items starting index should be -1");
				Assert.IsNotNull(e.NewItems, "new items should not be null");
				Assert.AreEqual(3, e.NewItems.Count, "there should be 3 new items");
				Assert.AreEqual(100, e.NewItems[0], "items[0] should be 100");
				Assert.AreEqual(101, e.NewItems[1], "items[1] should be 101");
				Assert.AreEqual(102, e.NewItems[2], "items[2] should be 102");
				Assert.AreEqual(-1, e.NewStartingIndex, "new starting index should be -1");
				Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should be 10");

				e.Cancel = true;
			};

			bool collectionChangedEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
			{
				collectionChangedEventRaised = true;
			};

			CommonTests.observableCollectionEx.AddRange(new[] { 100, 101, 102 });
			Assert.IsTrue(!collectionChangedEventRaised, "collection changed event should not have been raised");
			Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should still be 10");
		}
	}
}
