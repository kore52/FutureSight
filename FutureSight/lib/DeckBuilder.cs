﻿namespace FutureSight.lib
{
    public class DeckBuilder
    {
        public static void Initialize()
        {
            MTGCardInfoLoader.GetInstance().ReadFromCSVFile("CardInfo.csv");
        }
        
        public static void LoadDeck(MTGPlayer player, string filename)
        {
            // stub
//            player.Library.Add();
        }
    }
}