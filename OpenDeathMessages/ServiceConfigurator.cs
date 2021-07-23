using EvolutionPlugins.Universal.Extras.Broadcast;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenMod.API.Ioc;

namespace EvolutionPlugins.OpenDeathMessages
{
    public class ServiceConfigurator : IServiceConfigurator
    {
        public void ConfigureServices(IOpenModServiceConfigurationContext openModStartupContext, IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IBroadcastManager, BroadcastManager>();
        }
    }
}
