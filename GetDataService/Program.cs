using DNTPersianUtils.Core;
using GetDataService.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace GetDataService
{
    class Program
    {
        static string conString = "User Id=ili;Password=iliqaz987;Data Source=86.104.46.204:1521/kanoondb;";
        static string now;
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {

            test();
            now = DateTime.Now.AddDays(-1).ToShortPersianDateString();
            List<UserModel> users = GetUsers();
            List<UserModel> Teachers = GetTeachers();
            users.AddRange(Teachers);
            foreach (var item in users)
            {
                SendDataViewModel vm = new SendDataViewModel()
                {
                    Date = $"{item.Date}-{item.Time}",
                    GroupCode = item.ClassUniqCode,
                    IsTeacher = item.IsTeacher,
                    LastName = item.LastName,
                    Name = item.Name,
                    Password = item.UniqCode,
                    UserName = item.MeliCode,

                };
                var res = client.PostAsJsonAsync("http://localhost:49478/Users/AddUser", vm).Result;
                res.EnsureSuccessStatusCode();
                var data = res.Content.ReadAsStringAsync();
                Console.WriteLine(data);
            }


        }

        private static void test()
        {
            SendDataViewModel vm = new SendDataViewModel()
            {
                Date = "{item.Date}-{item.Time}",
                GroupCode = "item.ClassUniqCode",
                IsTeacher = true,
                LastName = "item.LastName",
                Name = "item.Name",
                Password = "item.UniqCode",
                UserName = "item.MeliCode",

            };
            var res = client.PostAsJsonAsync("http://localhost:5000/Users/AddUser", vm).Result;
            res.EnsureSuccessStatusCode();
            var data = res.Content.ReadFromJsonAsync<AddUserResultViewModel>();
            Console.WriteLine(data);
        }

        private static List<UserModel> GetTeachers()
        {
            string Query = $"select distinct TEACHER_FNAME,TEACHER_LNAME,TEACHER_CODE,CLASS_UNICODE,DTE_SABT from zaban.v_ili ";
            //Query += $" Where DTE_SABT='{now}'";
            List<UserModel> ls = new List<UserModel>();
            using (OracleConnection con = new OracleConnection(conString))
            {
                using OracleCommand cmd = con.CreateCommand();
                try
                {
                    con.Open();
                    Console.WriteLine("Connect Success ...");
                    cmd.BindByName = true;

                    cmd.CommandText = Query;

                    OracleDataReader reader = cmd.ExecuteReader();
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    while (reader.Read())
                    {

                        UserModel user = new UserModel()
                        {
                            Name = reader[0].ToString(),
                            LastName = reader[1].ToString(),
                            MeliCode = reader[2].ToString(),
                            UniqCode = reader[2].ToString(),
                            ClassUniqCode = reader[3].ToString(),
                            RegisterDate = reader[4].ToString(),
                            IsTeacher = true
                        };

                        ls.Add(user);
                        Console.WriteLine("User Added");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Press 'Enter' to continue");

                    reader.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
            return ls;
        }

        private static List<UserModel> GetUsers()
        {
            string Query = "select  * from zaban.V_ili";
            //Query += $" Where DTE_SABT='{now}'";

            List<UserModel> ls = new List<UserModel>();
            using (OracleConnection con = new OracleConnection(conString))
            {
                using OracleCommand cmd = con.CreateCommand();
                try
                {
                    con.Open();
                    Console.WriteLine("Connect Success ...");
                    cmd.BindByName = true;

                    cmd.CommandText = Query;

                    OracleDataReader reader = cmd.ExecuteReader();
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    while (reader.Read())
                    {

                        UserModel user = new UserModel()
                        {
                            Name = reader[0].ToString(),
                            LastName = reader[1].ToString(),
                            MeliCode = reader[2].ToString(),
                            UniqCode = reader[3].ToString(),
                            ClassUniqCode = reader[4].ToString(),
                            RegisterDate = reader[5].ToString(),
                            Date = reader[11].ToString(),
                            Time = reader[12].ToString(),
                            IsTeacher = false
                        };

                        ls.Add(user);
                        Console.WriteLine("User Added");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Press 'Enter' to continue");

                    reader.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
            return ls;
        }
    }
}
