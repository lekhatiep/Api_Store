using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helper
{
    public static class AppSettings
    {
        public const string ISSUER = "adminMyshop@gmail.com";

        public const string AUDIENCE = "adminMyshop@gmail.com";

        public const string SECRET_KEY = "KeyOfMyshop10121994"; // in appsettings.json

    }
}
