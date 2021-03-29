using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class  UserDeleteRequest
    {
        public Guid id { get; set; }
        public string Username { get; set; }
    }
}
