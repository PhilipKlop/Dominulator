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
        public static class FeodumDevelop
        {
            // big money smithy player
            public static PlayerAction Player(int playerNumber)
            {
                return new PlayerAction(
                            "FeodumDevelop",
                            playerNumber,
                            purchaseOrder: PurchaseOrder(),
                            treasurePlayOrder: Default.TreasurePlayOrder(),
                            actionOrder: ActionOrder(),
                            trashOrder: TrashOrder(),
                            discardOrder: Default.EmptyPickOrder(),
                            gainOrder: GainOrder());
            }

            private static IGetMatchingCard PurchaseOrder()
            {
                return new CardPickByPriority(
                           CardAcceptance.For<CardTypes.Province>(),
                           CardAcceptance.For<CardTypes.Develop>(ShouldGainDevelop),
                           CardAcceptance.For<CardTypes.Feodum>(ShouldGainFeodum),
                           CardAcceptance.For<CardTypes.Silver>());
            }

            private static IGetMatchingCard GainOrder()
            {
                return new CardPickByPriority(
                           CardAcceptance.For<CardTypes.Develop>(ShouldGainDevelop),
                           CardAcceptance.For<CardTypes.Feodum>(ShouldGainFeodum),
                           CardAcceptance.For<CardTypes.Silver>(),
                           CardAcceptance.For<CardTypes.Duchy>(),
                           CardAcceptance.For<CardTypes.Feodum>(),
                           CardAcceptance.For<CardTypes.Develop>());
            }

            private static CardPickByPriority ActionOrder()
            {
                return new CardPickByPriority(
                           CardAcceptance.For<CardTypes.Develop>(ShouldPlayDevelop));
            }

            private static CardPickByPriority TrashOrder()
            {
                return new CardPickByPriority(
                           CardAcceptance.For<CardTypes.Duchy>(),
                           CardAcceptance.For<CardTypes.Feodum>(ShouldTrashFeodum),
                           CardAcceptance.For<CardTypes.Estate>(),
                           CardAcceptance.For<CardTypes.Copper>());
            }

            private static bool ShouldGainDevelop(GameState gameState)
            {
                return CountAllOwned<CardTypes.Develop>(gameState) < 2 &&
                       CountAllOwned<CardTypes.Feodum>(gameState) >= CountAllOwned<CardTypes.Develop>(gameState);
            }

            private static bool ShouldPlayDevelop(GameState gameState)
            {
                var currentPlayer = gameState.players.CurrentPlayer;

                Type result;
                if (currentPlayer.Hand.Where(card => card.Is<CardTypes.Develop>()).Count() > 1)
                {
                    result = TrashOrder().GetMatchingCard(gameState, card => currentPlayer.Hand.HasCard(card));
                }
                else
                {
                    result = TrashOrder().GetMatchingCard(gameState, card => currentPlayer.Hand.HasCard(card) && !card.Is<CardTypes.Develop>());
                }

                return result != null;
            }

            private static bool ShouldTrashFeodum(GameState gameState)
            {
                int countFeodumRemaining = gameState.GetPile<CardTypes.Feodum>().Count();

                int countSilvers = CountAllOwned<CardTypes.Silver>(gameState);
                int countFeodum = CountAllOwned<CardTypes.Feodum>(gameState);

                if (countSilvers < 12)
                {
                    return true;
                }

                int scoreTrashNothing = CardTypes.Feodum.VictoryCountForSilver(countSilvers) * countFeodum;
                int scoreTrashFeodum = CardTypes.Feodum.VictoryCountForSilver((countSilvers + 4)) * (countFeodum - 1);

                return scoreTrashFeodum > scoreTrashNothing;
            }

            private static bool ShouldGainFeodum(GameState gameState)
            {
                int countFeodumRemaining = gameState.GetPile<CardTypes.Feodum>().Count();

                int countSilvers = CountAllOwned<CardTypes.Silver>(gameState);
                int countFeodum = CountAllOwned<CardTypes.Feodum>(gameState);

                if (countSilvers < 1)
                {
                    return false;
                }

                int scoreGainFeodum = CardTypes.Feodum.VictoryCountForSilver(countSilvers) * (countFeodum + 1);
                int scoreGainSilver = CardTypes.Feodum.VictoryCountForSilver((countSilvers + 1)) * (countFeodum);

                return scoreGainFeodum > scoreGainSilver;
            }
        }
    }
}