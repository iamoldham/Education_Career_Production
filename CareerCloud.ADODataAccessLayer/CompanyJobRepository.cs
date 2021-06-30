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
    public class CompanyJobRepository : IDataRepository<CompanyJobPoco>
    {

        protected readonly string _connStr = string.Empty;

        public CompanyJobRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyJobPoco[] items)
        {
            try
            {
                foreach (CompanyJobPoco companyjobpoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Company_Jobs]
                                                                       ([Id]
                                                                       ,[Company]
                                                                       ,[Profile_Created]
                                                                       ,[Is_Inactive]
                                                                       ,[Is_Company_Hidden])
                                                                        VALUES
                                                                       (@Id
                                                                       ,@Company
                                                                       ,@Profile_Created
                                                                       ,@Is_Inactive
                                                                       ,@Is_Company_Hidden)", con);
                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", companyjobpoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Company", companyjobpoco.Company);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Profile_Created", companyjobpoco.ProfileCreated);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Is_Inactive", companyjobpoco.IsInactive);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Is_Company_Hidden", companyjobpoco.IsCompanyHidden);

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

        public IList<CompanyJobPoco> GetAll(params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            IList<CompanyJobPoco> pocoResult = new List<CompanyJobPoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                          ,[Company]
                                          ,[Profile_Created]
                                          ,[Is_Inactive]
                                          ,[Is_Company_Hidden]
                                          ,[Time_Stamp]
                                      FROM [dbo].[Company_Jobs]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CompanyJobPoco companyjobpoco = new CompanyJobPoco();

                        companyjobpoco.Id = reader.GetGuid(0);
                        companyjobpoco.Company = reader.GetGuid(1);
                        companyjobpoco.ProfileCreated = reader.GetDateTime(2);
                        companyjobpoco.IsInactive = reader.GetBoolean(3);
                        companyjobpoco.IsCompanyHidden = reader.GetBoolean(4);
                        companyjobpoco.TimeStamp = reader.IsDBNull(5) ? (byte[]?)null : (byte[])reader[5];

                        pocoResult.Add(companyjobpoco);
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


            }
            return pocoResult.Where(a => a != null).ToList();
        }

        public IList<CompanyJobPoco> GetList(Expression<Func<CompanyJobPoco, bool>> where, params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobPoco GetSingle(Expression<Func<CompanyJobPoco, bool>> where, params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobPoco> companyjobpoco = GetAll().AsQueryable();
            return companyjobpoco.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (CompanyJobPoco companyjobpoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[Company_Jobs] WHERE [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", companyjobpoco.Id);

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


            }
        }

        public void Update(params CompanyJobPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var companyjobpoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Company_Jobs]
                                                       SET 
                                                          [Company] = @Company
                                                          ,[Profile_Created] = @Profile_Created
                                                          ,[Is_Inactive] = @Is_Inactive
                                                          ,[Is_Company_Hidden] = @Is_Company_Hidden
                                                            WHERE [Id]=@Id";

                            command.Parameters.AddWithValue("@Id", companyjobpoco.Id);
                            command.Parameters.AddWithValue("@Company", companyjobpoco.Company);
                            command.Parameters.AddWithValue("@Profile_Created", companyjobpoco.ProfileCreated);
                            command.Parameters.AddWithValue("@Is_Inactive", companyjobpoco.IsInactive);
                            command.Parameters.AddWithValue("@Is_Company_Hidden", companyjobpoco.IsCompanyHidden);


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

                Console.WriteLine(ex.Message);
            }
        }
    }
}




