using System;
using System.Collections.Generic;

namespace Core.Web.Extensions
{
    public sealed class DefaultSettings
    {
        private static readonly object SyncRoot = new object();

        private DefaultSettings() { }

        private static DefaultSettings instance;
        public static DefaultSettings Instance
        {
            get
            {
                lock (SyncRoot)
                {
                    if (instance == null)
                        instance = new DefaultSettings();
                    instance.PageSizes = new List<int> { 15, 25, 50, 100, 200, 300, 400 };
                    instance.ServerDate = DateTime.Now;
                    instance.ApplicationDisplayName = "BET Commerce";
                    instance.ApplicationCopyright = $"Copyright © 2020-{DateTime.Now.Year}. All rights reserved.";
                    instance.EndDateTimeSpan = new TimeSpan(23, 59, 59);                    
                    instance.CreatedDate = "CreatedDate";
                }

                return instance;
            }
        }

        public List<int> PageSizes { get; private set; } = new List<int>();

        public DateTime ServerDate { get; set; }

        public string ApplicationDisplayName { get; private set; } = string.Empty;

        public string ApplicationCopyright { get; private set; } = string.Empty;

        public TimeSpan EndDateTimeSpan { get; private set; }

        public string CreatedDate { get; private set; } = string.Empty;

    }
}
