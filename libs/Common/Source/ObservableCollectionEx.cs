using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;

namespace KSquared.FantasySportsCoach.Common
{
	/// <summary>Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed.</summary>
	/// <typeparam name="TItem">The type of elements in the collection.</typeparam>
	public class ObservableCollectionEx<TItem> : ObservableCollection<TItem>
	{
		#region Constructors

		/// <summary>Initializes a new instance of the collection.</summary>
		public ObservableCollectionEx() : base() { }

		/// <summary>Initializes a new instance of the collection that contains elements copied from the specified collection.</summary>
		/// <param name="collection">The collection from which the elements are copied.</param>
		public ObservableCollectionEx(IEnumerable<TItem> collection) : base(collection) { }

		/// <summary>Initializes a new instance of the collection that contains elements copied from the specified list.</summary>
		/// <param name="list">The list from which the elements are copied.</param>
		public ObservableCollectionEx(List<TItem> list) : base(list) { }

		#endregion Constructors
		#region Events

		#region Raised

		/// <summary>Occurs before an item is added, removed, changed, moved, or the entire list is refreshed.</summary>
		[field: NonSerialized]
		public virtual event EventHandler<NotifyCollectionChangingEventArgs> CollectionChanging;
		/// <summary>Raises the <see cref="ObservableCollectionEx{TItem}.CollectionChanging"/> event with the provided arguments.</summary>
		/// <param name="e">Arguments of the event being raised.</param>
		protected virtual void OnCollectionChanging(NotifyCollectionChangingEventArgs e)
		{
			var handler = this.CollectionChanging;
			if (handler != null)
			{
				using (this.BlockReentrancy()) { handler(this, e); }
			}
		}

		#endregion

		#endregion
		#region Methods

		/// <summary>Adds the elements of the specified collection to the end of the collection.</summary>
		/// <param name="collection">The collection whose elements should be added. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
		public void AddRange(IEnumerable<TItem> collection)
		{
			this.CheckReentrancy();

			var newItems = collection.ToArray();
			NotifyCollectionChangingEventArgs e = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Add, newItems);
			this.OnCollectionChanging(e);
			if (!e.Cancel)
			{
				foreach (var item in collection) { this.Items.Add(item); }
				this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
				this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
				base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collection.ToList(), this.Count));
			}
		}

		/// <summary>Inserts an item into the collection at the specified index.</summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert.</param>
		protected override void InsertItem(int index, TItem item)
		{
			NotifyCollectionChangingEventArgs e = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Add, item, index);
			this.OnCollectionChanging(e);
			if (!e.Cancel) { base.InsertItem(index, item); }
		}

		/// <summary>Removes all items from the collection.</summary>
		protected override void ClearItems()
		{
			NotifyCollectionChangingEventArgs e = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Reset);
			this.OnCollectionChanging(e);
			if (!e.Cancel) { base.ClearItems(); }
		}

		/// <summary>Removes the item at the specified index of the collection.</summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		protected override void RemoveItem(int index)
		{
			TItem item = base[index];
			NotifyCollectionChangingEventArgs e = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Remove, item, index);
			this.OnCollectionChanging(e);
			if (!e.Cancel) { base.RemoveItem(index); }
		}

		/// <summary>Replaces the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to replace.</param>
		/// <param name="item">The new value for the element at the specified index.</param>
		protected override void SetItem(int index, TItem item)
		{
			TItem oldItem = base[index];
			NotifyCollectionChangingEventArgs e = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Replace, item, oldItem, index);
			this.OnCollectionChanging(e);
			if (!e.Cancel) { base.SetItem(index, item); }
		}

		/// <summary>Moves the item at the specified index to a new location in the collection.</summary>
		/// <param name="oldIndex">The zero-based index specifying the location of the item to be moved.</param>
		/// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
		protected override void MoveItem(int oldIndex, int newIndex)
		{
			TItem item = base[oldIndex];
			NotifyCollectionChangingEventArgs e = new NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex);
			this.OnCollectionChanging(e);
			if (!e.Cancel) { base.MoveItem(oldIndex, newIndex); }
		}

		#endregion Methods
	}

	/// <summary>Cancelable event arguments for collections.</summary>
	/// <remarks>This should probably inherit <see cref="CancelEventArgs"/>, but there's too much functionality in the existing base class to duplicate.</remarks>
	public class NotifyCollectionChangingEventArgs : NotifyCollectionChangedEventArgs
	{
		#region Constructors

		/// <summary>Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs class that describes a System.Collections.Specialized.NotifyCollectionChangedAction.Reset change.</summary>
		/// <param name="action">The action that caused the event. This must be set to System.Collections.Specialized.NotifyCollectionChangedAction.Reset.</param>
		/// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
		public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, bool cancel = false) : base(action) { }

		/// <summary>Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs class that describes a multi-item change.</summary>
		/// <param name="action">The action that caused the event. This can be set to System.Collections.Specialized.NotifyCollectionChangedAction.Reset, System.Collections.Specialized.NotifyCollectionChangedAction.Add, or System.Collections.Specialized.NotifyCollectionChangedAction.Remove.</param>
		/// <param name="changedItems">The items that are affected by the change.</param>
		/// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
		public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, IList changedItems, bool cancel = false) : base(action, changedItems) { }

		/// <summary>Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs class that describes a one-item change.</summary>
		/// <param name="action">The action that caused the event. This can be set to System.Collections.Specialized.NotifyCollectionChangedAction.Reset, System.Collections.Specialized.NotifyCollectionChangedAction.Add, or System.Collections.Specialized.NotifyCollectionChangedAction.Remove.</param>
		/// <param name="changedItem">The item that is affected by the change.</param>
		/// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
		public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, object changedItem, bool cancel = false) : base(action, changedItem) { }

		/// <summary>Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs class that describes a multi-item System.Collections.Specialized.NotifyCollectionChangedAction.Replace change.</summary>
		/// <param name="action">The action that caused the event. This can only be set to System.Collections.Specialized.NotifyCollectionChangedAction.Replace.</param>
		/// <param name="newItems">The new items that are replacing the original items.</param>
		/// <param name="oldItems">The original items that are replaced.</param>
		/// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
		public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, bool cancel = false) : base(action, newItems, oldItems) { }

		/// <summary>Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs class that describes a multi-item change or a System.Collections.Specialized.NotifyCollectionChangedAction.Reset change.</summary>
		/// <param name="action">The action that caused the event. This can be set to System.Collections.Specialized.NotifyCollectionChangedAction.Reset, System.Collections.Specialized.NotifyCollectionChangedAction.Add, or System.Collections.Specialized.NotifyCollectionChangedAction.Remove.</param>
		/// <param name="changedItems">The items affected by the change.</param>
		/// <param name="startingIndex">The index where the change occurred.</param>
		/// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
		public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex, bool cancel = false) : base(action, changedItems, startingIndex) { }

		/// <summary>Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs class that describes a one-item change.</summary>
		/// <param name="action">The action that caused the event. This can be set to System.Collections.Specialized.NotifyCollectionChangedAction.Reset, System.Collections.Specialized.NotifyCollectionChangedAction.Add, or System.Collections.Specialized.NotifyCollectionChangedAction.Remove.</param>
		/// <param name="changedItem">The item that is affected by the change.</param>
		/// <param name="index">The index where the change occurred.</param>
		/// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
		public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, bool cancel = false) : base(action, changedItem, index) { }

		/// <summary>Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs class that describes a one-item System.Collections.Specialized.NotifyCollectionChangedAction.Replace change.</summary>
		/// <param name="action">The action that caused the event. This can only be set to System.Collections.Specialized.NotifyCollectionChangedAction.Replace.</param>
		/// <param name="newItem">The new item that is replacing the original item.</param>
		/// <param name="oldItem">The original item that is replaced.</param>
		/// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
		public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, bool cancel = false) : base(action, newItem, oldItem) { }

		/// <summary>Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs class that describes a multi-item System.Collections.Specialized.NotifyCollectionChangedAction.Replace change.</summary>
		/// <param name="action">The action that caused the event. This can only be set to System.Collections.Specialized.NotifyCollectionChangedAction.Replace.</param>
		/// <param name="newItems">The new items that are replacing the original items.</param>
		/// <param name="oldItems">The original items that are replaced.</param>
		/// <param name="startingIndex">The index of the first item of the items that are being replaced.</param>
		/// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
		public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, bool cancel = false) : base(action, newItems, oldItems, startingIndex) { }

		/// <summary>Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs class that describes a multi-item System.Collections.Specialized.NotifyCollectionChangedAction.Move change.</summary>
		/// <param name="action">The action that caused the event. This can only be set to System.Collections.Specialized.NotifyCollectionChangedAction.Move.</param>
		/// <param name="changedItems">The items affected by the change.</param>
		/// <param name="index">The new index for the changed items.</param>
		/// <param name="oldIndex">The old index for the changed items.</param>
		/// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
		public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex, bool cancel = false) : base(action, changedItems, index, oldIndex) { }

		/// <summary>Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs class that describes a multi-item System.Collections.Specialized.NotifyCollectionChangedAction.Move change.</summary>
		/// <param name="action">The action that caused the event. This can only be set to System.Collections.Specialized.NotifyCollectionChangedAction.Move.</param>
		/// <param name="changedItem">The item affected by the change.</param>
		/// <param name="index">The new index for the changed item.</param>
		/// <param name="oldIndex">The old index for the changed item.</param>
		/// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
		public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex, bool cancel = false) : base(action, changedItem, index, oldIndex) { }

		/// <summary>Initializes a new instance of the System.Collections.Specialized.NotifyCollectionChangedEventArgs class that describes a one-item System.Collections.Specialized.NotifyCollectionChangedAction.Replace change.</summary>
		/// <param name="action">The action that caused the event. This can be set to System.Collections.Specialized.NotifyCollectionChangedAction.Replace.</param>
		/// <param name="newItem">The new item that is replacing the original item.</param>
		/// <param name="oldItem">The original item that is replaced.</param>
		/// <param name="index">The index of the item being replaced.</param>
		/// <param name="cancel"><c>true</c> to cancel the event; otherwise, <c>false</c>.</param>
		public NotifyCollectionChangingEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index, bool cancel = false) : base(action, newItem, oldItem, index) { }

		#endregion
		#region Properties

		/// <summary>Gets or sets a value indicating whether the event should be canceled.</summary>
		public bool Cancel { get; set; }

		#endregion Properties
	}
}
