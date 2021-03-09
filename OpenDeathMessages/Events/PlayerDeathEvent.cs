using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.API.Users;
using OpenMod.UnityEngine.Extensions;
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace EvolutionPlugins.OpenDeathMessages.Events
{
    public class PlayerDeathEvent : IEventListener<UnturnedPlayerDeathEvent>
    {
        private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
        private readonly IUserManager m_UserManager;
        private readonly IStringLocalizer m_StringLocalizer;

        public PlayerDeathEvent(IUnturnedUserDirectory unturnedUserDirectory, IUserManager userManager,
            IStringLocalizer stringLocalizer)
        {
            m_UnturnedUserDirectory = unturnedUserDirectory;
            m_UserManager = userManager;
            m_StringLocalizer = stringLocalizer;
        }

        public async Task HandleEventAsync(object? sender, UnturnedPlayerDeathEvent @event)
        {
            var victimUser = m_UnturnedUserDirectory.FindUser(@event.Player.SteamId);
            if (victimUser == null)
            {
                return;
            }

            var instigatorUser = m_UnturnedUserDirectory.FindUser(@event.Instigator);

            var provider = victimUser.Provider ?? m_UserManager.UserProviders.FirstOrDefault(x => x is UnturnedUserProvider);
            if (provider == null)
            {
                return;
            }

            var deathPositionUVector = @event.DeathPosition.ToUnityVector();
            var distance = Vector3.Distance(victimUser.Player.Transform.Position,
                instigatorUser?.Player.Transform.Position ?? victimUser.Player.Transform.Position);

            var nearNode = LevelNodes.nodes.OfType<LocationNode>()
                .OrderBy(x => (x.point - deathPositionUVector).sqrMagnitude)
                .FirstOrDefault();

            await provider.BroadcastAsync(m_StringLocalizer[$"deathCause:{@event.DeathCause.ToString().ToLower()}", new
            {
                Victim = victimUser,
                Instigator = instigatorUser,
                @event.DeathPosition,
                Distance = distance,
                Node = nearNode?.name, // can smartFormat parse fields?
                Limb = m_StringLocalizer[$"limbParse:{@event.Limb.ToString().ToLower()}"].Value
            }]);
        }
    }
}
