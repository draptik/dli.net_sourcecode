using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Primitives;

namespace Ploeh.Samples.Menu.Mef
{
    public class SauceCatalog : ComposablePartCatalog
    {
        private readonly ComposablePartCatalog catalog;

        public SauceCatalog(ComposablePartCatalog cat)
        {
            if (cat == null)
            {
                throw new ArgumentNullException("cat");
            }

            this.catalog = cat;
        }

        public override
            IQueryable<ComposablePartDefinition> Parts
        {
            get
            {
                return this.catalog.Parts.Where(def =>
                    def.ExportDefinitions.Any(x => 
                        x.ContractName
                            .Contains("Sauce")));
            }
        }
    }
}
