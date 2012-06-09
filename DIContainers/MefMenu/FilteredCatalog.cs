using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Linq.Expressions;

namespace Ploeh.Samples.Menu.Mef
{
    public class FilteredCatalog : ComposablePartCatalog
    {
        private readonly IQueryable<ComposablePartDefinition> parts;

        public FilteredCatalog(ComposablePartCatalog cat,
            Expression<Func<ComposablePartDefinition, bool>> exp)
        {
            if (cat == null)
            {
                throw new ArgumentNullException("cat");
            }
            if (exp == null)
            {
                throw new ArgumentNullException("exp");
            }

            this.parts = cat.Parts.Where(exp);
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return this.parts; }
        }
    }
}
