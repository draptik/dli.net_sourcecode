using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.MethodInjection
{
    public class AddInClient
    {
        private readonly IEnumerable<IAddIn> addIns;
        private readonly ISomeContext context;

        public AddInClient(IEnumerable<IAddIn> addIns)
        {
            if (addIns == null)
            {
                throw new ArgumentNullException("addIns");
            }

            this.addIns = addIns;
            this.context = new AddInClientContext();
        }

        public SomeValue DoStuff(SomeValue value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var returnValue = new SomeValue();
            returnValue.Message = value.Message;

            foreach (var addIn in this.addIns)
            {
                returnValue.Message = 
                    addIn.DoStuff(returnValue, this.context);
            }

            return returnValue;
        }

        private class AddInClientContext : ISomeContext
        {
            #region ISomeContext Members

            public string Name
            {
                get { return "AddInClient"; }
            }

            #endregion
        }

    }
}
