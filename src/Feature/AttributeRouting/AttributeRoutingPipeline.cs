using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Loader;
using Sitecore.Pipelines;
using TestSite.SC91.Foundation.Common.Extensions;

namespace TestSite.SC91.Feature.AttributeRouting
{
    public class AttributeRoutingPipeline : InitializeRoutes
    {
        public override void Process(PipelineArgs args)
        {
            try
            {
                List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.FullName.StartsWith("TestSite.SC91") && !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .Where(t => typeof(Controller).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                    .ToList();

                foreach (Type type in types)
                {
                    string routePrefix = type.Name;
                    foreach (Attribute attribute in type.GetCustomAttributes(true))
                    {
                        if (attribute is RoutePrefixAttribute routePrefixAttribute)
                        {
                            routePrefix = routePrefixAttribute.Prefix;
                        }
                    }

                    foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                    {
                        foreach (Attribute methodAttribute in method.GetCustomAttributes(true))
                        {
                            if (methodAttribute is RouteAttribute methodRouteAttribute)
                            {
                                RouteTable.Routes.MapRoute(
                                    $"{type.FullName}.{method.Name}",
                                    $"api/custom/{routePrefix}/{methodRouteAttribute.Template}",
                                    new { controller = type.Name.TrimEnd("Controller"), action = method.Name },
                                    new[] { type.Namespace });
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException e)
            {
                Log.Error("Could not create routes", e, this);
                foreach (Exception loaderException in e.LoaderExceptions)
                {
                    Log.Error("Inner LoaderException:", loaderException, this);
                }
            }
            catch (Exception e)
            {
                Log.Error("Could not create routes", e, this);
            }
        }
    }
}