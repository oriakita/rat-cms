using System;
using System.Collections.Generic;
using System.Text;

namespace Raci.Common.Models
{
    public class BitwiseActionId
    {
        public const int View = 1;
        public const int Add = 2;
        public const int Edit = 4;
        public const int Delete = 8;
        public const int Import = 16;
        public const int Export = 32;
    }
}
