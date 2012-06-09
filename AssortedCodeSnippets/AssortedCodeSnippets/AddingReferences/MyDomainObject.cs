using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AddingReferences
{
    public class MyDomainObject
    {
        private readonly IRepository repository;

        public MyDomainObject(IRepository repository)
        {
            this.repository = repository;
        }

        public string GetStuff()
        {
            return this.repository.GetMessage(1);
        }
    }
}
