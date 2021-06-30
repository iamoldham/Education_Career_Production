using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Diagnostics;

namespace CareerCloud.ADODataAccessLayer
{
    public class SecurityLoginsLogRepository : IDataRepository<SecurityLoginsLogPoco>
    {
        protected readonly string _connStr = string.Empty;

        public SecurityLoginsLogRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params SecurityLoginsLogPoco[] items)
        {
            try
            {
                foreach (SecurityLoginsLogPoco loginsLogPoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Security_Logins_Log]
                                                                       ([Id]
                                                                       ,[Login]
                                                                       ,[Source_IP]
                                                                       ,[Logon_Date]
                                                                       ,[Is_Succesful])
                                                                        VALUES
                                                                       (@Id
                                                                       ,@Login
                                                                       ,@Source_IP
                                                                       ,@Logon_Date
                                                                       ,@Is_Succesful)", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", loginsLogPoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Login", loginsLogPoco.Login);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Source_IP", loginsLogPoco.SourceIP);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Logon_Date", loginsLogPoco.LogonDate);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Is_Succesful", loginsLogPoco.IsSuccesful);
                            

                            if (con.State != ConnectionState.Open)
                            {
                                con.Open();
                            }

                            dataAdapter.InsertCommand.ExecuteNonQuery();

                            if (con.State != ConnectionState.Closed)
                            {
                                con.Close();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Add - SecurityLoginsLogPoco");
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityLoginsLogPoco> GetAll(params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            IList<SecurityLoginsLogPoco> pocoResult = new List<SecurityLoginsLogPoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                            ,[Login]
                                            ,[Source_IP]
                                            ,[Logon_Date]
                                            ,[Is_Succesful]
                                            FROM [dbo].[Security_Logins_Log]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SecurityLoginsLogPoco loginsLogPoco = new SecurityLoginsLogPoco();

                        loginsLogPoco.Id = reader.GetGuid(0);
                        loginsLogPoco.Login = reader.GetGuid(1);
                        loginsLogPoco.SourceIP = reader.GetString(2);
                        loginsLogPoco.LogonDate = reader.GetDateTime(3);
                        loginsLogPoco.IsSuccesful = reader.GetBoolean(4);
                        
                        pocoResult.Add(loginsLogPoco);
                    }

                    reader.Close();

                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "GetAll - SecurityLoginsLogPoco");
            }

            return pocoResult.Where(a => a != null).ToList();
        }

        public IList<SecurityLoginsLogPoco> GetList(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginsLogPoco GetSingle(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginsLogPoco> loginsLogPoco = GetAll().AsQueryable();

            return loginsLogPoco.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginsLogPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (SecurityLoginsLogPoco loginsLogPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[Security_Logins_Log] WHERE [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", loginsLogPoco.Id);

                            if (con.State != ConnectionState.Open)
                            {
                                con.Open();
                            }

                            command.ExecuteNonQuery();

                            if (con.State != ConnectionState.Closed)
                            {
                                con.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Remove - SecurityLoginsLogPoco");
            }
        }

        public void Update(params SecurityLoginsLogPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var loginsLogPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Security_Logins_Log]
                                                    SET [Login] = @Login
                                                    ,[Source_IP] = @Source_IP
                                                    ,[Logon_Date] = @Logon_Date
                                                    ,[Is_Succesful] = @Is_Succesful                                         
                                                    WHERE [Id]= @Id";

                            command.Parameters.AddWithValue("@Id", loginsLogPoco.Id);
                            command.Parameters.AddWithValue("@Login", loginsLogPoco.Login);
                            command.Parameters.AddWithValue("@Source_IP", loginsLogPoco.SourceIP);
                            command.Parameters.AddWithValue("@Is_Succesful", loginsLogPoco.IsSuccesful);
                            command.Parameters.AddWithValue("@Logon_Date", loginsLogPoco.LogonDate);


                            if (con.State != ConnectionState.Open)
                            {
                                con.Open();
                            }

                            command.ExecuteNonQuery();

                            if (con.State != ConnectionState.Closed)
                            {
                                con.Close();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Update - SecurityLoginsLogPoco");
            }
        }
    }
}
