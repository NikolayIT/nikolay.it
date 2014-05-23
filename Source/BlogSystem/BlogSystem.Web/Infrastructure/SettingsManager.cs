namespace BlogSystem.Web.Infrastructure
{
    using System;

    using System.Collections.Generic;

    public class SettingsManager
    {
        private Lazy<IDictionary<string, string>> settings;

        public SettingsManager(Func<IDictionary<string, string>> initializer)
        {
            settings = new Lazy<IDictionary<string, string>>(initializer);
        }

        public IDictionary<string, string> Get
        {
            get
            {
                return this.settings.Value;
            }
        }
    }
}