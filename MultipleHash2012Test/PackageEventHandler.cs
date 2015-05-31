using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleHash2012Test
{
    public class PackageEventHandler : DefaultEvents
    {
        public List<string> eventMessages;
        public PackageEventHandler()
        {
            eventMessages = new List<string>();
        }

        public override bool OnError(DtsObject source, int errorCode, string subComponent, string description, string helpFile, int helpContext, string idofInterfaceWithError)
        {
            HandleEvent("Error", errorCode, subComponent, description);
            return base.OnError(source, errorCode, subComponent, description, helpFile, helpContext, idofInterfaceWithError);
        }

        public override void OnInformation(DtsObject source, int informationCode, string subComponent, string description, string helpFile, int helpContext, string idofInterfaceWithError, ref bool fireAgain)
        {
            HandleEvent("Information", informationCode, subComponent, description);
            base.OnInformation(source, informationCode, subComponent, description, helpFile, helpContext, idofInterfaceWithError, ref fireAgain);
        }

        public override void OnWarning(DtsObject source, int warningCode, string subComponent, string description, string helpFile, int helpContext, string idofInterfaceWithError)
        {
            HandleEvent("Warning", warningCode, subComponent, description);
            base.OnWarning(source, warningCode, subComponent, description, helpFile, helpContext, idofInterfaceWithError);
        }

        private void HandleEvent(string type, int errorCode, string subComponent, string description)
        {
            eventMessages.Add(String.Format("[{0}] {1}: {2}: {3}", type, errorCode, subComponent, description));
        }

    }
}
