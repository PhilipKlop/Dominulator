﻿using Dominion;
using CardTypes = Dominion.CardTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public static partial class Strategies
    {
        public static class DoubleWarehouse
        {
            // big money smithy player
            public static PlayerAction Player(int playerNumber)
            {
                return new PlayerAction(
                            "DoubleWarehouse",
                            playerNumber,
                            purchaseOrder: PurchaseOrder(),
                            actionOrder: ActionOrder(),
                            discardOrder: DiscardOrder());
            }

            static CardPickByPriority PurchaseOrder()
            {
                return new CardPickByPriority(
                           CardAcceptance.For<CardTypes.Province>(gameState => gameState.players.CurrentPlayer.AllOwnedCards.Where(card => card is CardTypes.Gold).Count() > 2),
                           CardAcceptance.For<CardTypes.Duchy>(gameState => CountOfPile<CardTypes.Province>(gameState) < 5),
                           CardAcceptance.For<CardTypes.Estate>(gameState => CountOfPile<CardTypes.Province>(gameState) < 2),
                           CardAcceptance.For<CardTypes.Gold>(),
                           CardAcceptance.For<CardTypes.Warehouse>(gameState => gameState.players.CurrentPlayer.AllOwnedCards.Where(card => card is CardTypes.Warehouse).Count() < 1),
                           CardAcceptance.For<CardTypes.Warehouse>(gameState => gameState.players.CurrentPlayer.AllOwnedCards.Where(card => card is CardTypes.Silver).Count() > 2 &&
                                                                                gameState.players.CurrentPlayer.AllOwnedCards.Where(card => card is CardTypes.Warehouse).Count() < 2),
                           CardAcceptance.For<CardTypes.Silver>());

            }

            static CardPickByPriority ActionOrder()
            {
                return new CardPickByPriority(
                    CardAcceptance.For<CardTypes.Warehouse>());
            }

            static CardPickByPriority DiscardOrder()
            {
                return new CardPickByPriority(
                    CardAcceptance.For<CardTypes.Province>(),
                    CardAcceptance.For<CardTypes.Duchy>(),
                    CardAcceptance.For<CardTypes.Estate>(),
                    CardAcceptance.For<CardTypes.Copper>(),
                    CardAcceptance.For<CardTypes.Silver>(),
                    CardAcceptance.For<CardTypes.Warehouse>(),
                    CardAcceptance.For<CardTypes.Gold>());
            }
        }     
    }
}