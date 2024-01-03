using Microsoft.AspNetCore.Mvc.Razor;

namespace WebApp;

public class AppViewLocator : IViewLocationExpander
{
    public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context, 
            IEnumerable<string> viewLocations) {
        return new[] { "/{1}/{0}.cshtml" }.Union(viewLocations);
    }

    public void PopulateValues(ViewLocationExpanderContext context) { }
}
