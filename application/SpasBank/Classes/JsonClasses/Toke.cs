using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpasBank.Classes.JsonClasses
{
    [JsonObject]
    public class Token
    {
        [JsonProperty]
        public int token { get; set; }
    }
}
