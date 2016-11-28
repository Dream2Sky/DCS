using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.dcs.common
{
    public class ConditionModal
    {
        public int Sex { get; set; }
        public int IsMarry { get; set; }
        public int Children { get; set; }
        public int HasCar { get; set; }
        public int HasHouse { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public string Income { get; set; }
        public string InsertTime { get; set; }
    }
}