using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;
using OpenMod.Unturned.Plugins;
using System;

[assembly: PluginMetadata("EvolutionPlugins.OpenDeathMessages", Author = "EvolutionPlugins", DisplayName = "Open Death Messages",
    Website = "https://discord.gg/5MT2yke")]

namespace EvolutionPlugins.OpenDeathMessages
{
    public class OpenDeathMessages : OpenModUnturnedPlugin
    {
        private readonly ILogger<OpenDeathMessages> m_Logger;

        public OpenDeathMessages(IServiceProvider serviceProvider, ILogger<OpenDeathMessages> logger) : base(serviceProvider)
        {
            m_Logger = logger;
        }

        protected override UniTask OnLoadAsync()
        {
            m_Logger.LogInformation($"Made with <3 by {Author}");
            m_Logger.LogInformation("https://github.com/evolutionplugins \\ https://github.com/diffoz");
            m_Logger.LogInformation($"Support discord: {Website}");

            return UniTask.CompletedTask;
        }
    }
}
