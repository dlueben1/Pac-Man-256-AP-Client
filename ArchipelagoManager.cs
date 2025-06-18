using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApPac256
{
    public static class ArchipelagoManager
    {
        #region Connection Details

        public static string ServerAddress {get; set;}
        public static string ServerPassword { get; set;}
        public static string PlayerName { get; set;}

        #endregion

        #region Power-Ups

        public static Dictionary<string, int> PowerUps { get; set;}

        #endregion

        #region Shop

        public static LocationShop[] Purchasables { get; set; }

        #endregion

        #region Options

        public static bool DisableFreeGifts { get; set; }

        #endregion
    }
}
