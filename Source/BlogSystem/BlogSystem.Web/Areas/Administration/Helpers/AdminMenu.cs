namespace BlogSystem.Web.Areas.Administration.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BlogSystem.Web.Areas.Administration.Controllers;
    using BlogSystem.Web.Helpers;

    public static class AdminMenu
    {
        private static IEnumerable<string> controllers;

        public static IEnumerable<string> Items
        {
            get
            {
                return controllers ?? (controllers = GetControllerNames());
            }
        }

        private static IEnumerable<string> GetControllerNames()
        {
            return ReflectionHelper.GetSubClasses<AdminController>().Select(c => c.Name.Replace("Controller", string.Empty).PascalCaseToText());
        }
    }
}