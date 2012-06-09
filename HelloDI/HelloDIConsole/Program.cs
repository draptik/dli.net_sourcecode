using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Security.Principal;

namespace Ploeh.Samples.HelloDI.CommandLine
{
    internal class Program
    {
        private static void Main()
        {
            /* Replace the hard-coded initialization of ConsoleMessageWriter with the below three
             * statements to use an example of Constrained Construction late binding. */
            //var typeName = 
            //    ConfigurationManager.AppSettings["messageWriter"];
            //var type = Type.GetType(typeName, true);
            //IMessageWriter writer = 
            //    (IMessageWriter)Activator.CreateInstance(type);

            /* Replace the hard-coded initialization of ConsoleMessageWriter with the below two
             * statements to use an example of a Decorator. */
            //Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            //IMessageWriter writer = 
            //    new SecureMessageWriter(
            //        new ConsoleMessageWriter());

            IMessageWriter writer = new ConsoleMessageWriter();
            var salutation = new Salutation(writer);
            salutation.Exclaim();
        }
    }
}
