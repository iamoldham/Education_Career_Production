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
    public class ApplicantProfileRepository : IDataRepository<ApplicantProfilePoco>
    {

        protected readonly string _connStr = string.Empty;

        public ApplicantProfileRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantProfilePoco[] items)
        {
            try
            {
                foreach (ApplicantProfilePoco profilePoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Applicant_Profiles]
                                                                        ([Id]
                                                                        ,[Login]
                                                                        ,[Current_Salary]
                                                                        ,[Current_Rate]
                                                                        ,[Currency]
                                                                        ,[Country_Code]
                                                                        ,[State_Province_Code]
                                                                        ,[Street_Address]
                                                                        ,[City_Town]
                                                                        ,[Zip_Postal_Code])
                                                                        VALUES(                                                             
                                                                         @Id
                                                                        ,@Login
                                                                        ,@Current_Salary
                                                                        ,@Current_Rate
                                                                        ,@Currency
                                                                        ,@Country_Code
                                                                        ,@State_Province_Code
                                                                        ,@Street_Address
                                                                        ,@City_Town
                                                                        ,@Zip_Postal_Code)", con);
                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", profilePoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Login", profilePoco.Login);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Current_Salary", profilePoco.CurrentSalary);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Current_Rate", profilePoco.CurrentRate);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Currency", profilePoco.Currency);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Country_Code", profilePoco.Country);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@State_Province_Code", profilePoco.Province);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Street_Address", profilePoco.Street);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@City_Town", profilePoco.City);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Zip_Postal_Code", profilePoco.PostalCode);

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

        public IList<ApplicantProfilePoco> GetAll(params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            IList<ApplicantProfilePoco> pocoResult = new List<ApplicantProfilePoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                            ,[Login]
                                            ,[Current_Salary]
                                            ,[Current_Rate]
                                            ,[Currency]
                                            ,[Country_Code]
                                            ,[State_Province_Code]
                                            ,[Street_Address]
                                            ,[City_Town]
                                            ,[Zip_Postal_Code]
                                            ,[Time_Stamp]
                                            FROM [dbo].[Applicant_Profiles]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ApplicantProfilePoco profilePoco = new ApplicantProfilePoco();

                        profilePoco.Id = reader.GetGuid(0);
                        profilePoco.Login = reader.GetGuid(1);
                        profilePoco.CurrentSalary = reader.GetDecimal(2);
                        profilePoco.CurrentRate = reader.GetDecimal(3);
                        profilePoco.Currency = reader.GetString(4);
                        profilePoco.Country = reader.GetString(5);
                        profilePoco.Province = reader.GetString(6);
                        profilePoco.Street = reader.GetString(7);
                        profilePoco.City = reader.GetString(8);
                        profilePoco.PostalCode = reader.GetString(9);
                        profilePoco.TimeStamp = (byte[])reader[10];

                        pocoResult.Add(profilePoco);
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

        public IList<ApplicantProfilePoco> GetList(Expression<Func<ApplicantProfilePoco, bool>> where, params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public ApplicantProfilePoco GetSingle(Expression<Func<ApplicantProfilePoco, bool>> where, params Expression<Func<ApplicantProfilePoco, object>>[] navigationProperties)
        {
            IQueryable<ApplicantProfilePoco> profilePoco = GetAll().AsQueryable();
            return profilePoco.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantProfilePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (ApplicantProfilePoco profilePoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[Applicant_Profiles] WHERE [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", profilePoco.Id);

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

        public void Update(params ApplicantProfilePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var profilePoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Applicant_Profiles]
                                                    SET 
                                                     [Login] = @Login
                                                    ,[Current_Salary] = @Current_Salary
                                                    ,[Current_Rate] = @Current_Rate
                                                    ,[Currency] = @Currency
                                                    ,[Country_Code] = @Country_Code
                                                    ,[State_Province_Code] = @State_Province_Code
                                                    ,[Street_Address] = @Street_Address
                                                    ,[City_Town] = @City_Town
                                                    ,[Zip_Postal_Code] = @Zip_Postal_Code
                                                     WHERE [Id]=@Id";
                            command.Parameters.AddWithValue("@Id", profilePoco.Id);
                            command.Parameters.AddWithValue("@Login", profilePoco.Login);
                            command.Parameters.AddWithValue("@Current_Salary", profilePoco.CurrentSalary);
                            command.Parameters.AddWithValue("@Current_Rate", profilePoco.CurrentRate);
                            command.Parameters.AddWithValue("@Currency", profilePoco.Currency);
                            command.Parameters.AddWithValue("@Country_Code", profilePoco.Country);
                            command.Parameters.AddWithValue("@State_Province_Code", profilePoco.Province);
                            command.Parameters.AddWithValue("@Street_Address", profilePoco.Street);
                            command.Parameters.AddWithValue("@City_Town", profilePoco.City);
                            command.Parameters.AddWithValue("@Zip_Postal_Code", profilePoco.PostalCode);

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