using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Mef.ExportProvided
{
    public class OliveOilExportProvider : ExportProvider
    {
        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            if (typeof(OliveOil).FullName != definition.ContractName)
            {
                return Enumerable.Empty<Export>();
            }

            var export = new Export(definition.ContractName, () => new OliveOil());
            return new[] { export };
        }
    }
}
