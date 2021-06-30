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
    public class SecurityLoginRepository : IDataRepository<SecurityLoginPoco>
    {
        protected readonly string _connStr = string.Empty;

        public SecurityLoginRepository()
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params SecurityLoginPoco[] items)
        {
            try
            {
                foreach (SecurityLoginPoco loginPoco in items)
                {
                    using (SqlConnection con = new SqlConnection(_connStr))
                    {
                        using SqlDataAdapter dataAdapter = new SqlDataAdapter();
                        {
                            dataAdapter.InsertCommand = new SqlCommand(@"INSERT INTO [dbo].[Security_Logins]
                                                                      ([Id]
                                                                      ,[Login]
                                                                      ,[Password]
                                                                      ,[Created_Date]
                                                                      ,[Password_Update_Date]
                                                                      ,[Agreement_Accepted_Date]
                                                                      ,[Is_Locked]
                                                                      ,[Is_Inactive]
                                                                      ,[Email_Address]
                                                                      ,[Phone_Number]
                                                                      ,[Full_Name]
                                                                      ,[Force_Change_Password]
                                                                      ,[Prefferred_Language])
                                                                        VALUES
                                                                      (@Id
                                                                      ,@Login
                                                                      ,@Password
                                                                      ,@Created_Date
                                                                      ,@Password_Update_Date
                                                                      ,@Agreement_Accepted_Date
                                                                      ,@Is_Locked
                                                                      ,@Is_Inactive
                                                                      ,@Email_Address
                                                                      ,@Phone_Number
                                                                      ,@Full_Name
                                                                      ,@Force_Change_Password
                                                                      ,@Prefferred_Language)", con);

                            //Adding values to the Insert Command
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Id", loginPoco.Id);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Login", loginPoco.Login);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Password", loginPoco.Password);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Created_Date", loginPoco.Created);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Password_Update_Date", loginPoco.PasswordUpdate);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Agreement_Accepted_Date", loginPoco.AgreementAccepted);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Is_Locked", loginPoco.IsLocked);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Is_Inactive", loginPoco.IsInactive);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Email_Address", loginPoco.EmailAddress);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Phone_Number", loginPoco.PhoneNumber);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Full_Name", loginPoco.FullName);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Force_Change_Password", loginPoco.ForceChangePassword);
                            dataAdapter.InsertCommand.Parameters.AddWithValue("@Prefferred_Language", loginPoco.PrefferredLanguage);

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
                Debug.WriteLine(ex.Message, "Add - SecurityLoginPoco");
            }
        }

        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<SecurityLoginPoco> GetAll(params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            IList<SecurityLoginPoco> pocoResult = new List<SecurityLoginPoco>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = con;
                    command.CommandText = @"SELECT [Id]
                                            ,[Login]
                                            ,[Password]
                                            ,[Created_Date]
                                            ,[Password_Update_Date]
                                            ,[Agreement_Accepted_Date]
                                            ,[Is_Locked]
                                            ,[Is_Inactive]
                                            ,[Email_Address]
                                            ,[Phone_Number]
                                            ,[Full_Name]
                                            ,[Force_Change_Password]
                                            ,[Prefferred_Language]
                                            ,[Time_Stamp]
                                            FROM [dbo].[Security_Logins]"; 

                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SecurityLoginPoco loginPoco = new SecurityLoginPoco();

                        loginPoco.Id = reader.GetGuid(0);
                        loginPoco.Login = reader.GetString(1);
                        loginPoco.Password = reader.GetString(2);
                        loginPoco.Created = reader.GetDateTime(3);
                        loginPoco.PasswordUpdate = reader.IsDBNull(4) ? (DateTime?)null:reader.GetDateTime(4);
                        loginPoco.AgreementAccepted = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5);
                        loginPoco.IsLocked = reader.GetBoolean(6);
                        loginPoco.IsInactive = reader.GetBoolean(7);
                        loginPoco.EmailAddress= reader.GetString(8);
                        loginPoco.PhoneNumber = reader.IsDBNull(9) ? (string)null : reader.GetString(9);
                        loginPoco.FullName = reader.GetString(10);
                        loginPoco.ForceChangePassword = reader.GetBoolean(11);
                        loginPoco.PrefferredLanguage = reader.IsDBNull(12) ? (string)null : reader.GetString(12);
                        loginPoco.TimeStamp= (byte[])reader[13];

                        pocoResult.Add(loginPoco);
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
                Debug.WriteLine(ex.Message, "GetAll - SecurityLoginPoco");
            }

            return pocoResult.Where(a => a != null).ToList();
        }

        public IList<SecurityLoginPoco> GetList(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public SecurityLoginPoco GetSingle(Expression<Func<SecurityLoginPoco, bool>> where, params Expression<Func<SecurityLoginPoco, object>>[] navigationProperties)
        {
            IQueryable<SecurityLoginPoco> loginPoco = GetAll().AsQueryable();

            return loginPoco.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (SecurityLoginPoco loginPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"DELETE FROM [dbo].[Security_Logins] WHERE [Id]= @Id";
                            command.Parameters.AddWithValue("@Id", loginPoco.Id);

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
                Debug.WriteLine(ex.Message, "Remove - SecurityLoginPoco");
            }
        }

        public void Update(params SecurityLoginPoco[] items)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
                {
                    foreach (var loginsPoco in items)
                    {
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = con;
                            command.CommandText = @"UPDATE [dbo].[Security_Logins] SET [Login]=@Login
                                                        ,[Password]=@Password
                                                        ,[Created_Date]=@Created_Date
                                                        ,[Password_Update_Date]=@Password_Update_Date
                                                        ,[Agreement_Accepted_Date]=@Agreement_Accepted_Date
                                                        ,[Is_Locked]=@Is_Locked
                                                        ,[Is_Inactive]=@Is_Inactive
                                                        ,[Email_Address]=@Email_Address
                                                        ,[Phone_Number]=@Phone_Number
                                                        ,[Full_Name]=@Full_Name
                                                        ,[Force_Change_Password]=@Force_Change_Password
                                                        ,[Prefferred_Language]=@Prefferred_Language                                          
                                                    WHERE [Id]= @Id";

                            command.Parameters.AddWithValue("@Id", loginsPoco.Id);
                            command.Parameters.AddWithValue("@Login", loginsPoco.Login);
                            command.Parameters.AddWithValue("@Password", loginsPoco.Password);
                            command.Parameters.AddWithValue("@Created_Date", loginsPoco.Created);
                            command.Parameters.AddWithValue("@Password_Update_Date", loginsPoco.PasswordUpdate);
                            command.Parameters.AddWithValue("@Agreement_Accepted_Date", loginsPoco.AgreementAccepted);
                            command.Parameters.AddWithValue("@Is_Locked", loginsPoco.IsLocked);
                            command.Parameters.AddWithValue("@Is_Inactive", loginsPoco.IsInactive);
                            command.Parameters.AddWithValue("@Email_Address", loginsPoco.EmailAddress);
                            command.Parameters.AddWithValue("@Phone_Number", loginsPoco.PhoneNumber);
                            command.Parameters.AddWithValue("@Full_Name", loginsPoco.FullName);
                            command.Parameters.AddWithValue("@Force_Change_Password", loginsPoco.ForceChangePassword);
                            command.Parameters.AddWithValue("@Prefferred_Language", loginsPoco.PrefferredLanguage);


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
                Debug.WriteLine(ex.Message, "Update - SecurityLoginPoco");
            }
        }
    }
}
