using System;
using DevExpress.XtraBars.Ribbon;
using Microsoft.Practices.CompositeUI.UIElements;
using Microsoft.Practices.CompositeUI.Utility;

namespace CABDevExpress.UIElements
{
    /// <summary>
    /// An adapter that wraps a <see cref="RibbonPageGroup"/> for use as an <see cref="IUIElementAdapter"/>.
    /// Class it is used to Manage groups added and removed from a ribbon Page
    /// </summary>
    public class RibbonPageGroupCollectionUIAdapter : UIElementAdapter<RibbonPageGroup>
    {
        private RibbonPageGroupCollection collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonPageGroupCollectionUIAdapter"/> class.
        /// </summary>
        /// <param name="collection"></param>
        public RibbonPageGroupCollectionUIAdapter(RibbonPageGroupCollection collection)
        {
            Guard.ArgumentNotNull(collection, "RibbonPageGroupCollection");
            this.collection = collection;
        }

        /// <summary>
        /// See <see cref="UIElementAdapter{TUIElement}.Add(TUIElement)"/> for more information.
        /// </summary>
        protected override RibbonPageGroup Add(RibbonPageGroup uiElement)
        {
            if (collection == null)
                throw new InvalidOperationException();

            collection.Insert(GetInsertingIndex(uiElement), uiElement);
            return uiElement;
        }

        /// <summary>
        /// See <see cref="UIElementAdapter{TUIElement}.Remove(TUIElement)"/> for more information.
        /// </summary>
        protected override void Remove(RibbonPageGroup uiElement)
        {
            if (uiElement.Page != null)
                uiElement.Page.Groups.Remove(uiElement);
        }

        /// <summary>
        /// When overridden in a derived class, returns the correct index for the item being added. By default,
        /// it will return the length of the collection.
        /// </summary>
        /// <param name="uiElement"></param>
        /// <returns></returns>
        protected virtual int GetInsertingIndex(object uiElement)
        {
            return collection.Count;
        }

        /// <summary>
        /// Returns the internal collection mananged by the <see cref="NavBarGroupCollectionUIAdapter"/>
        /// </summary>
        protected RibbonPageGroupCollection InternalCollection
        {
            get { return collection; }
            set { collection = value; }
        }
    }
}