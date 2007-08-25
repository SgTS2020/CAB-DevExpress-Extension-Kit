//===============================================================================
// Microsoft patterns & practices
// CompositeUI Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Windows.Forms;
using BankTellerCommon;
using BankTellerModule.WorkItems.BankTeller;
using BankTellerModule.WorkItems.Customer;
using DevExpress.XtraBars;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace BankTellerModule.WorkItems.BankTeller
{
	public class BankTellerWorkItem : WorkItem, IShowInShell
	{
		private BarItem queueItem;
		private IWorkspace contentWorkspace;

		// The work item uses the state persistence service that's been registered
		// in the shell initialization
		public IStatePersistenceService PersistenceService
		{
			get { return Services.Get<IStatePersistenceService>(); }
		}

		// Here we populate the work item with some of our views and start showing
		// ourselves. The BankTellerMainView has smart part placeholders named
		// UserInfo and CustomerList; these are filled in at runtime with the
		// smart parts that are registered with those names. We chose to put a
		// UserInfoView in the "UserInfo" placeholder, and a CustomerQueueView
		// in the "CustomerList" placeholder.
		//
		// Note that order is important here. When we create the BankTellerMainView,
		// it is going to assume that the smart parts that it needs already exist
		// in the work item. Therefore, we create the smart parts first, and then
		// create the main view that contains them.
		public void Show(IWorkspace sideBar, IWorkspace content)
		{
			contentWorkspace = content;

			//Needs to be named because it will be used in a placeholder
			this.Items.AddNew<UserInfoView>("UserInfo");
			SideBarView sideBarView = this.Items.AddNew<SideBarView>();
			StatsBarView statsBarView = this.Items.AddNew<StatsBarView>();

			AddMenuItems();

			sideBar.Show(sideBarView);
			sideBar.Show(statsBarView);
			this.Activate();
		}

		private void AddMenuItems()
		{
			if (queueItem == null)
			{
				queueItem = new BarSubItem();
				queueItem.Caption = "Queue";

				UIExtensionSites[UIExtensionConstants.FILE].Add(queueItem);
				UIExtensionSites.RegisterSite(UIExtensionConstants.QUEUE, queueItem);

				BarButtonItem acceptCustomer = new BarButtonItem();
				acceptCustomer.Caption = "Accept Customer";
				acceptCustomer.ItemShortcut =  new BarShortcut(Keys.Control | Keys.A);
				UIExtensionSites[UIExtensionConstants.QUEUE].Add(acceptCustomer);

				Commands[CommandConstants.ACCEPT_CUSTOMER].AddInvoker(acceptCustomer, "ItemClick");
			}
		}

		private bool ShowQueueMenu
		{
			set
			{
				//if (queueItem != null && queueItem.Visibility != value)
				if (queueItem != null && ((queueItem.Visibility == BarItemVisibility.Always ? true : false) != value))
				{
					queueItem.Visibility = value ? BarItemVisibility.Always : BarItemVisibility.Never;
				}
			}
		}

		protected override void OnActivated()
		{
			base.OnActivated();

			ShowQueueMenu = true;
		}

		// When the user clicks on a customer in their customer queue, the
		// CustomerQueueController calls us to tell us to start working with
		// the customer.
		//
		// Editing a customer is self-contained in a work item (the CustomerWorkItem)
		// so we end up with one CustomerWorkItem contained in ourselves for
		// each customer that is being edited.
		public void WorkWithCustomer(BankTellerCommon.Customer customer)
		{
			// Construct a key to register the work item in ourselves
			string key = string.Format("Customer#{0}", customer.ID);

			// Have we already made the work item for this customer?
			// If so, return the existing one.
			CustomerWorkItem workItem = this.Items.Get<CustomerWorkItem>(key);

			if (workItem == null)
			{
				workItem = WorkItems.AddNew<CustomerWorkItem>(key);
				//Set ID before setting state.  State will be cleared if a new id is set.
				workItem.ID = key;
				workItem.State[StateConstants.CUSTOMER] = customer;
				
				// Ask the persistence service if we have a saved version of
				// this work item. If so, load it from persistence.
				if (PersistenceService != null && PersistenceService.Contains(workItem.ID))
					workItem.Load();
			}

			workItem.Show(contentWorkspace);
		}

	}
}