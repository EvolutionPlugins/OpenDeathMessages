using Microsoft.Extensions.Localization;
using OpenMod.API.Eventing;
using OpenMod.API.Users;
using OpenMod.UnityEngine.Extensions;
using OpenMod.Unturned.Players.Life.Events;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using System;
using System.Drawing;
using System.Linq;
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

            var uVector = @event.DeathPosition.ToUnityVector();
            var nearNode = LevelNodes.nodes.Cast<LocationNode>()
                .OrderBy(x => (x.point - uVector).sqrMagnitude)
                .FirstOrDefault();

            await provider.BroadcastAsync(m_StringLocalizer[$"deathCause:{@event.DeathCause.ToString().ToLower()}", new
            {
                Victim = victimUser,
                Instigator = instigatorUser,
                @event.DeathPosition,
                Limb = m_StringLocalizer[$"limbParse:{@event.Limb.ToString().ToLower()}"].Value
            }]);
        }

        /// <summary>
        /// If player is null or doesn't exits, it should be used
        /// </summary>
        private class DummyUser : IUser
        {
            public IUserSession? Session => throw new NotImplementedException();

            public IUserProvider? Provider => null!;

            public string Id => DisplayName;

            public string Type => DisplayName;

            public string DisplayName => "<unknown>";

            public string FullActorName => DisplayName;

            public Task<T?> GetPersistentDataAsync<T>(string key)
            {
                throw new NotImplementedException();
            }

            public Task PrintMessageAsync(string message)
            {
                throw new NotImplementedException();
            }

            public Task PrintMessageAsync(string message, Color color)
            {
                throw new NotImplementedException();
            }

            public Task SavePersistentDataAsync<T>(string key, T? data)
            {
                throw new NotImplementedException();
            }
        }
    }
}
