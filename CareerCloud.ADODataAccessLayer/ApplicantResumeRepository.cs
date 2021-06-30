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
    public class ApplicantResumeRepository : IDataRepository<ApplicantResumePoco>
    {
        protected readonly string _connStr = string.Empty;

        public ApplicantResumeRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantResumePoco[] items)
        {
            try
            {
                foreach (ApplicantResumePoco resumepoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Applicant_Resumes]
                                                                       ([Id]
                                                                       ,[Applicant]
                                                                       ,[Resume]
                                                                       ,[Last_Updated])
                                                                 VALUES
                                                                       (@Id 
                                                                       ,@Applicant
                                                                       ,@Resume
                                                                       ,@Last_Updated)", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", resumepoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Applicant", resumepoco.Applicant);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Resume", resumepoco.Resume);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Last_Updated", resumepoco.LastUpdated);


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
                Debug.WriteLine(ex.Message, "Add-Resume");
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<ApplicantResumePoco> GetAll(params Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            IList<ApplicantResumePoco> pocoResult = new List<ApplicantResumePoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                          ,[Applicant]
                                          ,[Resume]
                                          ,[Last_Updated]
                                      FROM [dbo].[Applicant_Resumes]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ApplicantResumePoco resumepoco = new ApplicantResumePoco();

                        resumepoco.Id = reader.GetGuid(0);
                        resumepoco.Applicant = reader.GetGuid(1);
                        resumepoco.Resume = reader.GetString(2);
                        resumepoco.LastUpdated = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3);

                        pocoResult.Add(resumepoco);
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

        public IList<ApplicantResumePoco> GetList(Expression<Func<ApplicantResumePoco, bool>> where, params Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantResumePoco GetSingle(Expression<Func<ApplicantResumePoco, bool>> where, params Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantResumePoco> resumepoco = GetAll().AsQueryable();
            return resumepoco.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantResumePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (ApplicantResumePoco resumepoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[Applicant_Resumes] WHERE  [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", resumepoco.Id);

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

        public void Update(params ApplicantResumePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var resumepoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Applicant_Resumes]
                                                   SET [Applicant]=@Applicant
                                                      ,[Resume]=@Resume 
                                                      ,[Last_Updated]=@Last_Updated
                                                       WHERE  [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", resumepoco.Id);
                            command.Parameters.AddWithValue("@Applicant", resumepoco.Applicant);
                            command.Parameters.AddWithValue("@Resume", resumepoco.Resume);
                            command.Parameters.AddWithValue("@Last_Updated", resumepoco.LastUpdated);

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

                Debug.WriteLine(ex.Message, "Update-Resume");
            }
        }
    }
}