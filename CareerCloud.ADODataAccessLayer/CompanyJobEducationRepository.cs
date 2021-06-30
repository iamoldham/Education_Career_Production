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
    public class CompanyJobEducationRepository : IDataRepository<CompanyJobEducationPoco>
    {

        protected readonly string _connStr = string.Empty;

        public CompanyJobEducationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyJobEducationPoco[] items)
        {
            try
            {
                foreach (CompanyJobEducationPoco companyjobedupoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Company_Job_Educations]
                                                                       ([Id]
                                                                       ,[Job]
                                                                       ,[Major]
                                                                       ,[Importance])
                                                                 VALUES
                                                                       (@Id
                                                                       ,@Job
                                                                       ,@Major
                                                                       ,@Importance)", con);
                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", companyjobedupoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Job", companyjobedupoco.Job);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Major", companyjobedupoco.Major);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Importance", companyjobedupoco.Importance);

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

        public IList<CompanyJobEducationPoco> GetAll(params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            IList<CompanyJobEducationPoco> pocoResult = new List<CompanyJobEducationPoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                          ,[Job]
                                          ,[Major]
                                          ,[Importance]
                                          ,[Time_Stamp]
                                      FROM [dbo].[Company_Job_Educations]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CompanyJobEducationPoco companyjobedupoco = new CompanyJobEducationPoco();

                        companyjobedupoco.Id = reader.GetGuid(0);
                        companyjobedupoco.Job = reader.GetGuid(1);
                        companyjobedupoco.Major = reader.GetString(2);
                        companyjobedupoco.Importance = reader.GetInt16(3);
                        companyjobedupoco.TimeStamp = (byte[])reader[4];

                        pocoResult.Add(companyjobedupoco);
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

        public IList<CompanyJobEducationPoco> GetList(Expression<Func<CompanyJobEducationPoco, bool>> where, params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobEducationPoco GetSingle(Expression<Func<CompanyJobEducationPoco, bool>> where, params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobEducationPoco> companyjobedupoco = GetAll().AsQueryable();
            return companyjobedupoco.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobEducationPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (CompanyJobEducationPoco companyjobedupoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[Company_Job_Educations] WHERE [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", companyjobedupoco.Id);

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

        public void Update(params CompanyJobEducationPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var companyjobedupoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Company_Job_Educations]
                                                   SET 
                                                      [Job] = @Job
                                                      ,[Major] = @Major
                                                      ,[Importance] = @Importance
                                                          WHERE [Id]=@Id";

                            command.Parameters.AddWithValue("@Id", companyjobedupoco.Id);
                            command.Parameters.AddWithValue("@Job", companyjobedupoco.Job);
                            command.Parameters.AddWithValue("@Major", companyjobedupoco.Major);
                            command.Parameters.AddWithValue("@Importance", companyjobedupoco.Importance);


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




