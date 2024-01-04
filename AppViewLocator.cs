using Microsoft.AspNetCore.Mvc.Razor;

namespace WebApp;

public class AppViewLocator : IViewLocationExpander
{
    public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context, 
            IEnumerable<string> viewLocations) {
        return new[] { 
            "/Views/",
            "/{1}/{0}.cshtml",
            "/{1}/Views/{0}.cshtml"
        }.Union(viewLocations);
    }

    public void PopulateValues(ViewLocationExpanderContext context) { }
}
