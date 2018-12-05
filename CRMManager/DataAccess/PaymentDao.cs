using CRMManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CRMManager.DataAccess
{
    public class PaymentDao
    {
        public List<Payment> PaymentsSearch(string searchValue, string searchKey)
        {
            string sqlSelectQuery = "";
            if ("MemberMobile".Equals(searchKey))
            {
                sqlSelectQuery =
                    "select MemberCode,MemberName,MemberMobile,PaymentDate,PaymentAmount,ReceiptNumber,PaymentMode,CollectedBy,Notes from MemberPayment where MemberMobile=@searchvalue order by PaymentDate desc";
            }
            else if ("MemberCode".Equals(searchKey))
            {
                sqlSelectQuery =
                    "select MemberCode,MemberName,MemberMobile,PaymentDate,PaymentAmount,ReceiptNumber,PaymentMode,CollectedBy,Notes from MemberPayment where MemberCode=@searchvalue order by PaymentDate desc";
            }

            SqlCommand sqlCommand = null;
            List<Payment> returnMembers = new List<Payment>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {

                    sqlCommand = new SqlCommand(sqlSelectQuery, connection);
                    sqlCommand.Parameters.AddWithValue("@searchvalue", searchValue);
                    sqlCommand.Connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var objDateTime = (DateTime)reader[3];
                        string memberPaymentDate = objDateTime.ToString("dd-MMM-yyyy");

                        Payment resultMember = new Payment
                        {
                            MemberCode = reader[0] + "",
                            MemberName = reader[1] + "",
                            MemberMobile = reader[2] + "",
                            PaymentDate = memberPaymentDate,
                            PaymentAmount = reader[4] + "",
                            ReceiptNumber = reader[5] + "",
                            PaymentMode = reader[6] + "",
                            CollectedBy = reader[7] + "",
                            Notes = reader[8] + "",
                            PaymentTowards = "" // to be replaced with db values
                        };
                        returnMembers.Add(resultMember);
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

            return returnMembers;
        }

        public string InsertMemberPayments(Payment payment)
        {
            SqlCommand command = new SqlCommand();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    DateTime paymentDateTime = Convert.ToDateTime(payment.PaymentDate);

                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Sp_putmemberpayment";

                        command.Parameters.AddWithValue("@membercode", payment.MemberCode);
                        command.Parameters.AddWithValue("@membername", payment.MemberName);
                        command.Parameters.AddWithValue("@membermobile", payment.MemberMobile);
                        command.Parameters.AddWithValue("@paymentdate", paymentDateTime);
                        command.Parameters.AddWithValue("@paymentamount", payment.PaymentAmount);
                        command.Parameters.AddWithValue("@receiptnumber", payment.ReceiptNumber);
                        command.Parameters.AddWithValue("@paymentmode", payment.PaymentMode);
                        command.Parameters.AddWithValue("@regfeesamount", payment.RegFeesAmount);
                        command.Parameters.AddWithValue("@paymenttowards", payment.PaymentTowards);
                        command.Parameters.AddWithValue("@collectedby", payment.CollectedBy);
                        command.Parameters.AddWithValue("@notes", payment.Notes);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                command?.Connection?.Close();
            }

            return null;
        }

        public string UpdateSysRefNumber(string memberPaymentId)
        {
            SqlCommand sqlCommand = new SqlCommand();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    connection.Open();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Sp_pushsysrefnumber";
                    sqlCommand.Parameters.AddWithValue("@RECEIPTNUMBER", memberPaymentId);
                    sqlCommand.ExecuteNonQuery();
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

            return null;
        }

        public List<PaymentReport> ListMemberPayments(int monthId)
        {
            int month = monthId;
            int year = DateTime.Now.Year;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand();
            List<PaymentReport> listMemberPayments = new List<PaymentReport>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    connection.Open();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Sp_getmemberspaymentlist";

                    List<SqlParameter> prm = new List<SqlParameter>()
                    {
                        new SqlParameter("@seldate", SqlDbType.Int) {Value = month},
                        new SqlParameter("@selmonth", SqlDbType.Int) {Value = month},
                        new SqlParameter("@selyear", SqlDbType.Int) {Value = year},
                    };
                    sqlCommand.Parameters.AddRange(prm.ToArray());
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objDateTime = (DateTime)ds.Tables[0].Rows[i][7];
                        string memberPaymentDate = objDateTime.ToString("dd-MMM-yyyy");

                        PaymentReport resultMember = new PaymentReport
                        {
                            MemberPaymentId = ds.Tables[0].Rows[i][0].ToString(),
                            MemberCode = (string)ds.Tables[0].Rows[i][1],
                            MemberName = (string)ds.Tables[0].Rows[i][2],
                            MemberMobile = ds.Tables[0].Rows[i][3].ToString(),
                            MemberTimeslot = (string)ds.Tables[0].Rows[i][4],
                            MemberType = (string)ds.Tables[0].Rows[i][5],
                            PaymentAmount = ds.Tables[0].Rows[i][6].ToString(),
                            PaymentDate = memberPaymentDate,
                            ReceiptNumber = (string)ds.Tables[0].Rows[i][8]
                        };

                        listMemberPayments.Add(resultMember);
                    }
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

            return listMemberPayments;
        }

        public List<SysReceipts> ReceiptsListReport(int monthId)
        {
            int month = monthId;
            int year = DateTime.Now.Year;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand();
            List<SysReceipts> listSysReceipts = new List<SysReceipts>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    connection.Open();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Sp_getsysreceipts";

                    List<SqlParameter> prm = new List<SqlParameter>()
                    {
                        new SqlParameter("@seldate", SqlDbType.Int) {Value = month},
                        new SqlParameter("@selmonth", SqlDbType.Int) {Value = month},
                        new SqlParameter("@selyear", SqlDbType.Int) {Value = year},
                    };
                    sqlCommand.Parameters.AddRange(prm.ToArray());
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objDateTime = (DateTime)ds.Tables[0].Rows[i]["Paymentdate"];
                        string memberPaymentDate = objDateTime.ToString("dd-MMM-yyyy");

                        SysReceipts resultReceipts = new SysReceipts
                        {
                            ReceiptsCount = ds.Tables[0].Rows[i]["ReceiptCount"].ToString(),
                            PaymentDate = memberPaymentDate,
                            PaymentMode = ds.Tables[0].Rows[i]["PaymentMode"].ToString(),
                            PaymentAmount = ds.Tables[0].Rows[i]["RenewalAmt"].ToString(),
                            RegFeesAmt = ds.Tables[0].Rows[i]["RegFeesAmt"].ToString(),
                            SysRefNumber = ds.Tables[0].Rows[i]["SysRefNumber"].ToString()
                        };

                        listSysReceipts.Add(resultReceipts);
                    }
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

            return listSysReceipts;
        }

        public List<PaymentReport> MemberCollectionReport(DateTime searchDateTime)
        {
            int date = Int32.Parse(searchDateTime.ToString("dd"));
            int month = searchDateTime.Month;
            int year = searchDateTime.Year;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand();
            List<PaymentReport> listMemberPayments = new List<PaymentReport>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    connection.Open();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Sp_getmemberspaymentdatewise";

                    List<SqlParameter> prm = new List<SqlParameter>()
                    {
                        new SqlParameter("@seldate", SqlDbType.Int) {Value = date},
                        new SqlParameter("@selmonth", SqlDbType.Int) {Value = month},
                        new SqlParameter("@selyear", SqlDbType.Int) {Value = year},
                    };
                    sqlCommand.Parameters.AddRange(prm.ToArray());
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objDateTime = (DateTime)ds.Tables[0].Rows[i][7];
                        string memberPaymentDate = objDateTime.ToString("dd-MMM-yyyy");

                        PaymentReport resultMember = new PaymentReport
                        {
                            MemberPaymentId = ds.Tables[0].Rows[i][0].ToString(),
                            MemberCode = (string)ds.Tables[0].Rows[i][1],
                            MemberName = (string)ds.Tables[0].Rows[i][2],
                            MemberMobile = ds.Tables[0].Rows[i][3].ToString(),
                            MemberTimeslot = (string)ds.Tables[0].Rows[i][4],
                            MemberType = (string)ds.Tables[0].Rows[i][5],
                            PaymentAmount = ds.Tables[0].Rows[i][6].ToString(),
                            PaymentDate = memberPaymentDate,
                            ReceiptNumber = (string)ds.Tables[0].Rows[i][8]
                        };

                        listMemberPayments.Add(resultMember);
                    }
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

            return listMemberPayments;
        }

        public List<PaymentReport> ListGuestPayments(int monthId)
        {
            int month = monthId;
            int year = DateTime.Now.Year;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand();
            List<PaymentReport> listGuestPayments = new List<PaymentReport>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    connection.Open();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Sp_getguestpaymentlist";

                    List<SqlParameter> prm = new List<SqlParameter>()
                    {
                        new SqlParameter("@seldate", SqlDbType.Int) {Value = month},
                        new SqlParameter("@selmonth", SqlDbType.Int) {Value = month},
                        new SqlParameter("@selyear", SqlDbType.Int) {Value = year},
                    };
                    sqlCommand.Parameters.AddRange(prm.ToArray());
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objDateTime = (DateTime)ds.Tables[0].Rows[i][6];
                        string memberPaymentDate = objDateTime.ToString("dd-MMM-yyyy");

                        PaymentReport resultMember = new PaymentReport
                        {
                            MemberPaymentId = ds.Tables[0].Rows[i][0].ToString(),
                            MemberName = (string)ds.Tables[0].Rows[i][1],
                            MemberMobile = ds.Tables[0].Rows[i][2].ToString(),
                            MemberTimeslot = (string)ds.Tables[0].Rows[i][3],
                            MemberType = (string)ds.Tables[0].Rows[i][4],
                            PaymentAmount = ds.Tables[0].Rows[i][5].ToString(),
                            PaymentDate = memberPaymentDate,
                            ReceiptNumber = (string)ds.Tables[0].Rows[i][7],
                            PaymentMode = (string)ds.Tables[0].Rows[i][8],
                            CollectedBy = (string)ds.Tables[0].Rows[i][9],
                            Notes = (string)ds.Tables[0].Rows[i][10]
                        };

                        listGuestPayments.Add(resultMember);
                    }
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

            return listGuestPayments;
        }

        public List<PaymentReport> GuestCollectionReport(DateTime searchDateTime)
        {
            int date = Int32.Parse(searchDateTime.ToString("dd"));
            int month = searchDateTime.Month;
            int year = searchDateTime.Year;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand();
            List<PaymentReport> listGuestPayments = new List<PaymentReport>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    connection.Open();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Sp_getguestpaymentdatewise";

                    List<SqlParameter> prm = new List<SqlParameter>()
                    {
                        new SqlParameter("@seldate", SqlDbType.Int) {Value = date},
                        new SqlParameter("@selmonth", SqlDbType.Int) {Value = month},
                        new SqlParameter("@selyear", SqlDbType.Int) {Value = year},
                    };
                    sqlCommand.Parameters.AddRange(prm.ToArray());
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objDateTime = (DateTime)ds.Tables[0].Rows[i][6];
                        string memberPaymentDate = objDateTime.ToString("dd-MMM-yyyy");

                        PaymentReport resultMember = new PaymentReport
                        {
                            MemberPaymentId = ds.Tables[0].Rows[i][0].ToString(),
                            MemberName = (string)ds.Tables[0].Rows[i][1],
                            MemberMobile = ds.Tables[0].Rows[i][2].ToString(),
                            MemberTimeslot = (string)ds.Tables[0].Rows[i][3],
                            MemberType = (string)ds.Tables[0].Rows[i][4],
                            PaymentAmount = ds.Tables[0].Rows[i][5].ToString(),
                            PaymentDate = memberPaymentDate,
                            ReceiptNumber = (string)ds.Tables[0].Rows[i][7],
                            PaymentMode = (string)ds.Tables[0].Rows[i][8],
                            CollectedBy = (string)ds.Tables[0].Rows[i][9],
                            Notes = (string)ds.Tables[0].Rows[i][10],
                            IsActive = (bool)ds.Tables[0].Rows[i][12]
                        };

                        listGuestPayments.Add(resultMember);
                    }
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

            return listGuestPayments;
        }

        public List<MemberPayment> MemberLastPaymentReport()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand();
            List<MemberPayment> listMemberPayments = new List<MemberPayment>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    connection.Open();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Sp_getmemberwiselastpayment";
                    
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objDateTime = (DateTime)ds.Tables[0].Rows[i]["lastpaymentdate"];
                        string memberPaymentDate = objDateTime.ToString("dd-MMM-yyyy");

                        MemberPayment membersList = new MemberPayment
                        {   
                            MemberCode = (string)ds.Tables[0].Rows[i]["membercode"],
                            MemberName = (string)ds.Tables[0].Rows[i]["membername"],
                            MemberMobile = ds.Tables[0].Rows[i]["membermobile"].ToString(),
                            MemberType = (string)ds.Tables[0].Rows[i]["membertype"],
                            TimeSlot = (string)ds.Tables[0].Rows[i]["slotname"],
                            PaymentDate = memberPaymentDate,
                            PaymentAmount = ds.Tables[0].Rows[i]["paymentamount"].ToString(),
                            IsActive = (bool)ds.Tables[0].Rows[i]["isactive"]
                        };

                        listMemberPayments.Add(membersList);
                    }
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

            return listMemberPayments;
        }
    }
}