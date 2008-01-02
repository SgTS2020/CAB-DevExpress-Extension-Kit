using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CABDevExpress.Workspaces;
using Microsoft.Practices.CompositeUI.WinForms;

namespace CABDevExpress.SmartPartInfos
{
    /// <summary>
    /// Provides information to show smartparts in the <see cref="XtraWindowWorkspace"/>.
    /// </summary>
    [ToolboxBitmap(typeof(XtraWindowSmartPartInfo), "XtraWindowSmartPartInfo")]
    public class XtraWindowSmartPartInfo : WindowSmartPartInfo
    {
        private bool showInTaskbar = false;
        private FormStartPosition startPosition = default(FormStartPosition);

        ///
        /// The start position of the Modal form
        ///
        [DefaultValue(false)]
        [Category("Layout")]
        public FormStartPosition StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        ///
        /// Whether the form shows in the Windows taskbar
        ///
        [DefaultValue(false)]
        [Category("Layout")]
        public bool ShowInTaskbar
        {
            get { return showInTaskbar; }
            set { showInTaskbar = value; }
        }
    }
}
