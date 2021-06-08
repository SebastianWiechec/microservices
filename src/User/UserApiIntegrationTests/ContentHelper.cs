using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UserApiIntegrationTests
{
    public static class ContentHelper
    {
        public static StringContent GetStringContent(object obj, string param = null)
            => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
    }
}
