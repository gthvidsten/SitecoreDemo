using System;
using Sitecore.Framework.Rules;
using Sitecore.XConnect;
using TestSite.SC91.Feature.AutomationRules.Interfaces;
using TestSite.SC91.Foundation.XConnect.Model;

namespace TestSite.SC91.Feature.AutomationRules.AutomationConditions
{
    public class IsCustomerEmployeeCondition : ICustomCondition
    {
        public Guid AssociatedItemId => new Guid("{FEC9A694-C874-4DAF-8496-DE847473282B}");

        public bool Evaluate(IRuleExecutionContext context)
        {
            Contact contact = context.Fact<Contact>();

            bool? isEmployee = contact.GetFacet<CustomerInfo>(CustomerInfo.DefaultFacetKey)?.IsEmployee;

            if (!isEmployee.HasValue)
            {
                return false;
            }

            return isEmployee.Value;
        }
    }

    public class IsOldCustomerCondition : ICustomCondition
    {
        public Guid AssociatedItemId => new Guid("{FEC9A694-C874-4DAF-8496-DE847473282B}");

        public bool Evaluate(IRuleExecutionContext context)
        {
            Contact contact = context.Fact<Contact>();

            int? customerId = contact.GetFacet<CustomerInfo>(CustomerInfo.DefaultFacetKey)?.CustomerId;

            if (!customerId.HasValue)
            {
                return false;
            }

            return customerId.Value < 1000;
        }
    }
}
