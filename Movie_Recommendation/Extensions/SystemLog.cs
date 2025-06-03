using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Recommendation.Extensions
{
   
        public class SystemLog 
        {
            public string Type { get; set; }
            public string Message { get; set; }
            public AlertLevel Level { get; set; }
        }
        public enum AlertLevel
        {
            Info, Warning, Error, Success
        }
    
}
