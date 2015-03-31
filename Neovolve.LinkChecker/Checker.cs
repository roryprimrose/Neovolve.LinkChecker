using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Neovolve.LinkChecker
{
    /// <summary>
    /// The <see cref="Checker"/> form is used to process resources and their metadata.
    /// </summary>
    public partial class Checker : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Checker"/> class.
        /// </summary>
        public Checker()
        {
            InitializeComponent();

            Processor = new ResourceProcessor();

            Processor.ResourceFound += Processor_ResourceFound;
            Processor.ResourcesFound += Processor_ResourcesFound;
            Processor.ResourceUpdated += Processor_ResourceUpdated;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.FormClosingEventArgs"/> that contains the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Processor.ResourceFound -= Processor_ResourceFound;
            Processor.ResourcesFound -= Processor_ResourcesFound;
            Processor.ResourceUpdated -= Processor_ResourceUpdated;

            base.OnFormClosing(e);
        }

        /// <summary>
        /// Handles the Click event of the CheckLinks control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CheckLinks_Click(Object sender, EventArgs e)
        {
            LinksFound.Items.Clear();

            Processor.Start(BaseLocation.Text, FilterRegex.Text);
        }

        /// <summary>
        /// Handles the Click event of the Export control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Export_Click(Object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV Files (*.csv)|*.csv";

                if (dialog.ShowDialog()
                    == DialogResult.OK)
                {
                    ExportDetails exportDetails = new ExportDetails();

                    exportDetails.BaseLocation = Processor.BaseLocation.ToString();
                    exportDetails.Items = Processor.ExportResources();
                    exportDetails.Path = dialog.FileName;

                    Thread exportThread = new Thread(ExportMetadataItems);

                    exportThread.IsBackground = true;
                    exportThread.Name = "Export records";
                    exportThread.Start(exportDetails);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the Import control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Import_Click(Object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "CSV Files (*.csv)|*.csv";

                if (dialog.ShowDialog()
                    == DialogResult.OK)
                {
                    Thread importThread = new Thread(ImportMetadataItems);

                    importThread.IsBackground = true;
                    importThread.Name = "Import records";
                    importThread.Start(dialog.FileName);
                }
            }
        }

        /// <summary>
        /// Imports the metadata items.
        /// </summary>
        /// <param name="path">The path.</param>
        private void ImportMetadataItems(Object path)
        {
            String filePath = path as String;

            // Exit if there isn't a path to use
            if (String.IsNullOrEmpty(filePath))
            {
                return;
            }

            LinksFound.Items.Clear();

            List<ResourceMetadata> newItems = new List<ResourceMetadata>();

            using (TextReader reader = new StreamReader(filePath))
            {
                String contents = reader.ReadLine();

                while (contents != null)
                {
                    if (contents.Contains("Relative Uri") || contents.Contains("Response")
                        || contents.Contains("MIME Type") || contents.Contains("Absolute Uri")
                        || contents.Contains("Referrer"))
                    {
                        contents = reader.ReadLine();

                        continue;
                    }

                    String[] parts = GetFields(contents);

                    try
                    {
                        Uri relativeUri = new Uri(parts[0], UriKind.Relative);
                        Uri referrerUri = null;

                        if (String.IsNullOrEmpty(parts[3]) == false)
                        {
                            referrerUri = new Uri(parts[3], UriKind.Relative);
                        }

                        ResourceMetadata metadata = new ResourceMetadata(relativeUri, referrerUri);

                        metadata.Status = ResourceMetadataStatus.ResourcesChecked;
                        metadata.ResponseCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), parts[1]);
                        metadata.MimeType = parts[2];

                        newItems.Add(metadata);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }

                    contents = reader.ReadLine();
                }
            }

            Processor.ImportResources(newItems);
        }

        /// <summary>
        /// Inserts the list view item.
        /// </summary>
        /// <param name="newItem">The new item.</param>
        private void InsertListViewItem(ListViewItem newItem)
        {
            if (newItem == null)
            {
                return;
            }

            if (LinksFound.InvokeRequired)
            {
                LinksFound.Invoke(new InsertListViewItemDelegate(InsertListViewItem), new Object[] { newItem });

                return;
            }

            if (GetListItemIndex(newItem.Name) >= 0)
            {
                return;
            }

            Debug.WriteLine("Adding " + newItem.Name);

            LinksFound.Items.Add(newItem);
        }

        /// <summary>
        /// Lists the contains item.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private Int32 GetListItemIndex(String name)
        {
            for (int index = 0; index < LinksFound.Items.Count; index++)
            {
                ListViewItem item = LinksFound.Items[index];

                if (item.Name == name)
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Handles the ItemActivate event of the LinksFound control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LinksFound_ItemActivate(Object sender, EventArgs e)
        {
            Uri location = new Uri(BaseLocation.Text + LinksFound.SelectedItems[0].Text);

            Process.Start(location.ToString());
        }

        /// <summary>
        /// Populates the imported items.
        /// </summary>
        /// <param name="newItems">The new items.</param>
        private void PopulateImportedItems(ListViewItem[] newItems)
        {
            if (LinksFound.InvokeRequired)
            {
                LinksFound.Invoke(new ImportMetadataItemsDelegate(PopulateImportedItems), new Object[] { newItems });

                return;
            }

            LinksFound.BeginUpdate();
            LinksFound.Items.AddRange(newItems);
            LinksFound.EndUpdate();
        }

        /// <summary>
        /// Handles the ResourceFound event of the Processor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Neovolve.LinkChecker.ResourceEventArgs"/> instance containing the event data.</param>
        private void Processor_ResourceFound(Object sender, ResourceEventArgs e)
        {
            ResourceMetadata newResource = e.Resource;

            // Checks whether the newResource parameter has been supplied
            if (newResource == null)
            {
                const String NewResourceParameterName = "e.Resource";

                throw new ArgumentNullException(NewResourceParameterName);
            }

            ListViewItem newItem = CreateNewListViewItem(newResource);

            if (newItem == null)
            {
                return;
            }

            InsertListViewItem(newItem);
        }

        /// <summary>
        /// Handles the ResourcesFound event of the Processor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Neovolve.LinkChecker.ResourcesEventArgs"/> instance containing the event data.</param>
        private void Processor_ResourcesFound(Object sender, ResourcesEventArgs e)
        {
            ListViewItem[] newItems = new ListViewItem[e.Resources.Count];

            for (Int32 index = 0; index < e.Resources.Count; index++)
            {
                ListViewItem newItem = CreateNewListViewItem(e.Resources[index]);

                newItems[index] = newItem;
            }

            PopulateImportedItems(newItems);
        }

        /// <summary>
        /// Handles the ResourceUpdated event of the Processor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Neovolve.LinkChecker.ResourceEventArgs"/> instance containing the event data.</param>
        private void Processor_ResourceUpdated(Object sender, ResourceEventArgs e)
        {
            UpdateLinkProgress(e.Resource);
        }

        /// <summary>
        /// Handles the Click event of the ReplayChecks control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ReplayChecks_Click(Object sender, EventArgs e)
        {
            Processor.BaseLocation = new Uri(BaseLocation.Text);

            if (String.IsNullOrEmpty(FilterRegex.Text) == false)
            {
                Processor.FilterExpression = new Regex(FilterRegex.Text);
            }
            else
            {
                Processor.FilterExpression = null;
            }

            List<String> codes = new List<String>();

            // Loop through the links found an get the unique response codes
            for (Int32 index = 0; index < LinksFound.Items.Count; index++)
            {
                ListViewItem item = LinksFound.Items[index];

                if (codes.Contains(item.SubItems[2].Text) == false)
                {
                    codes.Add(item.SubItems[2].Text);
                }
            }

            StatusSelector selector = new StatusSelector();

            selector.SetCodes(codes.ToArray());

            if (selector.ShowDialog(this)
                == DialogResult.OK)
            {
                List<String> selectedCodes = new List<String>(selector.GetCheckedCodes());

                if (selectedCodes.Count > 0)
                {
                    Processor.ReplayByStatusCodes(selectedCodes);
                }
            }
        }

        /// <summary>
        /// Updates the link progress.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        private void UpdateLinkProgress(ResourceMetadata metadata)
        {
            if (LinksFound.InvokeRequired)
            {
                LinksFound.Invoke(new UpdateLinkProgressDelegate(UpdateLinkProgress), new Object[] { metadata });

                return;
            }

            Debug.WriteLine("Updating status for " + metadata.Key);

            Int32 index = GetListItemIndex(metadata.Key);
            ListViewItem currentItem;

            if (index > -1)
            {
                currentItem = LinksFound.Items[index];
            }
            else
            {
                Debug.WriteLine("Item " + metadata.Key + " isn't in the ListView control");

                currentItem = CreateNewListViewItem(metadata);

                InsertListViewItem(currentItem);
            }

            currentItem.SubItems[1].Text = metadata.Status.ToString();
            currentItem.SubItems[2].Text = metadata.ResponseCode.ToString();
            currentItem.SubItems[3].Text = metadata.MimeType;
            
            currentItem.EnsureVisible();
        }

        /// <summary>
        /// Creates the new list view item.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns></returns>
        private static ListViewItem CreateNewListViewItem(ResourceMetadata metadata)
        {
            String location = String.Empty;

            if (metadata.RelativeLocation != null
                && String.IsNullOrEmpty(metadata.RelativeLocation.ToString()) == false)
            {
                location = metadata.RelativeLocation.ToString();
            }

            // Add the item to the listview
            ListViewItem newItem = new ListViewItem(location);

            newItem.Name = metadata.Key;
            newItem.SubItems.Add(metadata.Status.ToString());
            newItem.SubItems.Add(metadata.ResponseCode.ToString());
            newItem.SubItems.Add(metadata.MimeType);

            if (metadata.Referrer != null)
            {
                newItem.SubItems.Add(metadata.Referrer.ToString());
            }
            else
            {
                newItem.SubItems.Add(String.Empty);
            }

            return newItem;
        }

        /// <summary>
        /// Exports the metadata items.
        /// </summary>
        /// <param name="exportInformation">The export information.</param>
        private static void ExportMetadataItems(Object exportInformation)
        {
            ExportDetails exportDetails = (ExportDetails)exportInformation;

            using (TextWriter writer = new StreamWriter(exportDetails.Path))
            {
                writer.WriteLine("Relative Uri,Response,MIME Type,Referrer,Absolute Uri");

                for (Int32 index = 0; index < exportDetails.Items.Count; index++)
                {
                    ResourceMetadata item = exportDetails.Items[index];

                    writer.WriteLine(
                        "{0},{1},{2},{3},{4}",
                        item.RelativeLocation,
                        item.ResponseCode,
                        item.MimeType,
                        item.Referrer,
                        exportDetails.BaseLocation + item.RelativeLocation);
                }

                writer.Close();
            }
        }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <param name="contents">The contents.</param>
        /// <returns></returns>
        private static String[] GetFields(String contents)
        {
            String[] fields = contents.Split(',');

            for (Int32 index = 0; index < fields.Length; index++)
            {
                if (fields[index].StartsWith("\"", StringComparison.OrdinalIgnoreCase))
                {
                    fields[index] = fields[index].Substring(1);
                }

                if (fields[index].EndsWith("\"", StringComparison.OrdinalIgnoreCase))
                {
                    fields[index] = fields[index].Substring(0, fields[index].Length - 1);
                }
            }

            return fields;
        }

        /// <summary>
        /// Gets or sets the processor.
        /// </summary>
        /// <value>The processor.</value>
        private ResourceProcessor Processor
        {
            get;
            set;
        }

        private delegate void ImportMetadataItemsDelegate(ListViewItem[] newItems);

        private delegate void InsertListViewItemDelegate(ListViewItem newItem);

        private delegate void UpdateLinkProgressDelegate(ResourceMetadata metadata);
    }
}