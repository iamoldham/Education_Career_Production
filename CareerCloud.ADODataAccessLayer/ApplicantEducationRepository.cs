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
    public class ApplicantEducationRepository : IDataRepository<ApplicantEducationPoco>
    {
        protected readonly string _connStr = string.Empty;

        public ApplicantEducationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantEducationPoco[] items)
        {
            try
            {
                foreach (ApplicantEducationPoco educationPoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO[dbo].[Applicant_Educations]
                                   ([Id]
                                   ,[Applicant]
                                   ,[Major]
                                   ,[Certificate_Diploma]
                                   ,[Start_Date]
                                   ,[Completion_Date]
                                   ,[Completion_Percent])
                             VALUES
                                 (
                                    @Id
                                   ,@Applicant
                                   ,@Major
                                   ,@Certificate_Diploma
                                   ,@Start_Date
                                   ,@Completion_Date
                                   ,@Completion_Percent 
                                  )", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", educationPoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Applicant", educationPoco.Applicant);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Major", educationPoco.Major);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Certificate_Diploma", educationPoco.CertificateDiploma);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Start_Date", educationPoco.StartDate);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Completion_Date", educationPoco.CompletionDate);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Completion_Percent", educationPoco.CompletionPercent);

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

                
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<ApplicantEducationPoco> GetAll(params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            IList<ApplicantEducationPoco> pocoResult=new List<ApplicantEducationPoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                      ,[Applicant]
                                      ,[Major]
                                      ,[Certificate_Diploma]
                                      ,[Start_Date]
                                      ,[Completion_Date]
                                      ,[Completion_Percent]
                                      ,[Time_Stamp]
                                  FROM [dbo].[Applicant_Educations]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ApplicantEducationPoco educationPoco = new ApplicantEducationPoco();

                        educationPoco.Id = reader.GetGuid(0);
                        educationPoco.Applicant = reader.GetGuid(1);
                        educationPoco.Major = reader.GetString(2);
                        educationPoco.CertificateDiploma = reader.GetString(3);
                        educationPoco.StartDate = reader.GetDateTime(4);
                        educationPoco.CompletionDate = reader.GetDateTime(5);
                        educationPoco.CompletionPercent = reader.GetByte(6);
                        educationPoco.TimeStamp = (byte[])reader[7];

                        pocoResult.Add(educationPoco);
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
            return pocoResult.ToList();
        }

        public IList<ApplicantEducationPoco> GetList(Expression<Func<ApplicantEducationPoco, bool>> where, params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantEducationPoco GetSingle(Expression<Func<ApplicantEducationPoco, bool>> where, params Expression<Func<ApplicantEducationPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantEducationPoco> educationPoco = GetAll().AsQueryable();

            return educationPoco.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantEducationPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (ApplicantEducationPoco educationPoco in items)
                    {
                        using (SqlCommand command=new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM[dbo].[Applicant_Educations] WHERE  [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", educationPoco.Id);

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

        public void Update(params ApplicantEducationPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var educationPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Applicant_Educations]
                                           SET 
                                               [Applicant] = @Applicant
                                              ,[Major] = @Major
                                              ,[Certificate_Diploma] = @Certificate_Diploma
                                              ,[Start_Date] = @Start_Date
                                              ,[Completion_Date] = @Completion_Date
                                              ,[Completion_Percent] = @Completion_Percent
                                         WHERE  [Id]= @Id";

                            command.Parameters.AddWithValue("@Id", educationPoco.Id);
                            command.Parameters.AddWithValue("@Applicant", educationPoco.Applicant);
                            command.Parameters.AddWithValue("@Major", educationPoco.Major);
                            command.Parameters.AddWithValue("@Certificate_Diploma", educationPoco.CertificateDiploma);
                            command.Parameters.AddWithValue("@Start_Date", educationPoco.StartDate);
                            command.Parameters.AddWithValue("@Completion_Date", educationPoco.CompletionDate);
                            command.Parameters.AddWithValue("@Completion_Percent ", educationPoco.CompletionPercent);

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
