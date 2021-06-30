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
    public class CompanyJobSkillRepository : IDataRepository<CompanyJobSkillPoco>
    {

        protected readonly string _connStr = string.Empty;

        public CompanyJobSkillRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyJobSkillPoco[] items)
        {
            try
            {
                foreach (CompanyJobSkillPoco companyjobskillpoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Company_Job_Skills]
                                                                       ([Id]
                                                                       ,[Job]
                                                                       ,[Skill]
                                                                       ,[Skill_Level]
                                                                       ,[Importance])
                                                                 VALUES
                                                                       (@Id
                                                                       ,@Job
                                                                       ,@Skill
                                                                       ,@Skill_Level
                                                                       ,@Importance)", con);
                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", companyjobskillpoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Job", companyjobskillpoco.Job);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Skill", companyjobskillpoco.Skill);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Skill_Level", companyjobskillpoco.SkillLevel);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Importance", companyjobskillpoco.Importance);

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

        public IList<CompanyJobSkillPoco> GetAll(params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {
            IList<CompanyJobSkillPoco> pocoResult = new List<CompanyJobSkillPoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                          ,[Job]
                                          ,[Skill]
                                          ,[Skill_Level]
                                          ,[Importance]
                                          ,[Time_Stamp]
                                      FROM [dbo].[Company_Job_Skills]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CompanyJobSkillPoco companyjobskillpoco = new CompanyJobSkillPoco();

                        companyjobskillpoco.Id = reader.GetGuid(0);
                        companyjobskillpoco.Job = reader.GetGuid(1);
                        companyjobskillpoco.Skill = reader.GetString(2);
                        companyjobskillpoco.SkillLevel = reader.GetString(3);
                        companyjobskillpoco.Importance = reader.GetInt32(4);
                        companyjobskillpoco.TimeStamp = (byte[])reader[5];

                        pocoResult.Add(companyjobskillpoco);
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

        public IList<CompanyJobSkillPoco> GetList(Expression<Func<CompanyJobSkillPoco, bool>> where, params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyJobSkillPoco GetSingle(Expression<Func<CompanyJobSkillPoco, bool>> where, params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyJobSkillPoco> companyjobskillpoco = GetAll().AsQueryable();
            return companyjobskillpoco.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobSkillPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (CompanyJobSkillPoco companyjobskillpoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[Company_Job_Skills] WHERE [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", companyjobskillpoco.Id);

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

        public void Update(params CompanyJobSkillPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var companyjobskillpoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Company_Job_Skills]
                                                   SET
                                                       [Job] = @Job
                                                      ,[Skill] = @Skill
                                                      ,[Skill_Level] = @Skill_Level
                                                      ,[Importance] = @Importance
                                                       WHERE [Id]=@Id";

                            command.Parameters.AddWithValue("@Id", companyjobskillpoco.Id);
                            command.Parameters.AddWithValue("@Job", companyjobskillpoco.Job);
                            command.Parameters.AddWithValue("@Skill", companyjobskillpoco.Skill);
                            command.Parameters.AddWithValue("@Skill_Level", companyjobskillpoco.SkillLevel);
                            command.Parameters.AddWithValue("@Importance", companyjobskillpoco.Importance);


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




