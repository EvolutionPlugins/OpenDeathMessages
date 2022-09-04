using Cysharp.Threading.Tasks;
using EvolutionPlugins.OpenDeathMessages.API;
using Microsoft.Extensions.Configuration;
using OpenMod.API.Ioc;
using OpenMod.UnityEngine.Extensions;
using OpenMod.Unturned.Players;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace EvolutionPlugins.OpenDeathMessages.Services
{
    [PluginServiceImplementation]
    public class PlayerMessager : IPlayerMessager
    {
        private readonly DisplayType m_DisplayType;
        private readonly HashSet<CSteamID> m_GroupOnlyMessagger = new();

        public PlayerMessager(IConfiguration configuration)
        {
            m_DisplayType = (DisplayType)(configuration.GetValue("defaultDisplay:groupDeath", false) ? 1 : 0);
        }

        private bool ShouldMessageToGroup(UnturnedPlayer player) => m_GroupOnlyMessagger.Contains(player.SteamId) || m_DisplayType is DisplayType.Group;

        public Task ChangeDisplayTypeAsync(UnturnedPlayer player, DisplayType displayType)
        {
            if (displayType is DisplayType.Group)
            {
                m_GroupOnlyMessagger.Add(player.SteamId);
                return Task.CompletedTask;
            }

            m_GroupOnlyMessagger.Remove(player.SteamId);
            return Task.CompletedTask;
        }

        public async Task SendMessageGlobalOrGroupAsync(UnturnedPlayer player, string message, string? iconUrl, Color color)
        {
            var unityColor = color.ToUnityColor();
            await UniTask.SwitchToMainThread();

            if (ShouldMessageToGroup(player))
            {
                foreach (var teammate in Provider.clients
                    .Where(x => !x.player.quests.isMemberOfAGroup || x.player.quests.isMemberOfSameGroupAs(player.Player)))
                {
                    ChatManager.serverSendMessage(message, unityColor, toPlayer: teammate, iconURL: iconUrl, useRichTextFormatting: true);
                }

                return;
            }

            ChatManager.serverSendMessage(message, unityColor, iconURL: iconUrl, mode: EChatMode.GLOBAL, useRichTextFormatting: true);
        }
    }
}
