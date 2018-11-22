using CRMManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace CRMManager.DataAccess
{
    public class GuestDao
    {
        public string GuestBookingCancel(string memberPaymentId)
        {
            string sqlQuery =
                "update GuestPayment set [IsActive]=@bookingstatus where [ID]=@bookingPaymentId";
            SqlCommand command = null;
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    command = new SqlCommand(sqlQuery, connection);
                    command.Connection.Open();
                    {
                        command.Parameters.AddWithValue("@bookingPaymentId", memberPaymentId);
                        command.Parameters.AddWithValue("@bookingstatus", false);
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

        public List<Guest> GuestSearch(Guest guest, string searchKey)
        {
            string sqlSelectQuery = "";
            if ("GuestMobile".Equals(searchKey))
            {
                sqlSelectQuery =
                    "select GuestName,GuestMobile,PaymentDate,PaymentAmount,ReceiptNumber,PaymentMode,CollectedBy,ts.SlotName,Court,Notes from GuestPayment gp inner join TimeSlotMaster ts on gp.Timeslot = ts.SlotId where GuestMobile=@searchvalue order by PaymentDate desc";
            }

            SqlCommand sqlCommand = null;
            List<Guest> guestPayments = new List<Guest>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {

                    sqlCommand = new SqlCommand(sqlSelectQuery, connection);
                    if ("GuestMobile".Equals(searchKey))
                    {
                        sqlCommand.Parameters.AddWithValue("@searchvalue", guest.GuestMobile);
                    }

                    sqlCommand.Connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {

                        var objDateTime = (DateTime)reader[2];
                        string memberPaymentDate = objDateTime.ToString("dd-MMM-yyyy");

                        Guest objGuest = new Guest
                        {
                            GuestName = reader[0] + "",
                            GuestMobile = reader[1] + "",
                            PaymentDate = memberPaymentDate,
                            PaymentAmount = reader[3] + "",
                            ReceiptNumber = reader[4] + "",
                            PaymentMode = reader[5] + "",
                            CollectedBy = reader[6] + "",
                            GuestTimeslot = reader[7] + "",
                            GuestCourt = reader[8] + "",
                            Notes = reader[9] + ""
                        };
                        guestPayments.Add(objGuest);
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

            return guestPayments;
        }


        public string GuestPayment(Guest guest)
        {
            string sqlQuery =
                "insert into GuestPayment (GuestName,GuestMobile,PaymentDate,PaymentAmount,ReceiptNumber,PaymentMode,CollectedBy,Timeslot,Court,Notes) " +
                "values(@guestname,@guestmobile,@paymentdate,@paymentamount,@receiptnumber,@paymentmode,@collectedby,@timeslot,@court,@notes)";

            SqlCommand command = null;
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    command = new SqlCommand(sqlQuery, connection);
                    DateTime guestDateTime = Convert.ToDateTime(guest.PaymentDate);
                    command.Connection.Open();
                    {
                        command.Parameters.AddWithValue("@guestname", guest.GuestName);
                        command.Parameters.AddWithValue("@guestmobile", guest.GuestMobile);
                        command.Parameters.AddWithValue("@paymentdate", guestDateTime);
                        command.Parameters.AddWithValue("@paymentamount", guest.PaymentAmount);
                        command.Parameters.AddWithValue("@receiptnumber", guest.ReceiptNumber);
                        command.Parameters.AddWithValue("@paymentmode", guest.PaymentMode);
                        command.Parameters.AddWithValue("@collectedby", guest.CollectedBy);
                        command.Parameters.AddWithValue("@timeslot", guest.GuestTimeslot);
                        command.Parameters.AddWithValue("@court", guest.GuestCourt);
                        command.Parameters.AddWithValue("@notes", guest.Notes);
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
    }
}