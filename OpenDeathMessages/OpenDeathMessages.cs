using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;

[assembly: PluginMetadata("EvolutionPlugins.OpenDeathMessages", Author = "EvolutionPlugins", DisplayName = "Open Death Messages",
    Website = "https://discord.gg/5MT2yke")]

namespace EvolutionPlugins.OpenDeathMessages
{
    public class OpenDeathMessages : OpenModUnturnedPlugin
    {
        public OpenDeathMessages(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
