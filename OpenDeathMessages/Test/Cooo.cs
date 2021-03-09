#if DEBUG

using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using SDG.Unturned;
using Steamworks;
using System;

namespace EvolutionPlugins.OpenDeathMessages.Test
{
    [Command("comm")]
    public class Cooo : UnturnedCommand
    {
        public Cooo(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override async UniTask OnExecuteAsync()
        {
            var user = (UnturnedUser)Context.Actor;

            await UniTask.SwitchToMainThread();
            DamageTool.damagePlayer(new(user.Player.Player)
            {
                damage = 101,
                applyGlobalArmorMultiplier = false,
                times = 1,
                killer = await Context.Parameters.GetAsync<bool>(1) ? CSteamID.Nil : user.SteamId,
                cause = (EDeathCause)Enum.Parse(typeof(EDeathCause), Context.Parameters[0], true)
            }, out _);
        }
    }
}
#endif
