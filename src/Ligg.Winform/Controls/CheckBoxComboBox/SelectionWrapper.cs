using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.ComponentModel;

namespace Ligg.Winform.Controls.CheckBoxComboBox
{
    public class ListSelection<T> : List<ObjectSelection<T>>
    {
        #region CONSTRUCTOR
        public ListSelection(IEnumerable source) : this(source, false) { }
        public ListSelection(IEnumerable source, bool showCounts)
            : base()
        {
            _Source = source;
            _showCounts = showCounts;
            if (_Source is IBindingList)
                ((IBindingList)_Source).ListChanged += new ListChangedEventHandler(ListSelectionWrapper_ListChanged);
            Populate();
        }
        public ListSelection(IEnumerable source, string usePropertyAsDisplayName) : this(source, false, usePropertyAsDisplayName) { }
        /// <summary>
        /// A Display "Name" property is specified. ToString() will not be performed on items.
        /// This is specifically useful on DataTable implementations, or where PropertyDescriptors are used to read the values.
        /// If a PropertyDescriptor is not found, a Property will be used.
        /// </summary>
        public ListSelection(IEnumerable source, bool showCounts, string usePropertyAsDisplayName)
            : this(source, showCounts)
        {
            _displayNameProperty = usePropertyAsDisplayName;
        }
        #endregion

        #region PRIVATE PROPERTIES
        private bool _showCounts;
        private readonly IEnumerable _Source;
        /// <summary>
        /// Used to indicate NOT to use ToString(), but read this property instead as a display value.
        /// </summary>
        private string _displayNameProperty = null;
        #endregion

        #region PUBLIC PROPERTIES
        public string DisplayNameProperty
        {
            get { return _displayNameProperty; }
            set { _displayNameProperty = value; }
        }

        public string SelectedNames
        {
            get
            {
                string Text = "";
                foreach (ObjectSelection<T> Item in this)
                    if (Item.Selected)
                        Text += (
                            string.IsNullOrEmpty(Text)
                            //? String.Format("\"{0}\"", Item.Name)
                            ? String.Format("{0}", Item.Name)
                            //: String.Format(" & \"{0}\"", Item.Name));
                            : String.Format(", {0}", Item.Name));
                return Text;
            }
        }

        public bool ShowCounts
        {
            get { return _showCounts; }
            set { _showCounts = value; }
        }
        #endregion

        #region HELPER MEMBERS
        /// <summary>
        /// Reset all counts to zero.
        /// </summary>
        public void ClearCounts()
        {
            foreach (ObjectSelection<T> Item in this)
                Item.Count = 0;
        }
  
        private ObjectSelection<T> CreateSelection(IEnumerator Object)
        {
            Type[] Types = new Type[] { typeof(T), this.GetType() };
            ConstructorInfo CI = typeof(ObjectSelection<T>).GetConstructor(Types);
            if (CI == null)
                throw new Exception(String.Format(
                              "The selection wrapper class {0} must have a constructor with ({1} Item, {2} Container) parameters.",
                              typeof(ObjectSelection<T>),
                              typeof(T),
                              this.GetType()));
            object[] parameters = new object[] { Object.Current, this };
            object result = CI.Invoke(parameters);
            return (ObjectSelection<T>)result;
        }

        public ObjectSelection<T> FindObjectWithItem(T Object)
        {
            return Find(new Predicate<ObjectSelection<T>>(
                            delegate(ObjectSelection<T> target)
                            {
                                return target.Item.Equals(Object);
                            }));
        }

       
        //public TSelectionWrapper FindObjectWithKey(object key)
        //{
        //    return FindObjectWithKey(new object[] { key });
        //}

        //public TSelectionWrapper FindObjectWithKey(object[] keys)
        //{
        //    return Find(new Predicate<TSelectionWrapper>(
        //                    delegate(TSelectionWrapper target)
        //                    {
        //                        return
        //                            ReflectionHelper.CompareKeyValues(
        //                                ReflectionHelper.GetKeyValuesFromObject(target.Item, target.Item.TableInfo),
        //                                keys);
        //                    }));
        //}

        //public object[] GetArrayOfSelectedKeys()
        //{
        //    List<object> List = new List<object>();
        //    foreach (TSelectionWrapper Item in this)
        //        if (Item.Selected)
        //        {
        //            if (Item.Item.TableInfo.KeyProperties.Length == 1)
        //                List.Add(ReflectionHelper.GetKeyValueFromObject(Item.Item, Item.Item.TableInfo));
        //            else
        //                List.Add(ReflectionHelper.GetKeyValuesFromObject(Item.Item, Item.Item.TableInfo));
        //        }
        //    return List.ToArray();
        //}

        //public T[] GetArrayOfSelectedKeys<T>()
        //{
        //    List<T> List = new List<T>();
        //    foreach (TSelectionWrapper Item in this)
        //        if (Item.Selected)
        //        {
        //            if (Item.Item.TableInfo.KeyProperties.Length == 1)
        //                List.Add((T)ReflectionHelper.GetKeyValueFromObject(Item.Item, Item.Item.TableInfo));
        //            else
        //                throw new LibraryException("This generator only supports single value keys.");
        //            // List.Add((T)ReflectionHelper.GetKeyValuesFromObject(Item.Item, Item.Item.TableInfo));
        //        }
        //    return List.ToArray();
        //}
       
        private void Populate()
        {
            Clear();
            /*
            for(int Index = 0; Index <= _Source.Count -1; Index++)
                Add(CreateSelection(_Source[Index]));
             */
            IEnumerator Enumerator = _Source.GetEnumerator();
            if (Enumerator != null)
                while (Enumerator.MoveNext())
                    Add(CreateSelection(Enumerator));
        }
        #endregion

        #region EVENT HANDLERS
        private void ListSelectionWrapper_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    Add(CreateSelection((IEnumerator)((IBindingList)_Source)[e.NewIndex]));
                    break;
                case ListChangedType.ItemDeleted:
                    Remove(FindObjectWithItem((T)((IBindingList)_Source)[e.OldIndex]));
                    break;
                case ListChangedType.Reset:
                    Populate();
                    break;
            }
        }

        #endregion
    }


    public class ObjectSelection<T>
    {
        public ObjectSelection(T item, ListSelection<T> container): base()
        {
            _Container = container;
            _Item = item;
        }

        #region PRIVATE PROPERTIES
        /// <summary>
        /// Used as a count indicator for the item. Not necessarily displayed.
        /// </summary>
        public int _Count = 0;
        /// <summary>
        /// Is this item selected.
        /// </summary>
        public bool _Selected = false;
        /// <summary>
        /// A reference to the wrapped item.
        /// </summary>
        public T _Item;
        /// <summary>
        /// The containing list for these selections.
        /// </summary>
        private ListSelection<T> _Container;
        #endregion

        #region PUBLIC PROPERTIES
        /// <summary>
        /// An indicator of how many items with the specified status is available for the current filter level.
        /// Thaught this would make the app a bit more user-friendly and help not to miss items in Statusses
        /// that are not often used.
        /// </summary>
        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }
        /// <summary>
        /// A reference to the item wrapped.
        /// </summary>
        public T Item
        {
            get { return _Item; }
            set { _Item = value; }
        }
        /// <summary>
        /// The item display value. If ShowCount is true, it displays the "Name [Count]".
        /// </summary>
        public string Name
        {
            get
            {
                string Name = null;
                if (string.IsNullOrEmpty(_Container.DisplayNameProperty))
                    Name = Item.ToString();
                else if (Item is DataRow) // A specific implementation for DataRow
                    Name = ((DataRow)((Object)Item))[_Container.DisplayNameProperty].ToString();
                else
                {
                    PropertyDescriptorCollection PDs = TypeDescriptor.GetProperties(Item);
                    foreach (PropertyDescriptor PD in PDs)
                        if (PD.Name.CompareTo(_Container.DisplayNameProperty) == 0)
                        {
                            Name = (string)PD.GetValue(Item).ToString();
                            break;
                        }
                    if (string.IsNullOrEmpty(Name))
                    {
                        PropertyInfo PI = Item.GetType().GetProperty(_Container.DisplayNameProperty);
                        if (PI == null)
                            throw new Exception(String.Format(
                                      "Property {0} cannot be found on {1}.",
                                      _Container.DisplayNameProperty,
                                      Item.GetType()));
                        Name = PI.GetValue(Item, null).ToString();
                    }
                }
                return _Container.ShowCounts ? String.Format("{0} [{1}]", Name, Count) : Name;
            }
        }
        /// <summary>
        /// The textbox display value. The names concatenated.
        /// </summary>
        public string NameConcatenated
        {
            get { return _Container.SelectedNames; }
        }
        /// <summary>
        /// Indicates whether the item is selected.
        /// </summary>
        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; }
        }
        #endregion
    }
}
