using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CareerCloud.ADODataAccessLayer
{
    public class ApplicantJobApplicationRepository : IDataRepository<ApplicantJobApplicationPoco>
    {
        protected readonly string _connStr = string.Empty;

        public ApplicantJobApplicationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantJobApplicationPoco[] items)
        {
            using (SqlConnection con = new SqlConnection(_connStr))
            {
                foreach (ApplicantJobApplicationPoco poco in items)
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"INSERT INTO[dbo].[Applicant_Job_Applications]
                                               ([Id]
                                               ,[Applicant]
                                               ,[Job]
                                               ,[Application_Date])
                                         VALUES
                                               (@Id
                                               ,@Applicant
                                               ,@Job
                                               ,@Application_Date)";


                    command.Parameters.AddWithValue("@Id", poco.Id);
                    command.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    command.Parameters.AddWithValue("@Job", poco.Job);
                    command.Parameters.AddWithValue("@Application_Date", poco.ApplicationDate);


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
        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<ApplicantJobApplicationPoco> GetAll(params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
            IList<ApplicantJobApplicationPoco> pocoResult = new List<ApplicantJobApplicationPoco>();

            using (SqlConnection con = new SqlConnection(_connStr))
            {

                SqlCommand command = new SqlCommand();
                command.Connection = con;
                command.CommandText = @"SELECT [Id]
                                  ,[Applicant]
                                  ,[Job]
                                  ,[Application_Date]
                                  ,[Time_Stamp]
                              FROM [dbo].[Applicant_Job_Applications]";

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    ApplicantJobApplicationPoco poco = new ApplicantJobApplicationPoco();
                    poco.Id = reader.GetGuid(0);
                    poco.Applicant = reader.GetGuid(1);
                    poco.Job = reader.GetGuid(2);
                    poco.ApplicationDate = reader.GetDateTime(3);
                    poco.TimeStamp = (byte[])reader[4];

                    pocoResult.Add(poco);


                }

                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
                
                return pocoResult.Where(a => a != null).ToList();
            }
        }

        public IList<ApplicantJobApplicationPoco> GetList(Expression<Func<ApplicantJobApplicationPoco, bool>> where, params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantJobApplicationPoco GetSingle(Expression<Func<ApplicantJobApplicationPoco, bool>> where, params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantJobApplicationPoco> jobApplicationPoco = GetAll().AsQueryable();
            return jobApplicationPoco.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantJobApplicationPoco[] items)
        {
            using (SqlConnection con = new SqlConnection(_connStr))
            {
                foreach (ApplicantJobApplicationPoco poco in items)
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"DELETE FROM[dbo].[Applicant_Job_Applications]
                                   WHERE  [Id]= @Id";
                    command.Parameters.AddWithValue("@Id", poco.Id);

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

        public void Update(params ApplicantJobApplicationPoco[] items)
        {
            using (SqlConnection con = new SqlConnection(_connStr))
            {
                foreach (var poco in items)
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"UPDATE [dbo].[Applicant_Job_Applications]
                                       SET 
                                           [Applicant] = @Applicant
                                          ,[Job] = @Job
                                          ,[Application_Date] = @Application_Date
                                          WHERE  [Id]= @Id";

                    command.Parameters.AddWithValue("@Id", poco.Id);
                    command.Parameters.AddWithValue("@Applicant", poco.Applicant);
                    command.Parameters.AddWithValue("@Job", poco.Job);
                    command.Parameters.AddWithValue("@Application_Date", poco.ApplicationDate);

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
}

