﻿namespace EPRN.Portal.Models
{
    public class ServicesConfiguration
    {
        public static string Name => "Services";

        public Service Waste { get; set; }
    }

    public class Service
    {
        public string Url { get; set; }
    }
}
