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
    public class ApplicantWorkHistoryRepository : IDataRepository<ApplicantWorkHistoryPoco>
    {
        protected readonly string _connStr = string.Empty;

        public ApplicantWorkHistoryRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantWorkHistoryPoco[] items)
        {
            try
            {
                foreach (ApplicantWorkHistoryPoco workhistoryPoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Applicant_Work_History]
                                                                       ([Id]
                                                                       ,[Applicant]
                                                                       ,[Company_Name]
                                                                       ,[Country_Code]
                                                                       ,[Location]
                                                                       ,[Job_Title]
                                                                       ,[Job_Description]
                                                                       ,[Start_Month]
                                                                       ,[Start_Year]
                                                                       ,[End_Month]
                                                                       ,[End_Year])
                                                                 VALUES
                                                                       (@Id
                                                                       ,@Applicant
                                                                       ,@Company_Name
                                                                       ,@Country_Code
                                                                       ,@Location
                                                                       ,@Job_Title
                                                                       ,@Job_Description
                                                                       ,@Start_Month
                                                                       ,@Start_Year
                                                                       ,@End_Month
                                                                       ,@End_Year)", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", workhistoryPoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Applicant", workhistoryPoco.Applicant);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Company_Name", workhistoryPoco.CompanyName);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Country_Code", workhistoryPoco.CountryCode);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Location", workhistoryPoco.Location);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Job_Title", workhistoryPoco.JobTitle);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Job_Description", workhistoryPoco.JobDescription);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Start_Month", workhistoryPoco.StartMonth);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Start_Year", workhistoryPoco.StartYear);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@End_Month", workhistoryPoco.EndMonth);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@End_Year", workhistoryPoco.EndYear);


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

        public IList<ApplicantWorkHistoryPoco> GetAll(params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            IList<ApplicantWorkHistoryPoco> pocoResult = new List<ApplicantWorkHistoryPoco>();

            using (SqlConnection con = new SqlConnection(_connStr))
            {

                SqlCommand command = new SqlCommand();
                command.Connection = con;
                command.CommandText = @"SELECT [Id]
                                      ,[Applicant]
                                      ,[Company_Name]
                                      ,[Country_Code]
                                      ,[Location]
                                      ,[Job_Title]
                                      ,[Job_Description]
                                      ,[Start_Month]
                                      ,[Start_Year]
                                      ,[End_Month]
                                      ,[End_Year]
                                      ,[Time_Stamp]
                                  FROM [dbo].[Applicant_Work_History]";

                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ApplicantWorkHistoryPoco workhistoryPoco = new ApplicantWorkHistoryPoco();
                    workhistoryPoco.Id = reader.GetGuid(0);
                    workhistoryPoco.Applicant = reader.GetGuid(1);
                    workhistoryPoco.CompanyName = reader.GetString(2);
                    workhistoryPoco.CountryCode = reader.GetString(3);
                    workhistoryPoco.Location = reader.GetString(4);
                    workhistoryPoco.JobTitle = reader.GetString(5);
                    workhistoryPoco.JobDescription = reader.GetString(6);
                    workhistoryPoco.StartMonth = reader.GetInt16(7);
                    workhistoryPoco.StartYear = reader.GetInt32(8);
                    workhistoryPoco.EndMonth = reader.GetInt16(9);
                    workhistoryPoco.EndYear = reader.GetInt32(10);
                    workhistoryPoco.TimeStamp = (byte[])reader[11];

                    pocoResult.Add(workhistoryPoco);
                }

                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

                return pocoResult.Where(a => a != null).ToList();
            }
        }

        public IList<ApplicantWorkHistoryPoco> GetList(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantWorkHistoryPoco GetSingle(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantWorkHistoryPoco> workhistoryPoco = GetAll().AsQueryable();
            return workhistoryPoco.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantWorkHistoryPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (ApplicantWorkHistoryPoco workhistoryPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM[dbo].[Applicant_Work_History] WHERE  [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", workhistoryPoco.Id);

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

        public void Update(params ApplicantWorkHistoryPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var workhistoryPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Applicant_Work_History]
                                                   SET 
                                                      [Applicant] = @Applicant
                                                      ,[Company_Name] = @Company_Name
                                                      ,[Country_Code] = @Country_Code
                                                      ,[Location] = @Location
                                                      ,[Job_Title] = @Job_Title
                                                      ,[Job_Description] = @Job_Description
                                                      ,[Start_Month] = @Start_Month
                                                      ,[Start_Year] = @Start_Year
                                                      ,[End_Month] = @End_Month
                                                      ,[End_Year] = @End_Year
                                                       WHERE  [Id]= @Id";

                            command.Parameters.AddWithValue("@Id", workhistoryPoco.Id);
                            command.Parameters.AddWithValue("@Applicant", workhistoryPoco.Applicant);
                            command.Parameters.AddWithValue("@Company_Name", workhistoryPoco.CompanyName);
                            command.Parameters.AddWithValue("@Country_Code", workhistoryPoco.CountryCode);
                            command.Parameters.AddWithValue("@Location", workhistoryPoco.Location);
                            command.Parameters.AddWithValue("@Job_Title", workhistoryPoco.JobTitle);
                            command.Parameters.AddWithValue("@Job_Description ", workhistoryPoco.JobDescription);
                            command.Parameters.AddWithValue("@Start_Month", workhistoryPoco.StartMonth);
                            command.Parameters.AddWithValue("@Start_Year", workhistoryPoco.StartYear);
                            command.Parameters.AddWithValue("@End_Month", workhistoryPoco.EndMonth);
                            command.Parameters.AddWithValue("@End_Year", workhistoryPoco.EndYear);



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


