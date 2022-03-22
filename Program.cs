using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GetUserFromADTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args).Build();
            IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

            string PropertyValue = config.GetValue<string>("UsernameAd");
            bool managerData = config.GetValue<bool>("GetManagerData");

            var response = new ResponseAD { UsersData = new List<UserProfileInfoAdDto>() };

            var PropertyName = "sAMAccountName";
            //string PropertyValue = "aabdelhalim_t";
            bool Singlevalue = true;
            string SearchQuery = "";
            //bool managerData = false;
            int pageLength = 10;
            var FilterQuery = "";
            if (Singlevalue)
                FilterQuery = $"({PropertyName}={PropertyValue})";
            else
                FilterQuery = SearchQuery;
            Console.WriteLine($"Step 1 : Open Connection");

            var myLdapConnection = CreateDirectoryEntry("LDAP://DC=MCIT,DC=LOCAL", "DIGITAL-app-sa-01", "nf4M@7612513n");
            var search = new DirectorySearcher(myLdapConnection)
            {
                SizeLimit = 10,
                Filter = FilterQuery,
                PageSize = 10
            };

            string[] requiredProperties =
               {
                    "cn", "company", "mail",
                    "department", "description", "displayName", "employeeID",
                    "employeeType", "givenName", "info", "lastLogon", "mailNickname", "manager",
                    "mobile", "name", "sAMAccountName", "sn", "telephoneNumber", "title", "userPrincipalName",
                    "thumbnailPhoto", "objectGUID","memberOf"
                };

            foreach (var property in requiredProperties)
            {
                search.PropertiesToLoad.Add(property);

            }
            Console.WriteLine($"Step 2 : Search for properties for user {PropertyValue}");
            var res = search.FindAll();
            Console.WriteLine($"Search result :{JsonConvert.SerializeObject(res)}");




            ///////////////////////////////////
            ///



            foreach (SearchResult result in search.FindAll())
            {
                var user = new UserProfileInfoAdDto();
                Console.WriteLine($"Foreach 1 : Search result :{result}");


                foreach (var property in requiredProperties)
                {
                    Console.WriteLine($"    Foreach 2 : Required Properties :{property}");

                    foreach (var myCollection in result.Properties[property])
                    {
                        Console.WriteLine($"        Foreach 3 : result.Properties :{myCollection}");
                        switch (property)
                        {
                            case "manager":
                                user.ManagerPath = myCollection.ToString();
                                if (managerData)
                                {
                                    var Result = GetUserDataByPath("LDAP://" + myCollection, "DIGITAL-app-sa-01", "nf4M@7612513n");
                                    if (Result.IsSuccess) user.Manager = Result.UsersData.FirstOrDefault();
                                }

                                break;

                            case "cn":
                                user.Cn = myCollection.ToString();
                                break;

                            case "company":
                                user.Company = myCollection.ToString();
                                break;

                            case "mail":
                                user.Mail = myCollection.ToString();
                                break;

                            case "telephoneNumber":
                                user.BusinessPhone = myCollection.ToString();
                                break;

                            case "department":
                                user.Department = myCollection.ToString();
                                break;

                            case "description":
                                user.Description = myCollection.ToString();
                                break;

                            case "displayName":
                                user.DisplayName = myCollection.ToString();
                                break;

                            case "employeeID":
                                user.EmployeeId = myCollection.ToString();
                                break;

                            case "objectGUID":
                                user.Id = new Guid(myCollection as byte[]).ToString();
                                break;

                            case "employeeType":
                                user.UserType = myCollection.ToString();
                                break;

                            case "givenName":
                                user.GivenName = myCollection.ToString();
                                break;

                            case "info":
                                user.Info = myCollection.ToString();
                                break;

                            case "lastLogon":
                                user.LastLogon = DateTime.FromFileTime((long)myCollection).ToString("s");
                                break;

                            case "mailNickname":
                                user.MailNickname = myCollection.ToString();
                                break;

                            case "mobile":
                                user.Phone = myCollection.ToString();
                                break;

                            case "name":
                                user.Name = myCollection.ToString();
                                break;

                            case "sAMAccountName":
                                user.SAMAccountName = myCollection.ToString();
                                break;

                            case "sn":
                                user.Sn = myCollection.ToString();
                                break;

                            case "title":
                                user.JobTitle = myCollection.ToString();
                                break;

                            case "userPrincipalName":
                                user.UserPrincipalName = myCollection.ToString();
                                break;

                            case "thumbnailPhoto":
                                user.Photo = Convert.ToBase64String((byte[])myCollection, 0,
                                    ((byte[])myCollection).Length);
                                break;
                        }
                    }
                }
                response.UsersData.Add(user);
                Console.WriteLine($"User : {user} added to list");
            }


            Console.WriteLine($"res-:{JsonConvert.SerializeObject(response)}");
            Console.ReadKey();
        }

        static public DirectoryEntry CreateDirectoryEntry(string domain, string Username, string Password)
        {
            var dirEnt = new DirectoryEntry(domain, Username, Password);
            return dirEnt;
        }

        static public ResponseAD GetUserDataByPath(string domain, string Username, string Password)
        {
            var response = new ResponseAD { UsersData = new List<UserProfileInfoAdDto>() };

            try
            {
                var myLdapConnection = CreateDirectoryEntry(domain, Username, Password);
                var search = new DirectorySearcher(myLdapConnection);

                var pairs = new Dictionary<string, string>();
                //create an array of properties that we would like and
                // add them to the search object

                string[] requiredProperties =
                {
                    "cn", "company", "mail", "department", "description", "displayName", "employeeID", "employeeType",
                    "givenName", "lastLogon", "mailNickname", "mobile", "name", "sAMAccountName", "sn",
                    "telephoneNumber", "title", "userPrincipalName", "thumbnailPhoto", "objectGUID"
                };

                foreach (var property in requiredProperties)
                    search.PropertiesToLoad.Add(property);

                foreach (SearchResult result in search.FindAll())
                {
                    var user = new UserProfileInfoAdDto();
                    foreach (var property in requiredProperties)
                        foreach (var myCollection in result.Properties[property])
                            switch (property)
                            {
                                case "cn":
                                    user.Cn = myCollection.ToString();
                                    break;

                                case "company":
                                    user.Company = myCollection.ToString();
                                    break;

                                case "mail":
                                    user.Mail = myCollection.ToString();
                                    break;

                                case "telephoneNumber":
                                    user.BusinessPhone = myCollection.ToString();
                                    break;

                                case "department":
                                    user.Department = myCollection.ToString();
                                    break;

                                case "description":
                                    user.Description = myCollection.ToString();
                                    break;

                                case "displayName":
                                    user.DisplayName = myCollection.ToString();
                                    break;

                                case "employeeID":
                                    user.EmployeeId = myCollection.ToString();
                                    break;

                                case "objectGUID":
                                    user.Id = new Guid(myCollection as byte[]).ToString();
                                    break;

                                case "employeeType":
                                    user.UserType = myCollection.ToString();
                                    break;

                                case "givenName":
                                    user.GivenName = myCollection.ToString();
                                    break;

                                case "info":
                                    user.Info = myCollection.ToString();
                                    break;

                                case "lastLogon":
                                    user.LastLogon = DateTime.FromFileTime((long)myCollection).ToString("s");
                                    break;

                                case "mailNickname":
                                    user.MailNickname = myCollection.ToString();
                                    break;

                                case "mobile":
                                    user.Phone = myCollection.ToString();
                                    break;

                                case "name":
                                    user.Name = myCollection.ToString();
                                    break;

                                case "sAMAccountName":
                                    user.SAMAccountName = myCollection.ToString();
                                    break;

                                case "sn":
                                    user.Sn = myCollection.ToString();
                                    break;

                                case "title":
                                    user.JobTitle = myCollection.ToString();
                                    break;

                                case "userPrincipalName":
                                    user.UserPrincipalName = myCollection.ToString();
                                    break;

                                case "thumbnailPhoto":
                                    user.Photo = Convert.ToBase64String((byte[])myCollection, 0,
                                        ((byte[])myCollection).Length);
                                    break;
                            }

                    response.UsersData.Add(user);
                }

                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.IsEmpty = true;
                response.IsException = true;
                response.ExceptionMSG = e.ToString();
            }

            return response;
        }
    }
}
