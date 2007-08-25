using DevExpress.XtraEditors;
using Microsoft.Practices.CompositeUI.SmartParts;

namespace BankTellerModule.WorkItems.BankTeller
{
	[SmartPart]
	public partial class SideBarView : XtraUserControl, ISmartPartInfoProvider
	{
		public SideBarView()
		{
			InitializeComponent();
		}

		public ISmartPartInfo GetSmartPartInfo(System.Type smartPartInfoType)
		{
			return infoProvider.GetSmartPartInfo(smartPartInfoType);
		}
	}
}