﻿using Dominion;
using System;
using System.Linq;

namespace Program.DefaultStrategies
{
    internal class Lookout
    {
        public static bool ShouldPlay(GameState gameState, PlayerAction playerAction)
        {
            int cardCountToTrash = Strategies.CountInDeck(Cards.Copper, gameState);

            if (!playerAction.purchaseOrder.DoesCardPickerMatch(gameState, Cards.Estate))
            {
                cardCountToTrash += Strategies.CountInDeck(Cards.Estate, gameState);
            }

            cardCountToTrash += Strategies.CountInDeck(Cards.Curse, gameState);
            cardCountToTrash += Strategies.CountInDeck(Cards.Hovel, gameState);
            cardCountToTrash += Strategies.CountInDeck(Cards.Necropolis, gameState);
            cardCountToTrash += Strategies.CountInDeck(Cards.OvergrownEstate, gameState);

            if (!playerAction.purchaseOrder.DoesCardPickerMatch(gameState, Cards.Lookout))
            {
                cardCountToTrash += Strategies.CountInDeck(Cards.Lookout, gameState);
            }

            int totalCardsOwned = gameState.Self.CardsInDeck.Count;

            return ((double)cardCountToTrash) / totalCardsOwned > 0.4;
        }
    }
}