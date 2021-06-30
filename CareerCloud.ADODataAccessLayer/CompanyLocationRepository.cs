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
    public class CompanyLocationRepository : IDataRepository<CompanyLocationPoco>
    {
        protected readonly string _connStr = string.Empty;

        public CompanyLocationRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyLocationPoco[] items)
        {
            try
            {
                foreach (CompanyLocationPoco companylocpoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Company_Locations]
                                                                       ([Id]
                                                                       ,[Company]
                                                                       ,[Country_Code]
                                                                       ,[State_Province_Code]
                                                                       ,[Street_Address]
                                                                       ,[City_Town]
                                                                       ,[Zip_Postal_Code])
                                                                 VALUES
                                                                       (@Id
                                                                       ,@Company
                                                                       ,@Country_Code
                                                                       ,@State_Province_Code
                                                                       ,@Street_Address
                                                                       ,@City_Town
                                                                       ,@Zip_Postal_Code)", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", companylocpoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Company", companylocpoco.Company);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Country_Code", companylocpoco.CountryCode);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@State_Province_Code", companylocpoco.Province);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Street_Address", companylocpoco.Street);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@City_Town", companylocpoco.City);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Zip_Postal_Code", companylocpoco.PostalCode);


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

        public IList<CompanyLocationPoco> GetAll(params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            IList<CompanyLocationPoco> pocoResult = new List<CompanyLocationPoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                          ,[Company]
                                          ,[Country_Code]
                                          ,[State_Province_Code]
                                          ,[Street_Address]
                                          ,[City_Town]
                                          ,[Zip_Postal_Code]
                                          ,[Time_Stamp]
                                      FROM [dbo].[Company_Locations]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CompanyLocationPoco companylocpoco = new CompanyLocationPoco();

                        companylocpoco.Id = reader.GetGuid(0);
                        companylocpoco.Company = reader.GetGuid(1);
                        companylocpoco.CountryCode = reader.GetString(2);
                        companylocpoco.Province = reader.IsDBNull(3) ? (String?)null : reader.GetString(3);
                        companylocpoco.Street = reader.IsDBNull(4) ? (String?)null : reader.GetString(4);
                        companylocpoco.City = reader.IsDBNull(5) ? (String?)null : reader.GetString(5);
                        companylocpoco.PostalCode = reader.IsDBNull(6) ? (String?)null : reader.GetString(6);
                        companylocpoco.TimeStamp = (byte[])reader[7];
                        pocoResult.Add(companylocpoco);
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

        public IList<CompanyLocationPoco> GetList(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyLocationPoco GetSingle(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyLocationPoco> companylocpoco = GetAll().AsQueryable();
            return companylocpoco.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyLocationPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (CompanyLocationPoco companylocpoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[Company_Locations] WHERE  [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", companylocpoco.Id);

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

        public void Update(params CompanyLocationPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var companylocpoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Company_Locations]
                                                   SET 
                                                      [Company] = @Company
                                                      ,[Country_Code] = @Country_Code
                                                      ,[State_Province_Code] = @State_Province_Code
                                                      ,[Street_Address] = @Street_Address
                                                      ,[City_Town] = @City_Town
                                                      ,[Zip_Postal_Code] = @Zip_Postal_Code
                                                       WHERE  [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", companylocpoco.Id);
                            command.Parameters.AddWithValue("@Company", companylocpoco.Company);
                            command.Parameters.AddWithValue("@Country_Code", companylocpoco.CountryCode);
                            command.Parameters.AddWithValue("@State_Province_Code", companylocpoco.Province);
                            command.Parameters.AddWithValue("@Street_Address", companylocpoco.Street);
                            command.Parameters.AddWithValue("@City_Town", companylocpoco.City);
                            command.Parameters.AddWithValue("@Zip_Postal_Code", companylocpoco.PostalCode);

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