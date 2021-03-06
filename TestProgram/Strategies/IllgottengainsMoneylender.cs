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
        public static class IllgottengainsMoneylender
        {
            
            public static PlayerAction Player()
            {
                return new MyPlayerAction();
            }

            class MyPlayerAction
                : PlayerAction
            {
                public MyPlayerAction()
                    : base("IllgottengainsMoneylender",                            
                            purchaseOrder: PurchaseOrder())
                {
                }              
            }

            private static ICardPicker PurchaseOrder()
            {
                var highPriority = new CardPickByPriority(
                    CardAcceptance.For(Cards.Province),
                    CardAcceptance.For(Cards.IllGottenGains, Default.ShouldGainIllGottenGains),
                    CardAcceptance.For(Cards.Gold, gameState => CountOfPile(Cards.Province, gameState) >= 6),
                    CardAcceptance.For(Cards.Duchy),
                    CardAcceptance.For(Cards.Estate, gameState => CountOfPile(Cards.Province, gameState) < 2));                    

                var buildOrder = new CardPickByBuildOrder(
                    CardAcceptance.For(Cards.Moneylender),
                    CardAcceptance.For(Cards.Silver));

                var lowPriority = new CardPickByPriority(                           
                           CardAcceptance.For(Cards.Silver),
                           CardAcceptance.For(Cards.Copper));

                return new CardPickConcatenator(highPriority, buildOrder, lowPriority);
            }            
        }

        public static class Illgottengains
        {
            
            public static PlayerAction Player()
            {
                return new MyPlayerAction();
            }

            class MyPlayerAction
                : PlayerAction
            {
                public MyPlayerAction()
                    : base("Illgottengains",                            
                            purchaseOrder: PurchaseOrder())
                {
                }
            }

            private static ICardPicker PurchaseOrder()
            {
                var highPriority = new CardPickByPriority(
                    CardAcceptance.For(Cards.Province),
                    CardAcceptance.For(Cards.IllGottenGains, Default.ShouldGainIllGottenGains),
                    CardAcceptance.For(Cards.Gold, gameState => CountOfPile(Cards.Province, gameState) >= 6),
                    CardAcceptance.For(Cards.Duchy),
                    CardAcceptance.For(Cards.Estate, gameState => CountOfPile(Cards.Province, gameState) < 2));

                var buildOrder = new CardPickByBuildOrder(
                    CardAcceptance.For(Cards.Silver));

                var lowPriority = new CardPickByPriority(
                           CardAcceptance.For(Cards.Silver),
                           CardAcceptance.For(Cards.Copper));

                return new CardPickConcatenator(highPriority, buildOrder, lowPriority);
            }
        }
    }
}
