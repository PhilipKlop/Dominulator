﻿using Dominion;

namespace Program.DefaultStrategies
{
    internal class Alchemist
      : UnimplementedPlayerAction
    {
        private readonly PlayerAction playerAction;

        public Alchemist(PlayerAction playerAction)
        {
            this.playerAction = playerAction;
        }

        public override bool ShouldPutCardOnTopOfDeck(Card card, GameState gameState)
        {
            return true;
        }
    }

}
