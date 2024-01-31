using System.Collections.Generic;

namespace IAP
{
    public class IAPItems
    {
        private static readonly Dictionary<string, IAPItemType> IAPItemsDictionary = new()
        {
            {"removeads", IAPItemType.RemoveAds}
        };


        public static IAPItemType GetIAPItemType(string identifier)
        {
            return IAPItemsDictionary.ContainsKey(identifier) ? IAPItemsDictionary[identifier] : IAPItemType.Unknown;
        }
    }
}