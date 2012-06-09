using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AddingReferences
{
    public class SomeOtherDevelopersObject
    {
        private readonly string connectionString;

        public string GetThing()
        {
            var repository = new SqlRepository(this.connectionString);
            return repository.GetMessage(2);
        }
    }
}
