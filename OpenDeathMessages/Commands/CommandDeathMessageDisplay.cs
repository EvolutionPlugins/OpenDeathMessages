using Cysharp.Threading.Tasks;
using EvolutionPlugins.OpenDeathMessages.API;
using EvolutionPlugins.OpenDeathMessages.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using OpenMod.Core.Commands;
using OpenMod.Unturned.Commands;
using OpenMod.Unturned.Users;
using System;

namespace EvolutionPlugins.OpenDeathMessages.Commands
{
    [Command("deathmessagedisplay")]
    [CommandAlias("dmd")]
    [CommandActor(typeof(UnturnedUser))]
    [CommandSyntax("<group|global>")]
    public class CommandDeathMessageDisplay : UnturnedCommand
    {
        private readonly IPlayerMessager m_PlayerMessager;
        private readonly IConfiguration m_Configuration;
        private readonly IStringLocalizer m_StringLocalizer;

        public CommandDeathMessageDisplay(IServiceProvider serviceProvider, IPlayerMessager playerMessager, IConfiguration configuration,
            IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            m_PlayerMessager = playerMessager;
            m_Configuration = configuration;
            m_StringLocalizer = stringLocalizer;
        }

        protected override async UniTask OnExecuteAsync()
        {
            if (Context.Parameters.Count != 1)
            {
                throw new CommandWrongUsageException(Context);
            }

            var displayTypeString = Context.Parameters[0];

            DisplayType displayType;
            if (displayTypeString.Equals("group", StringComparison.InvariantCultureIgnoreCase))
            {
                displayType = DisplayType.Group;
            }
            else if (displayTypeString.Equals("global", StringComparison.InvariantCultureIgnoreCase))
            {
                displayType = DisplayType.Global;
            }
            else
            {
                throw new CommandWrongUsageException(Context);
            }

            var user = (UnturnedUser)Context.Actor;
            await m_PlayerMessager.ChangeDisplayTypeAsync(user.Player, displayType);

            await PrintAsync(m_StringLocalizer[$"commands:deathMessageDisplay:{(displayType is DisplayType.Global ? "global" : "group")}"]);
        }
    }
}
