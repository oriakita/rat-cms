using System;
using System.Collections.Generic;
using System.Text;

namespace Raci.Common.Models
{
    public class RaciAccountModel : BaseModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
