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
	/// <summary>Tests for KSquare.FantasySportsCoach.Common.</summary>
	[TestFixture]
	public static class CommonTests
	{
		#region Setup

		private static ObservableCollectionEx<int> observableCollectionEx = null;

		/// <summary>Setup for each test.</summary>
		[SetUp]
		public static void SetupTests()
		{
			CommonTests.observableCollectionEx = new ObservableCollectionEx<int>(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
		}

		#endregion Setup
		#region Insert

		/// <summary>Test that adding an item to <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestInsert()
		{
			bool collectionChangedEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action, "Action should be add");
				Assert.IsNull(e.OldItems, "old items should be null");
				Assert.AreEqual(-1, e.OldStartingIndex, "old items starting index should be -1");
				Assert.IsNotNull(e.NewItems, "new items should not be null");
				Assert.AreEqual(1, e.NewItems.Count, "there should be 1 new items");
				Assert.AreEqual(100, e.NewItems[0], "NewItems[0] should be 100");
				Assert.AreEqual(10, e.NewStartingIndex, "new starting index should be 10");
				Assert.AreEqual(11, CommonTests.observableCollectionEx.Count, "collection count should now be 11");
				collectionChangedEventRaised = true;
			};

			CommonTests.observableCollectionEx.Add(100);
			Assert.IsTrue(collectionChangedEventRaised, "collection changed event was should have been raised");
			Assert.AreEqual(11, CommonTests.observableCollectionEx.Count, "collection count should now be 11");
		}

		/// <summary>Test that the preview event for adding an item to <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestAddInsertPreview()
		{
			bool collectionChangingEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action, "Action should be add");
				Assert.IsNull(e.OldItems, "old items should be null");
				Assert.AreEqual(-1, e.OldStartingIndex, "old items starting index should be -1");
				Assert.IsNotNull(e.NewItems, "new items should not be null");
				Assert.AreEqual(1, e.NewItems.Count, "there should be 1 new items");
				Assert.AreEqual(100, e.NewItems[0], "NewItems[0] should be 100");
				Assert.AreEqual(10, e.NewStartingIndex, "new starting index should be 10");
				Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should be 10");
				collectionChangingEventRaised = true;
			};

			CommonTests.observableCollectionEx.Add(100);
			Assert.IsTrue(collectionChangingEventRaised, "collection changing event should have been raised");
		}

		/// <summary>Test that canceling the preview event for adding an item to <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestAddInsertPreviewCancel()
		{
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				e.Cancel = true;
			};

			bool collectionChangedEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
			{
				collectionChangedEventRaised = true;
			};

			CommonTests.observableCollectionEx.Add(100);
			Assert.IsTrue(!collectionChangedEventRaised, "collection changed event should not have been raised");
			Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should still be 10");
		}

		#endregion Insert
		#region Remove

		/// <summary>Test that removing an item from <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestRemove()
		{
			bool collectionChangedEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action, "Action should be remove");
				Assert.IsNotNull(e.OldItems, "old items should not be null");
				Assert.AreEqual(1, e.OldItems.Count, "there should be 1 old items");
				Assert.AreEqual(1, e.OldItems[0], "OldItems[0] should be 1");
				Assert.AreEqual(1, e.OldStartingIndex, "old items starting index should be 1");
				Assert.IsNull(e.NewItems, "new items should be null");
				Assert.AreEqual(-1, e.NewStartingIndex, "new starting index should be -1");
				Assert.AreEqual(9, CommonTests.observableCollectionEx.Count, "collection count should now be 9");
				collectionChangedEventRaised = true;
			};
			
			CommonTests.observableCollectionEx.Remove(1);
			Assert.IsTrue(collectionChangedEventRaised, "collection changed event should have been raised");
			Assert.AreEqual(9, CommonTests.observableCollectionEx.Count, "collection count should be 9");
		}

		/// <summary>Test that the preview event for removing an item from <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestRemovePreview()
		{
			bool collectionChangingEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action, "Action should be remove");
				Assert.IsNotNull(e.OldItems, "old items should not be null");
				Assert.AreEqual(1, e.OldItems.Count, "there should be 1 old items");
				Assert.AreEqual(1, e.OldItems[0], "OldItems[0] should be 1");
				Assert.AreEqual(1, e.OldStartingIndex, "old items starting index should be 1");
				Assert.IsNull(e.NewItems, "new items should be null");
				Assert.AreEqual(-1, e.NewStartingIndex, "new starting index should be -1");
				Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should be 10");
				collectionChangingEventRaised = true;
			};

			CommonTests.observableCollectionEx.Remove(1);
			Assert.IsTrue(collectionChangingEventRaised, "collection changing event should have been raised");
		}

		/// <summary>Test that canceling the preview event for removing an item from <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestRemovePreviewCancel()
		{
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				e.Cancel = true;
			};

			bool collectionChangedEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
			{
				collectionChangedEventRaised = true;
			};

			CommonTests.observableCollectionEx.Remove(1);
			Assert.IsTrue(!collectionChangedEventRaised, "collection changed event should not have been raised");
			Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should still be 10");
		}

		#endregion Remove
		#region Replace

		/// <summary>Test that replacing an item in <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestReplace()
		{
			bool collectionChangedEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Replace, e.Action, "Action should be replace");
				Assert.IsNotNull(e.OldItems, "old items should not be null");
				Assert.AreEqual(1, e.OldItems.Count, "there should be 1 old items");
				Assert.AreEqual(1, e.OldItems[0], "OldItems[0] should be 1");
				Assert.AreEqual(1, e.OldStartingIndex, "old items starting index should be 1");
				Assert.IsNotNull(e.NewItems, "new items should not be null");
				Assert.AreEqual(1, e.NewItems.Count, "there should be 1 new items");
				Assert.AreEqual(100, e.NewItems[0], "NewItems[0] should be 100");
				Assert.AreEqual(1, e.NewStartingIndex, "new starting index should be 1");
				Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should still be 10");
				collectionChangedEventRaised = true;
			};

			CommonTests.observableCollectionEx[1] = 100;
			Assert.IsTrue(collectionChangedEventRaised, "collection changed event should have been raised");
			Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should still be 10");
		}

		/// <summary>Test that the preview event for replacing an item in <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestReplacePreview()
		{
			bool collectionChangingEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Replace, e.Action, "Action should be replace");
				Assert.IsNotNull(e.OldItems, "old items should not be null");
				Assert.AreEqual(1, e.OldItems.Count, "there should be 1 old items");
				Assert.AreEqual(1, e.OldItems[0], "OldItems[0] should be 1");
				Assert.AreEqual(1, e.OldStartingIndex, "old items starting index should be 1");
				Assert.IsNotNull(e.NewItems, "new items should not be null");
				Assert.AreEqual(1, e.NewItems.Count, "there should be 1 new items");
				Assert.AreEqual(100, e.NewItems[0], "NewItems[0] should be 100");
				Assert.AreEqual(1, e.NewStartingIndex, "new starting index should be 1");
				Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should be 10");
				collectionChangingEventRaised = true;
			};

			CommonTests.observableCollectionEx[1] = 100;
			Assert.IsTrue(collectionChangingEventRaised, "collection changing event should have been raised");
		}

		/// <summary>Test that canceling the preview event for replacing an item in <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestReplacePreviewCancel()
		{
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				e.Cancel = true;
			};

			bool collectionChangedEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
			{
				collectionChangedEventRaised = true;
			};

			CommonTests.observableCollectionEx[1] = 100;
			Assert.IsTrue(!collectionChangedEventRaised, "collection changed event should not have been raised");
			Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should still be 10");
		}

		#endregion Replace
		#region Move

		/// <summary>Test that moving an item in <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestMove()
		{
			bool collectionChangedEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Move, e.Action, "Action should be move");
				Assert.IsNotNull(e.OldItems, "old items should not be null");
				Assert.AreEqual(1, e.OldItems.Count, "there should be 1 old items");
				Assert.AreEqual(1, e.OldItems[0], "OldItems[0] should be 1");
				Assert.AreEqual(1, e.OldStartingIndex, "old items starting index should be 1");
				Assert.IsNotNull(e.NewItems, "new items should not be null");
				Assert.AreEqual(1, e.NewItems.Count, "there should be 1 new items");
				Assert.AreEqual(1, e.NewItems[0], "NewItems[0] should be 1");
				Assert.AreEqual(5, e.NewStartingIndex, "new starting index should be 5");
				Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should still be 10");
				collectionChangedEventRaised = true;
			};

			CommonTests.observableCollectionEx.Move(1, 5);
			Assert.IsTrue(collectionChangedEventRaised, "collection changed event should have been raised");
			Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should still be 10");
		}

		/// <summary>Test that the preview event for moving an item in <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestMovePreview()
		{
			bool collectionChangingEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Move, e.Action, "Action should be move");
				Assert.IsNotNull(e.OldItems, "old items should not be null");
				Assert.AreEqual(1, e.OldItems.Count, "there should be 1 old items");
				Assert.AreEqual(1, e.OldItems[0], "OldItems[0] should be 1");
				Assert.AreEqual(1, e.OldStartingIndex, "old items starting index should be 1");
				Assert.IsNotNull(e.NewItems, "new items should not be null");
				Assert.AreEqual(1, e.NewItems.Count, "there should be 1 new items");
				Assert.AreEqual(1, e.NewItems[0], "NewItems[0] should be 1");
				Assert.AreEqual(5, e.NewStartingIndex, "new starting index should be 5");
				Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should be 10");
				collectionChangingEventRaised = true;
			};

			CommonTests.observableCollectionEx.Move(1, 5);
			Assert.IsTrue(collectionChangingEventRaised, "collection changing event should have been raised");
		}

		/// <summary>Test that canceling the preview event for moving an item in <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestMovePreviewCancel()
		{
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				e.Cancel = true;
			};

			bool collectionChangedEventRaised = false;
			CommonTests.observableCollectionEx.CollectionChanged += delegate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
			{
				collectionChangedEventRaised = true;
			};

			CommonTests.observableCollectionEx.Move(1, 5);
			Assert.IsTrue(!collectionChangedEventRaised, "collection changed event should not have been raised");
			Assert.AreEqual(10, CommonTests.observableCollectionEx.Count, "collection count should still be 10");
		}

		#endregion Move
		#region AddRange

		/// <summary>Test that adding multiple items to <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
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
				Assert.AreEqual(13, e.NewStartingIndex, "new starting index should be 13");
				Assert.AreEqual(13, CommonTests.observableCollectionEx.Count, "collection count should now be 13");
			};

			CommonTests.observableCollectionEx.AddRange(new[] { 100, 101, 102 });
		}

		/// <summary>Test that the preview event for adding multiple items to <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestAddRangePreview()
		{
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action, "Action should be add");
				Assert.IsNull(e.OldItems, "old items should be null");
				Assert.IsNotNull(e.NewItems, "new items should not be null");
				Assert.AreEqual(3, e.NewItems.Count, "there should be 3 new items");
				Assert.AreEqual(100, e.NewItems[0], "items[0] should be 100");
				Assert.AreEqual(101, e.NewItems[1], "items[1] should be 101");
				Assert.AreEqual(102, e.NewItems[2], "items[2] should be 102");
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

		/// <summary>Test that cancling the preview event for adding multiple items to <see cref="ObservableCollectionEx{T}"/> works correctly.</summary>
		[Test]
		public static void TestAddRangePreviewCancel()
		{
			CommonTests.observableCollectionEx.CollectionChanging += delegate(object sender, NotifyCollectionChangingEventArgs e)
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action, "Action should be add");
				Assert.IsNull(e.OldItems, "old items should be null");
				Assert.IsNotNull(e.NewItems, "new items should not be null");
				Assert.AreEqual(3, e.NewItems.Count, "there should be 3 new items");
				Assert.AreEqual(100, e.NewItems[0], "items[0] should be 100");
				Assert.AreEqual(101, e.NewItems[1], "items[1] should be 101");
				Assert.AreEqual(102, e.NewItems[2], "items[2] should be 102");
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

		#endregion AddRange
	}
}
