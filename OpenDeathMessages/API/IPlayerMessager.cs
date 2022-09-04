using EvolutionPlugins.OpenDeathMessages.Services;
using OpenMod.API.Ioc;
using OpenMod.Unturned.Players;
using System.Drawing;
using System.Threading.Tasks;

namespace EvolutionPlugins.OpenDeathMessages.API
{
    [Service]
    public interface IPlayerMessager
    {
        Task SendMessageGlobalOrGroupAsync(UnturnedPlayer player, string message, string? iconUrl, Color color);

        Task ChangeDisplayTypeAsync(UnturnedPlayer player, DisplayType displayType);
    }
}
