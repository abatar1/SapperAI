using System;

namespace Soliter.Interface
{
    public class Turn
    {
        private Turn(Action<Player> action)
        {
            _action = action;
        }

        public void Apply(Player player)
        {
            _action(player);
        }

        private readonly Action<Player> _action;
    }
    
}
