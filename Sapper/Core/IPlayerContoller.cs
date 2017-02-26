using Sapper.Core.Primitives;

namespace Sapper.Core
{
    public interface IPlayerController
    {
        Turn MakeTurn(FieldView levelView);
    }
}
