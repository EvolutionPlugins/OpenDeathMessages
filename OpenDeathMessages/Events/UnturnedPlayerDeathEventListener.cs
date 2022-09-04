using EvolutionPlugins.OpenDeathMessages.API;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Locations;
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Users;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;

namespace EvolutionPlugins.OpenDeathMessages.Events
{
    public class UnturnedPlayerDeathEventListener : IEventListener<UnturnedPlayerDeathEvent>
    {
        private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IConfiguration m_Configuration;
        private readonly IUnturnedLocationDirectory m_UnturnedLocationDirectory;
        private readonly IPlayerMessager m_PlayerMessager;

        public UnturnedPlayerDeathEventListener(IUnturnedUserDirectory unturnedUserDirectory,
            IStringLocalizer stringLocalizer, IConfiguration configuration,
            IUnturnedLocationDirectory unturnedLocationDirectory, IPlayerMessager playerMessager)
        {
            m_UnturnedUserDirectory = unturnedUserDirectory;
            m_StringLocalizer = stringLocalizer;
            m_Configuration = configuration;
            m_UnturnedLocationDirectory = unturnedLocationDirectory;
            m_PlayerMessager = playerMessager;
        }

        public async Task HandleEventAsync(object? sender, UnturnedPlayerDeathEvent @event)
        {
            var victimUser = m_UnturnedUserDirectory.FindUser(@event.Player.SteamId);
            if (victimUser == null)
            {
                return;
            }

            var instigatorUser = m_UnturnedUserDirectory.FindUser(@event.Instigator);

            var victimPosition = victimUser.Player.Transform.Position;

            var distance = Vector3.Distance(victimPosition,
                instigatorUser?.Player.Transform.Position ?? victimUser.Player.Transform.Position);

            var location = m_UnturnedLocationDirectory.GetNearestLocation(victimPosition);

            var message = m_StringLocalizer[$"deathCause:{@event.DeathCause.ToString().ToLower()}", new
            {
                Victim = victimUser,
                Instigator = instigatorUser,
                @event.DeathPosition,
                Distance = distance,
                Node = location?.Name ?? string.Empty,
                Limb = m_StringLocalizer[$"limbParse:{@event.Limb.ToString().ToLower()}"].Value
            }];

            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            await m_PlayerMessager.SendMessageGlobalOrGroupAsync(victimUser.Player, message,
                m_Configuration["iconUrl"], ColorTranslator.FromHtml(m_Configuration["color"]));
        }
    }
}
