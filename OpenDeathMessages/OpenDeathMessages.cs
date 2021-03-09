using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;

[assembly: PluginMetadata("OpenDeathMessages", Author = "EvolutionPlugins", DisplayName = "Open Death Messages",
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
