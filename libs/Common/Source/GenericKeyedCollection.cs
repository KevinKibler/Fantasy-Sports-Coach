using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;

namespace KSquared.FantasySportsCoach.Common
{
	/// <summary>A generic implementation of <see cref="KeyedCollection{TKey, TItem}"/> that allows the key extraction function to specified externally.</summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TItem">The type of the item.</typeparam>
	public class GenericKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>
	{
		#region Fields

		private Converter<TItem, TKey> getKeyForItem = null;

		#endregion Fields
		#region Constructors

		/// <summary>Creates a new <see cref="GenericKeyedCollection{TKey, TItem}" />.</summary>
		/// <param name="genericKeyedCollection">Another <see cref="GenericKeyedCollection{TKey, TItem}"/> to copy data from.</param>
		public GenericKeyedCollection(GenericKeyedCollection<TKey, TItem> genericKeyedCollection)
			: this(genericKeyedCollection.getKeyForItem, genericKeyedCollection) { }

		/// <summary>Creates a new <see cref="GenericKeyedCollection{TKey, TItem}" />.</summary>
		/// <param name="getKeyForItem">A delegate that can be used to get the key for an item.</param>
		public GenericKeyedCollection(Converter<TItem, TKey> getKeyForItem)
			: this(getKeyForItem, new TItem[] { }) { }

		/// <summary>Creates a new <see cref="GenericKeyedCollection{TKey, TItem}" />.</summary>
		/// <param name="getKeyForItem">A delegate that can be used to get the key for an item.</param>
		/// <param name="items">Items to add to the collection.</param>
		public GenericKeyedCollection(Converter<TItem, TKey> getKeyForItem, IEnumerable<TItem> items)
		{
			if (getKeyForItem == null) { throw new ArgumentNullException("getKeyForItem"); }
			if (items == null) { throw new ArgumentNullException("items"); }

			this.getKeyForItem = getKeyForItem;
			this.AddRange(items);
		}

		#endregion Constructors
		#region Events

		#region Raised

		/// <summary>Occurs after an item is added, removed, changed, moved, or the entire list is refreshed.</summary>
		[field: NonSerialized]
		public virtual event EventHandler<NotifyCollectionChangedEventArgs> CollectionChanged;
		/// <summary>Raises the <see cref="GenericKeyedCollection{TKey, TItem}.CollectionChanged"/> event with the provided arguments.</summary>
		/// <param name="e">Arguments of the event being raised.</param>
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			var handler = this.CollectionChanged;
			if (handler != null) { handler(this, e); }
		}

		/// <summary>Occurs before an item is added, removed, changed, moved, or the entire list is refreshed.</summary>
		[field: NonSerialized]
		public virtual event EventHandler<NotifyCollectionChangingEventArgs> CollectionChanging;
		/// <summary>Raises the <see cref="GenericKeyedCollection{TKey, TItem}.CollectionChanging"/> event with the provided arguments.</summary>
		/// <param name="e">Arguments of the event being raised.</param>
		protected virtual void OnCollectionChanging(NotifyCollectionChangingEventArgs e)
		{
			var handler = this.CollectionChanging;
			if (handler != null) { handler(this, e); }
		}

		#endregion Raised

		#endregion Events
		#region Methods

		/// <summary>Inserts an item into the collection and raises the ItemInserted event.</summary>
		/// <param name="index">The index at which to add the item.</param>
		/// <param name="item">The item to add</param>
		protected override void InsertItem(int index, TItem item)
		{
			NotifyCollectionChangingEventArgs e = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Add, item, index);
			this.OnCollectionChanging(e);
			if (!e.Cancel)
			{
				base.InsertItem(index, item);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
			}
		}

		/// <summary>Removes an item from the collection and raises the ItemRemoved event.</summary>
		/// <param name="index">The index of the item to remove.</param>
		protected override void RemoveItem(int index)
		{
			TItem item = base[index];
			NotifyCollectionChangingEventArgs e = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Remove, item, index);
			this.OnCollectionChanging(e);
			if (!e.Cancel)
			{
				base.RemoveItem(index);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
			}
		}

		/// <summary>Sets the item at the specified index and raises the ItemChanged event.</summary>
		/// <param name="index">The index of the item to set.</param>
		/// <param name="item">The item to set.</param>
		protected override void SetItem(int index, TItem item)
		{
			TItem oldItem = base[index];
			NotifyCollectionChangingEventArgs e = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index);
			this.OnCollectionChanging(e);
			if (!e.Cancel)
			{
				base.SetItem(index, item);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, oldItem, item, index));
			}
		}

		/// <summary>Removes all items from the collection, raising the ItemRemoved event for each one.</summary>
		protected override void ClearItems()
		{
			NotifyCollectionChangingEventArgs e = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Reset);
			this.OnCollectionChanging(e);
			if (!e.Cancel)
			{
				base.ClearItems();
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}
	
		/// <summary>Extracts the key from the specified element.</summary>
		/// <param name="item">The element from which to extract the key.</param>
		/// <returns>The key for the specified element.</returns>
		protected override TKey GetKeyForItem(TItem item)
		{
			return this.getKeyForItem(item);
		}

		/// <summary>Sorts the items in the collection using the given comparison.</summary>
		public void Sort(Comparison<TItem> comparison)
		{
			if (comparison == null) { throw new ArgumentNullException("comparison"); }

			if (this.Count > 0)
			{
				TItem[] itemArray = this.ToArray();
				Array.Sort<TItem>(itemArray, comparison);

				// Clear current items then add the array - do so that no events are fired
				do
				{
					base.RemoveItem(0);
				} while (this.Count > 0);
				// add items - no events
				for (int i = 0; i < itemArray.Length; i++)
				{
					base.InsertItem(i, itemArray[i]);
				}
			}
		}

		/// <summary>Returns the list as an array of elements.</summary>
		public TItem[] ToArray()
		{
			TItem[] items = new TItem[this.Count];
			for (int i = 0; i < this.Count; i++)
			{
				items[i] = this.Items[i];
			}
			return items;
		}

		/// <summary>Extracts the key from the specified element.</summary>
		/// <param name="item">The element from which to extract the key.</param>
		/// <returns>The key for the specified element.</returns>
		/// <remarks>This method exists because GetKeyForItem is protected and accessibility cannot be overridden.</remarks>
		public TKey GetItemKey(TItem item)
		{
			return this.GetKeyForItem(item);
		}

		/// <summary>Adds the elements of the specified collection to the end of the <see cref="GenericKeyedCollection{TKey, TItem}"/>.</summary>
		/// <param name="collection">The items to be added to the end of the collection. It cannot be null, but it can contain elements that are null, if type TItem is a reference type.</param>
		public void AddRange(IEnumerable<TItem> collection)
		{
			if (collection == null) { throw new ArgumentNullException("items"); }

			var newItems = collection.ToArray();
			NotifyCollectionChangingEventArgs e = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Add, newItems);
			this.OnCollectionChanging(e);
			if (!e.Cancel)
			{
				foreach (var item in collection) { this.Items.Add(item); }
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection.ToList(), this.Count));
			}
		}

		/// <summary>Determines whether an element exists in the collection.</summary>
		/// <param name="item">The item to search for.</param>
		/// <returns>True if the item is found, false otherwise.</returns>
		/// <remarks>This method exists because Contains(TItem) is protected and accessibility cannot be overridden.</remarks>
		public bool ContainsItem(TItem item)
		{
			return this.Contains(item);
		}

		/// <summary>Determines whether an element with the specified key exists in the collection.</summary>
		/// <param name="key">The key to search for.</param>
		/// <returns>True if an item is found, false otherwise.</returns>
		/// <remarks>This method exists for consistency with ContainsItem(TItem).</remarks>
		public bool ContainsKey(TKey key)
		{
			return this.Contains(key);
		}

		#endregion Methods
	}
}