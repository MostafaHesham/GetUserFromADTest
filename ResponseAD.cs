using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetUserFromADTest
{
    public class ResponseAD
    {
        public bool IsSuccess { get; set; }
        public bool IsException { get; set; }
        public bool IsEmpty { get; set; }
        public string ExceptionMSG { get; set; }
        public string EmptyMSG { get; set; }
        public List<UserProfileInfoAdDto> UsersData { get; set; }
    }
}
