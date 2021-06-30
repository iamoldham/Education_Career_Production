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
    public class ApplicantSkillRepository : IDataRepository<ApplicantSkillPoco>
    {
        protected readonly string _connStr = string.Empty;

        public ApplicantSkillRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantSkillPoco[] items)
        {
            try
            {
                foreach (ApplicantSkillPoco skillPoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Applicant_Skills]
                                                                       ([Id]
                                                                       ,[Applicant]
                                                                       ,[Skill]
                                                                       ,[Skill_Level]
                                                                       ,[Start_Month]
                                                                       ,[Start_Year]
                                                                       ,[End_Month]
                                                                       ,[End_Year])
                                                                 VALUES
                                                                       (@Id
                                                                       ,@Applicant
                                                                       ,@Skill
                                                                       ,@Skill_Level
                                                                       ,@Start_Month
                                                                       ,@Start_Year
                                                                       ,@End_Month
                                                                       ,@End_Year)", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", skillPoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Applicant", skillPoco.Applicant);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Skill", skillPoco.Skill);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Skill_Level", skillPoco.SkillLevel);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Start_Month", skillPoco.StartMonth);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Start_Year", skillPoco.StartYear);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@End_Month", skillPoco.EndMonth);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@End_Year", skillPoco.EndYear);

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

        public IList<ApplicantSkillPoco> GetAll(params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            IList<ApplicantSkillPoco> pocoResult = new List<ApplicantSkillPoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                          ,[Applicant]
                                          ,[Skill]
                                          ,[Skill_Level]
                                          ,[Start_Month]
                                          ,[Start_Year]
                                          ,[End_Month]
                                          ,[End_Year]
                                          ,[Time_Stamp]
                                      FROM [dbo].[Applicant_Skills]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ApplicantSkillPoco skillPoco = new ApplicantSkillPoco();

                        skillPoco.Id = reader.GetGuid(0);
                        skillPoco.Applicant = reader.GetGuid(1);
                        skillPoco.Skill = reader.GetString(2);
                        skillPoco.SkillLevel = reader.GetString(3);
                        skillPoco.StartMonth = (byte)reader[4];
                        skillPoco.StartYear = reader.GetInt32(5);
                        skillPoco.EndMonth = (byte)reader[6];
                        skillPoco.EndYear = reader.GetInt32(7);
                        skillPoco.TimeStamp = (byte[])reader[8];

                        pocoResult.Add(skillPoco);
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

        public IList<ApplicantSkillPoco> GetList(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantSkillPoco GetSingle(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantSkillPoco> skillPoco = GetAll().AsQueryable();
            return skillPoco.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantSkillPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (ApplicantSkillPoco skillPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM[dbo].[Applicant_Skills] WHERE  [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", skillPoco.Id);

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

        public void Update(params ApplicantSkillPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var skillPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Applicant_Skills]
                                                   SET 
                                                       [Applicant] = @Applicant
                                                      ,[Skill] = @Skill
                                                      ,[Skill_Level] = @Skill_Level
                                                      ,[Start_Month] = @Start_Month
                                                      ,[Start_Year] = @Start_Year
                                                      ,[End_Month] = @End_Month
                                                      ,[End_Year] = @End_Year
                                                       WHERE  [Id]= @Id";

                            command.Parameters.AddWithValue("@Id", skillPoco.Id);
                            command.Parameters.AddWithValue("@Applicant", skillPoco.Applicant);
                            command.Parameters.AddWithValue("@Skill", skillPoco.Skill);
                            command.Parameters.AddWithValue("@Skill_Level", skillPoco.SkillLevel);
                            command.Parameters.AddWithValue("@Start_Month", skillPoco.StartMonth);
                            command.Parameters.AddWithValue("@Start_Year", skillPoco.StartYear);
                            command.Parameters.AddWithValue("@End_Month", skillPoco.EndMonth);
                            command.Parameters.AddWithValue("@End_Year", skillPoco.EndYear);


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
