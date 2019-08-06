using System;
using Sitecore.Framework.Rules;

namespace TestSite.SC91.Feature.AutomationRules.Interfaces
{
    public interface ICustomCondition : ICondition
    {
        Guid AssociatedItemId { get; }
    }
}
