using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpasBank.Classes.JsonClasses
{
    [JsonObject]
    public class Balance
    {
        [JsonProperty]
        public double balance { get; set; }
    }
}
