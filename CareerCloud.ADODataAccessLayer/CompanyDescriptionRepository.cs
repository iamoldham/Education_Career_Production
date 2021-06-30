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


namespace CareerCloud.ADODataAccessLayer
{
    public class CompanyDescriptionRepository : IDataRepository<CompanyDescriptionPoco>
    {
        protected readonly string _connStr = string.Empty;

        public CompanyDescriptionRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyDescriptionPoco[] items)
        {
            try
            {
                foreach (CompanyDescriptionPoco companydescPoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Company_Descriptions]
                                                                       ([Id]
                                                                       ,[Company]
                                                                       ,[LanguageID]
                                                                       ,[Company_Name]
                                                                       ,[Company_Description])
                                                                 VALUES
                                                                       (@Id 
                                                                       ,@Company
                                                                       ,@LanguageID
                                                                       ,@Company_Name
                                                                       ,@Company_Description
                                                                         )", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", companydescPoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Company", companydescPoco.Company);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@LanguageID", companydescPoco.LanguageId);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Company_Name", companydescPoco.CompanyName);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Company_Description", companydescPoco.CompanyDescription);


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

                Console.WriteLine(ex.Message);
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<CompanyDescriptionPoco> GetAll(params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            IList<CompanyDescriptionPoco> pocoResult = new List<CompanyDescriptionPoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                              ,[Company]
                                              ,[LanguageID]
                                              ,[Company_Name]
                                              ,[Company_Description]
                                              ,[Time_Stamp]
                                          FROM [dbo].[Company_Descriptions]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CompanyDescriptionPoco companydescPoco = new CompanyDescriptionPoco();

                        companydescPoco.Id = reader.GetGuid(0);
                        companydescPoco.Company = reader.GetGuid(1);
                        companydescPoco.LanguageId = reader.GetString(2);
                        companydescPoco.CompanyName = reader.GetString(3);
                        companydescPoco.CompanyDescription = reader.GetString(4);
                        companydescPoco.TimeStamp = (byte[])reader[5];

                        pocoResult.Add(companydescPoco);
                    }

                    reader.Close();

                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            catch (Exception)
            {


            }

            return pocoResult.Where(a => a != null).ToList();
        }

        public IList<CompanyDescriptionPoco> GetList(Expression<Func<CompanyDescriptionPoco, bool>> where, params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyDescriptionPoco GetSingle(Expression<Func<CompanyDescriptionPoco, bool>> where, params Expression<Func<CompanyDescriptionPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyDescriptionPoco> companydescPoco = GetAll().AsQueryable();
            return companydescPoco.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyDescriptionPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (CompanyDescriptionPoco companydescPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM[dbo].[Company_Descriptions] WHERE  [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", companydescPoco.Id);

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
            catch (Exception)
            {

                throw;
            }

        }

        public void Update(params CompanyDescriptionPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var companydescPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Company_Descriptions]
                                                      SET 
                                                       [Company] = @Company
                                                      ,[LanguageID] = @LanguageID
                                                      ,[Company_Name] = @Company_Name
                                                      ,[Company_Description] = @Company_Description
                                                        WHERE  [Id]= @Id";

                            command.Parameters.AddWithValue("@Id", companydescPoco.Id);
                            command.Parameters.AddWithValue("@Company", companydescPoco.Company);
                            command.Parameters.AddWithValue("@LanguageID", companydescPoco.LanguageId);
                            command.Parameters.AddWithValue("@Company_Name", companydescPoco.CompanyName);
                            command.Parameters.AddWithValue("@Company_Description", companydescPoco.CompanyDescription);


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
            catch (Exception)
            {

                throw;
            }
        }
    }
}
