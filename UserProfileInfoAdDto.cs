using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetUserFromADTest
{
    public class UserProfileInfoAdDto
    {
        public string Cn { get; set; }
        public string Company { get; set; }
        public string Mail { get; set; }
        public string BusinessPhone { get; set; }
        public string Department { get; set; }
        public string DepartmentAr { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string DisplayNameAr { get; set; }
        public string EmployeeId { get; set; }
        public string UserType { get; set; }
        public string GivenName { get; set; }
        public string LastLogon { get; set; }
        public string MailNickname { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string SAMAccountName { get; set; }
        public string Sn { get; set; }
        public string JobTitle { get; set; }
        public string JobTitleAr { get; set; }
        public string UserPrincipalName { get; set; }
        public string Id { get; set; }
        public string Photo { get; set; }
        public string ManagerPath { get; set; }
        public UserProfileInfoAdDto Manager { get; set; }
    }
}
