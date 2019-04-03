using System;
using Sitecore.XConnect;

namespace TestSite.SC91.Foundation.XConnect.Model
{
    [FacetKey(DefaultFacetKey)]
    [Serializable]
    public class CustomerInfo : Facet
    {
        public const string DefaultFacetKey = "CustomerInfo";

        public int? CustomerId { get; set; }
        public bool? IsEmployee { get; set; }
    }
}
