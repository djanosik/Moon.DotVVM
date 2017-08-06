using DotVVM.Framework.Binding;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;

namespace Moon.DotVVM.Controls
{
    /// <summary>
    /// Decorates an element with properties required for better menu items.
    /// </summary>
    public class MenuItem : Decorator
    {
        /// <summary>
        /// Gets or sets the menu item's route name. When the current route name matches "{RouteName}"
        /// or when it starts with "{RouteName}/", the element will have an "active" class.
        /// </summary>
        [AttachedProperty(typeof(string)), MarkupOptions(AllowBinding = false)]
        public static readonly ActiveDotvvmProperty RouteNameProperty =
            DelegateActionProperty<string>.Register<MenuItem>("RouteName", AddActiveClass);

        private static void AddActiveClass(IHtmlWriter writer, IDotvvmRequestContext context, DotvvmProperty property, DotvvmControl control)
        {
            var currentRouteName = context.Route.RouteName;
            var menuItemRouteName = (string)property.GetValue(control);

            if (currentRouteName == menuItemRouteName || currentRouteName.StartsWith($"{menuItemRouteName}/"))
            {
                writer.AddAttribute("class", "active", true);
            }
        }
    }
}