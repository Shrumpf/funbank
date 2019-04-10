using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpasBank.Classes.JsonClasses
{
    [JsonObject]
    public class AtmContainer
    {
        [JsonProperty]
        public int id { get; set; }

        public int[] bills = { 0, 0, 0, 0, 0, 0, 0 };
        [JsonProperty]
        public string zip { get; set; }


        [JsonProperty]
        public int fivehundred
        {
            get
            {
                return bills[0];
            }
            set
            {
                bills[0] = value;
            }
        }

        [JsonProperty]
        public int twohundred
        {
            get
            {
                return bills[1];
            }
            set
            {
                bills[1] = value;
            }
        }

        [JsonProperty]
        public int onehundred
        {
            get
            {
                return bills[2];
            }
            set
            {
                bills[2] = value;
            }
        }

        [JsonProperty]
        public int fifty
        {
            get
            {
                return bills[3];
            }
            set
            {
                bills[3] = value;
            }
        }

        [JsonProperty]
        public int twenty
        {
            get
            {
                return bills[4];
            }
            set
            {
                bills[4] = value;
            }
        }


        [JsonProperty]
        public int ten
        {
            get
            {
                return bills[5];
            }
            set
            {
                bills[5] = value;
            }
        }


        [JsonProperty]
        public int five
        {
            get
            {
                return bills[6];
            }
            set
            {
                bills[6] = value;
            }
        }
    }
}
