using Soliter.Views;

namespace Soliter.Interface
{
    public interface IPlayerController
    {
        Turn MakeTurn(FieldView levelView, IMessageReporter messageReporter);
    }
}
