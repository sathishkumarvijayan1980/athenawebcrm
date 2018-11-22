using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using Antlr.Runtime;
using CRMManager.Models;

namespace CRMManager.DataAccess
{
    public class AuthenticationDao
    {

        public Authentication LoginSearch(Authentication authentication)
        {
            string sqlSelectQuery =
                 "select UserName,UserType,Mobile from Users where LoginId=@loginid and Password=@password and IsActive=1";

            SqlCommand sqlCommand = null;
            try
            {   
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    sqlCommand = new SqlCommand(sqlSelectQuery, connection);
                    sqlCommand.Parameters.AddWithValue("@loginid", authentication.LoginId);
                    sqlCommand.Parameters.AddWithValue("@password", authentication.Password);

                    sqlCommand.Connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        authentication.UserName = reader[0] + "";
                        authentication.UserType = reader[1] + "";
                        authentication.Mobile = reader[2] + "";
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                sqlCommand?.Connection?.Close();
            }

            return authentication;
        }
    }
}