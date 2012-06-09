using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AddingReferences
{
    public class SqlRepository : IRepository
    {
        public SqlRepository(string connectionString)
        {

        }

        #region IRepository Members

        public string GetMessage(int messageId)
        {
            return "Fnaah";
        }

        #endregion
    }
}
