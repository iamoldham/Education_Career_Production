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
    public class SecurityLoginsRoleRepository : IDataRepository<SecurityLoginsRolePoco>
    {
        protected readonly string _connStr = string.Empty;

        public SecurityLoginsRoleRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params SecurityLoginsRolePoco[] items)
        {
            try
            {
                foreach (SecurityLoginsRolePoco loginsRolePoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Security_Logins_Roles]
                                                                        ([Id]
                                                                        ,[Login]
                                                                        ,[Role])
                                                                        VALUES(
                                                                          @Id
                                                                         ,@Login
                                                                         ,@Role
                                                                        )", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", loginsRolePoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Login", loginsRolePoco.Login);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Role", loginsRolePoco.Role);

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
                Debug.WriteLine(ex.Message, "Add - SecurityLoginsRolePoco");
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityLoginsRolePoco> GetAll(params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            IList<SecurityLoginsRolePoco> pocoResult = new List<SecurityLoginsRolePoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                            ,[Login]
                                            ,[Role]
                                            ,[Time_Stamp]
                                            FROM [dbo].[Security_Logins_Roles]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SecurityLoginsRolePoco loginsRolePoco = new SecurityLoginsRolePoco();

                        loginsRolePoco.Id = reader.GetGuid(0);
                        loginsRolePoco.Login = reader.GetGuid(1);
                        loginsRolePoco.Role = reader.GetGuid(2);
                        loginsRolePoco.TimeStamp = (byte[])reader[3];

                        pocoResult.Add(loginsRolePoco);
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
                Debug.WriteLine(ex.Message, "GetAll - SecurityLoginsRolePoco");
            }

            return pocoResult.Where(a => a != null).ToList();
        }

        public IList<SecurityLoginsRolePoco> GetList(Expression<Func<SecurityLoginsRolePoco, bool>> where, params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginsRolePoco GetSingle(Expression<Func<SecurityLoginsRolePoco, bool>> where, params Expression<Func<SecurityLoginsRolePoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginsRolePoco> loginsRolePoco = GetAll().AsQueryable();

            return loginsRolePoco.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginsRolePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (SecurityLoginsRolePoco loginsRolePoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[Security_Logins_Roles] WHERE [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", loginsRolePoco.Id);

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
                Debug.WriteLine(ex.Message, "Remove - SecurityLoginsRolePoco");
            }
        }

        public void Update(params SecurityLoginsRolePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var loginsRolePoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Security_Logins_Roles]
                                                       SET [Login] = @Login
                                                     ,[Role] = @Role                                            
                                                    WHERE [Id]= @Id";

                            command.Parameters.AddWithValue("@Id", loginsRolePoco.Id);
                            command.Parameters.AddWithValue("@Login", loginsRolePoco.Login);
                            command.Parameters.AddWithValue("@Role", loginsRolePoco.Role);


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
                Debug.WriteLine(ex.Message, "Update - SecurityLoginsRolePoco");
            }
        }
    }
}
