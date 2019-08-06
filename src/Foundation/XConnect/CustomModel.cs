using System.Reflection;
using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using TestSite.SC91.Foundation.XConnect.Model;

namespace TestSite.SC91.Foundation.XConnect
{
    public class CustomModel
    {
        public static XdbModel Model { get; } = BuildModel();

        private static XdbModel BuildModel()
        {
            string fullName = MethodBase.GetCurrentMethod().DeclaringType?.FullName ?? "CustomModel";
            XdbModelBuilder builder = new XdbModelBuilder(fullName, new XdbModelVersion(1, 0));
            builder.ReferenceModel(Sitecore.XConnect.Collection.Model.CollectionModel.Model);

            builder.DefineFacet<Contact, CustomerInfo>(CustomerInfo.DefaultFacetKey);

            return builder.BuildModel();
        }
    }
}
