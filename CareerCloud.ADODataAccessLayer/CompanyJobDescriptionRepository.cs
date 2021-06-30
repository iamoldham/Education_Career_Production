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
    public class CompanyJobDescriptionRepository : IDataRepository<CompanyJobDescriptionPoco>
    {

        protected readonly string _connStr = string.Empty;

        public CompanyJobDescriptionRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyJobDescriptionPoco[] items)
        {
            try
            {
                foreach (CompanyJobDescriptionPoco companyjobdescpoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Company_Jobs_Descriptions]
                                                                       ([Id]
                                                                       ,[Job]
                                                                       ,[Job_Name]
                                                                       ,[Job_Descriptions])
                                                                 VALUES
                                                                       (@Id
                                                                       ,@Job
                                                                       ,@Job_Name
                                                                       ,@Job_Descriptions)", con);
                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", companyjobdescpoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Job", companyjobdescpoco.Job);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Job_Name", companyjobdescpoco.JobName);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Job_Descriptions", companyjobdescpoco.JobDescriptions);

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

        public IList<CompanyJobDescriptionPoco> GetAll(params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            IList<CompanyJobDescriptionPoco> pocoResult = new List<CompanyJobDescriptionPoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                          ,[Job]
                                          ,[Job_Name]
                                          ,[Job_Descriptions]
                                          ,[Time_Stamp]
                                      FROM [dbo].[Company_Jobs_Descriptions]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CompanyJobDescriptionPoco companyjobdescpoco = new CompanyJobDescriptionPoco();

                        companyjobdescpoco.Id = reader.GetGuid(0);
                        companyjobdescpoco.Job = reader.GetGuid(1);
                        companyjobdescpoco.JobName = reader.IsDBNull(2) ? (String?)null : reader.GetString(2);
                        companyjobdescpoco.JobDescriptions = reader.IsDBNull(3) ? (String?)null : reader.GetString(3);
                        companyjobdescpoco.TimeStamp = reader.IsDBNull(4) ? (byte[]?)null : (byte[])reader[4];

                        pocoResult.Add(companyjobdescpoco);
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

        public IList<CompanyJobDescriptionPoco> GetList(Expression<Func<CompanyJobDescriptionPoco, bool>> where, params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobDescriptionPoco GetSingle(Expression<Func<CompanyJobDescriptionPoco, bool>> where, params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobDescriptionPoco> companyjobdescpoco = GetAll().AsQueryable();
            return companyjobdescpoco.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobDescriptionPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (CompanyJobDescriptionPoco companyjobdescpoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[Company_Jobs_Descriptions] WHERE [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", companyjobdescpoco.Id);

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

        public void Update(params CompanyJobDescriptionPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var companyjobdescpoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Company_Jobs_Descriptions]
                                                       SET 
                                                          [Job] = @Job
                                                          ,[Job_Name] = @Job_Name
                                                          ,[Job_Descriptions] = @Job_Descriptions
                                                          WHERE [Id]=@Id";

                            command.Parameters.AddWithValue("@Id", companyjobdescpoco.Id);
                            command.Parameters.AddWithValue("@Job", companyjobdescpoco.Job);
                            command.Parameters.AddWithValue("@Job_Name", companyjobdescpoco.JobName);
                            command.Parameters.AddWithValue("@Job_Descriptions", companyjobdescpoco.JobDescriptions);


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




