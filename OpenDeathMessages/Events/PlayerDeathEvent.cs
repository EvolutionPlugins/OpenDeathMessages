using EvolutionPlugins.Universal.Extras.Broadcast;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.API.Users;
using OpenMod.Unturned.Locations;
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Users;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;

namespace EvolutionPlugins.OpenDeathMessages.Events
{
    public class PlayerDeathEvent : IEventListener<UnturnedPlayerDeathEvent>
    {
        private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
        private readonly IUserManager m_UserManager;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly IConfiguration m_Configuration;
        private readonly IBroadcastManager m_BroadcastManager;
        private readonly IUnturnedLocationDirectory m_UnturnedLocationDirectory;

        public PlayerDeathEvent(IUnturnedUserDirectory unturnedUserDirectory, IUserManager userManager,
            IStringLocalizer stringLocalizer, IConfiguration configuration, IBroadcastManager broadcastManager,
            IUnturnedLocationDirectory unturnedLocationDirectory)
        {
            m_UnturnedUserDirectory = unturnedUserDirectory;
            m_UserManager = userManager;
            m_StringLocalizer = stringLocalizer;
            m_Configuration = configuration;
            m_BroadcastManager = broadcastManager;
            m_UnturnedLocationDirectory = unturnedLocationDirectory;
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

            await m_BroadcastManager.BroadcastAsync(m_StringLocalizer[$"deathCause:{@event.DeathCause.ToString().ToLower()}", new
            {
                Victim = victimUser,
                Instigator = instigatorUser,
                @event.DeathPosition,
                Distance = distance,
                Node = location?.Name ?? string.Empty,
                Limb = m_StringLocalizer[$"limbParse:{@event.Limb.ToString().ToLower()}"].Value
            }], m_Configuration["iconUrl"], ColorTranslator.FromHtml(m_Configuration["color"]));
        }
    }
}
