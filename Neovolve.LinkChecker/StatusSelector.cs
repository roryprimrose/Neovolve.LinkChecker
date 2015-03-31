using System;
using System.Windows.Forms;

namespace Neovolve.LinkChecker
{
    /// <summary>
    /// The <see cref="StatusSelector"/> dialog is used to allow the user to select the set of 
    /// status values of responses found by querying <see cref="ResourceMetadata"/> objects.
    /// </summary>
    public partial class StatusSelector : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusSelector"/> class.
        /// </summary>
        public StatusSelector()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the checked codes.
        /// </summary>
        /// <returns></returns>
        public String[] GetCheckedCodes()
        {
            String[] codes = new String[StatusCodes.CheckedItems.Count];

            for (Int32 index = 0; index < codes.Length; index++)
            {
                codes[index] = StatusCodes.CheckedItems[index].ToString();
            }

            return codes;
        }

        /// <summary>
        /// Sets the codes.
        /// </summary>
        /// <param name="codes">The codes.</param>
        public void SetCodes(String[] codes)
        {
            StatusCodes.DataSource = codes;
        }
    }
}