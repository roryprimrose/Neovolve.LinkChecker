using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Neovolve.LinkChecker
{
    internal struct ExportDetails
    {
        public String BaseLocation;

        public IList<ResourceMetadata> Items;

        public String Path;
    }
}