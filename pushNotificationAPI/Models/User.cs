
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pushNotificationAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string RegisterationToken { get; set; }
         
    }
}
