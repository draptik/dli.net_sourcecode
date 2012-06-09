using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Ploeh.Samples.CommerceService
{
    public partial class CommerceInstanceProvider : 
        IInstanceProvider, IContractBehavior
    {
        private readonly ICommerceServiceContainer container;

        public CommerceInstanceProvider(
            ICommerceServiceContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }
    }

    public partial class CommerceInstanceProvider
    {
        #region IInstanceProvider Members

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return this.container
                .ResolveProductManagementService();
        }

        public void ReleaseInstance(InstanceContext instanceContext,
            object instance)
        {
            this.container.Release(instance);
        }

        #endregion
    }

    public partial class CommerceInstanceProvider
    {
        #region IContractBehavior Members

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(
            ContractDescription contractDescription, ServiceEndpoint endpoint,
            DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = this;
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        #endregion
    }
}
