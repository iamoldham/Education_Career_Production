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
    public class CompanyProfileRepository : IDataRepository<CompanyProfilePoco>
    {
        protected readonly string _connStr = string.Empty;

        public CompanyProfileRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyProfilePoco[] items)
        {
            try
            {
                foreach (CompanyProfilePoco companyproPoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO[dbo].[Company_Profiles]
                                                                        ([Id]
                                                                        ,[Registration_Date]
                                                                        ,[Company_Website]
                                                                        ,[Contact_Phone]
                                                                        ,[Contact_Name]
                                                                        ,[Company_Logo])
                                                                        VALUES
                                                                        (@Id
                                                                        , @Registration_Date
                                                                        , @Company_Website
                                                                        , @Contact_Phone
                                                                        , @Contact_Name
                                                                        , @Company_Logo)", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", companyproPoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Registration_Date", companyproPoco.RegistrationDate);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Company_Website", companyproPoco.CompanyWebsite);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Contact_Phone", companyproPoco.ContactPhone);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Contact_Name", companyproPoco.ContactName);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Company_Logo", companyproPoco.CompanyLogo);

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

        public IList<CompanyProfilePoco> GetAll(params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            IList<CompanyProfilePoco> pocoResult = new List<CompanyProfilePoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                  ,[Registration_Date]
                                  ,[Company_Website]
                                  ,[Contact_Phone]
                                  ,[Contact_Name]
                                  ,[Company_Logo]
                                  ,[Time_Stamp]
                              FROM [dbo].[Company_Profiles]";

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        CompanyProfilePoco companyproPoco = new CompanyProfilePoco();

                        companyproPoco.Id = reader.GetGuid(0);

                        if (!reader.IsDBNull(reader.GetOrdinal("Registration_Date")))
                        {
                            companyproPoco.RegistrationDate = reader.GetDateTime(1);
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("Company_Website")))
                        {
                            companyproPoco.CompanyWebsite = reader.GetString(2);
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("Contact_Phone")))
                        {
                            companyproPoco.ContactPhone = reader.GetString(3);
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("Contact_Name")))
                        {
                            companyproPoco.ContactName = reader.GetString(4);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("Company_Logo")))
                        {
                            companyproPoco.CompanyLogo = (byte[])reader.GetValue(5);
                        }

                        companyproPoco.TimeStamp = (byte[])reader.GetValue(6);

                        pocoResult.Add(companyproPoco);
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

        public IList<CompanyProfilePoco> GetList(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public CompanyProfilePoco GetSingle(Expression<Func<CompanyProfilePoco, bool>> where, params Expression<Func<CompanyProfilePoco, object>>[] navigationProperties)
        {
            IQueryable<CompanyProfilePoco> companyproPoco = GetAll().AsQueryable();
            return companyproPoco.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyProfilePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (CompanyProfilePoco companyproPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM[dbo].[Company_Profiles] WHERE  [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", companyproPoco.Id);

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

        public void Update(params CompanyProfilePoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var companyproPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Company_Profiles]
                                                   SET 
                                                      [Registration_Date] = @Registration_Date
                                                      ,[Company_Website] = @Company_Website
                                                      ,[Contact_Phone] = @Contact_Phone
                                                      ,[Contact_Name] = @Contact_Name
                                                      ,[Company_Logo] = @Company_Logo
                                                        WHERE  [Id]= @Id";

                            command.Parameters.AddWithValue("@Id", companyproPoco.Id);
                            command.Parameters.AddWithValue("@Registration_Date", companyproPoco.RegistrationDate);
                            command.Parameters.AddWithValue("@Company_Website", companyproPoco.CompanyWebsite);
                            command.Parameters.AddWithValue("@Contact_Phone", companyproPoco.ContactPhone);
                            command.Parameters.AddWithValue("@Contact_Name", companyproPoco.ContactName);
                            command.Parameters.AddWithValue("@Company_Logo", companyproPoco.CompanyLogo);

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
