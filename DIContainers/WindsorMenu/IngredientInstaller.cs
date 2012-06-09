using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Ploeh.Samples.MenuModel;

namespace Ploeh.Samples.Menu.Windsor
{
    public class IngredientInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, 
            IConfigurationStore store)
        {
            container.Register(AllTypes
                .FromAssemblyContaining<Steak>()
                .BasedOn<IIngredient>());
        }
    }
}
