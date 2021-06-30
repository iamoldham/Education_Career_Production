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
    public class SystemLanguageCodeRepository : IDataRepository<SystemLanguageCodePoco>
    {
        protected readonly string _connStr = string.Empty;

        public SystemLanguageCodeRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params SystemLanguageCodePoco[] items)
        {
            try
            {
                foreach (SystemLanguageCodePoco languageCodePoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT [dbo].[System_Language_Codes]
                                                                        ([LanguageID]
                                                                        ,[Name]
                                                                        ,[Native_Name])
                                                                        VALUES(
                                                                          @Language_Id
                                                                         ,@Name
                                                                         ,@Native_Name
                                                                        )", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Language_Id", languageCodePoco.LanguageID);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Name", languageCodePoco.Name);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Native_Name", languageCodePoco.NativeName);
                            

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
                Debug.WriteLine(ex.Message, "Add - SystemLanguageCodePoco");
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SystemLanguageCodePoco> GetAll(params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            IList<SystemLanguageCodePoco> pocoResult = new List<SystemLanguageCodePoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [LanguageID]
                                            ,[Name]
                                            ,[Native_Name]
                                            FROM [dbo].[System_Language_Codes]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SystemLanguageCodePoco languageCodePoco = new SystemLanguageCodePoco();

                        languageCodePoco.LanguageID = reader.GetString(0);
                        languageCodePoco.Name = reader.GetString(1);
                        languageCodePoco.NativeName = reader.GetString(2);
                        
                        pocoResult.Add(languageCodePoco);
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
                Debug.WriteLine(ex.Message, "GetAll - SystemLanguageCodePoco");
            }

            return pocoResult.Where(a => a != null).ToList();
        }

        public IList<SystemLanguageCodePoco> GetList(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SystemLanguageCodePoco GetSingle(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            IQueryable<SystemLanguageCodePoco> languageCodePoco = GetAll().AsQueryable();

            return languageCodePoco.Where(where).FirstOrDefault();
        }

        public void Remove(params SystemLanguageCodePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (SystemLanguageCodePoco languageCodePoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[System_Language_Codes] WHERE [LanguageID]= @Language_Id";
                            command.Parameters.AddWithValue("@Language_Id", languageCodePoco.LanguageID);

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
                Debug.WriteLine(ex.Message, "Remove - SystemLanguageCodePoco");
            }
        }

        public void Update(params SystemLanguageCodePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var languageCodePoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[System_Language_Codes]
                                                    SET 
                                                    [Name] = @Name
                                                    ,[Native_Name] = @Native_Name                                             
                                                    WHERE [LanguageID]= @LanguageID";

                            command.Parameters.AddWithValue("@LanguageID", languageCodePoco.LanguageID);
                            command.Parameters.AddWithValue("@Name", languageCodePoco.Name);
                            command.Parameters.AddWithValue("@Native_Name", languageCodePoco.NativeName);
                           

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
                Debug.WriteLine(ex.Message, "Update - SystemLanguageCodePoco");
            }
        }
    }
}
