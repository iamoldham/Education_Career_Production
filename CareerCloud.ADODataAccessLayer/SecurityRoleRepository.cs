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
    public class SecurityRoleRepository : IDataRepository<SecurityRolePoco>
    {
        protected readonly string _connStr = string.Empty;

        public SecurityRoleRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params SecurityRolePoco[] items)
        {
            try
            {
                foreach (SecurityRolePoco rolePoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Security_Roles]
                                                                        ([Id]
                                                                        ,[Role]
                                                                        ,[Is_Inactive])
                                                                        VALUES(
                                                                          @Id
                                                                         ,@Role
                                                                         ,@Is_Inactive
                                                                        )", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", rolePoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Role", rolePoco.Role);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Is_Inactive", rolePoco.IsInactive);


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
                Debug.WriteLine(ex.Message, "Add - SecurityRolePoco");
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityRolePoco> GetAll(params Expression<Func<SecurityRolePoco, object>>[] navigationProperties)
        {
            IList<SecurityRolePoco> pocoResult = new List<SecurityRolePoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                        ,[Role]
                                        ,[Is_Inactive]
                                         FROM [dbo].[Security_Roles]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SecurityRolePoco rolePoco = new SecurityRolePoco();

                        rolePoco.Id = reader.GetGuid(0);
                        rolePoco.Role = reader.GetString(1);
                        rolePoco.IsInactive = reader.GetBoolean(2);

                        pocoResult.Add(rolePoco);
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
                Debug.WriteLine(ex.Message, "GetAll - SecurityRolePoco");
            }

            return pocoResult.Where(a => a != null).ToList();
        }

        public IList<SecurityRolePoco> GetList(Expression<Func<SecurityRolePoco, bool>> where, params Expression<Func<SecurityRolePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityRolePoco GetSingle(Expression<Func<SecurityRolePoco, bool>> where, params Expression<Func<SecurityRolePoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityRolePoco> rolePoco = GetAll().AsQueryable();

            return rolePoco.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityRolePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (SecurityRolePoco rolePoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[Security_Roles] WHERE [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", rolePoco.Id);

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
                Debug.WriteLine(ex.Message, "Remove - SecurityRolePoco");
            }
        }

        public void Update(params SecurityRolePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var rolePoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Security_Roles]
                                                    SET 
                                                    [Role] = @Role
                                                    ,[Is_Inactive] = @Is_Inactive                                            
                                                    WHERE [Id]= @Id";

                            command.Parameters.AddWithValue("@Id", rolePoco.Id);
                            command.Parameters.AddWithValue("@Role", rolePoco.Role);
                            command.Parameters.AddWithValue("@Is_Inactive", rolePoco.IsInactive);


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
                Debug.WriteLine(ex.Message, "Update - SecurityRolePoco");
            }
        }
    }
}
