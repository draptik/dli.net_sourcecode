using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AddingReferences
{
    public interface IRepository
    {
        string GetMessage(int messageId);
    }
}
