using Sapper.Core.Primitives;
using Sapper.Core.View;

namespace Sapper.Core
{
    public interface IPlayerController
    {
        Turn MakeTurn(FieldView levelView);
    }
}
