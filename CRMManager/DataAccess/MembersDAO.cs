using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using Antlr.Runtime;
using CRMManager.Models;

namespace CRMManager.DataAccess
{
    public class MembersDao
    {
        public string MemberRegister(Member member)
        {
            SqlCommand sqlCommand = null;
            int retValue = 0;
            try
            {
                DateTime memberDateTime = Convert.ToDateTime(member.MemberDob);
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    sqlCommand = new SqlCommand("Sp_putmemberregister", connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Sp_putmemberregister";
                    sqlCommand.Parameters.Add(new SqlParameter("@membername", member.MemberName));
                    sqlCommand.Parameters.Add(new SqlParameter("@memberaddress", member.MemberAddress));
                    sqlCommand.Parameters.Add(new SqlParameter("@membermobile", member.MemberMobile));
                    sqlCommand.Parameters.Add(new SqlParameter("@memberdob", memberDateTime));
                    sqlCommand.Parameters.Add(new SqlParameter("@membertype", member.MemberType));
                    sqlCommand.Parameters.Add(new SqlParameter("@membertimeslot", member.MemberTimeslot));
                    sqlCommand.Parameters.Add(new SqlParameter("@membercourt", member.MemberCourt));
                    sqlCommand.Parameters.Add(new SqlParameter("@memberstatus", member.MemberStatus));
                    connection.Open();
                    retValue = sqlCommand.ExecuteNonQuery();
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

            return retValue.ToString();
        }


        public List<Member> MemberSearch(Member member, string searchKey)
        {
            string sqlSelectQuery = "";
            if ("MemberMobile".Equals(searchKey))
            {
                sqlSelectQuery =
                    "select MemberCode,MemberName,MemberAddress,MemberMobile,MemberDOB,MemberType,ts.SlotName as MemberTimeslot,MemberCourt,mm.IsActive,ts.slotid as slotid from MemberMaster mm inner join TimeSlotMaster ts on mm.MemberTimeslot = ts.SlotId where MemberMobile=@searchvalue";
            }
            else if ("MemberCode".Equals(searchKey))
            {
                sqlSelectQuery =
                    "select MemberCode,MemberName,MemberAddress,MemberMobile,MemberDOB,MemberType,ts.SlotName as MemberTimeslot,MemberCourt,mm.IsActive,ts.slotid as slotid from MemberMaster mm inner join TimeSlotMaster ts on mm.MemberTimeslot = ts.SlotId where Membercode=@searchvalue";
            }
            else if ("MemberName".Equals(searchKey))
            {
                sqlSelectQuery =
                    "select MemberCode,MemberName,MemberAddress,MemberMobile,MemberDOB,MemberType,ts.SlotName as MemberTimeslot,MemberCourt,mm.IsActive,ts.slotid as slotid from MemberMaster mm inner join TimeSlotMaster ts on mm.MemberTimeslot = ts.SlotId where MemberName like @searchvalue";
            }

            SqlCommand sqlCommand = null;
            List<Member> returnMembers = new List<Member>();
            string searchValueText = "";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {

                    sqlCommand = new SqlCommand(sqlSelectQuery, connection);

                    if ("MemberName".Equals(searchKey))
                    {
                        searchValueText = "%" + member.MemberMobile + "%";
                    }
                    else if ("MemberCode".Equals(searchKey))
                    {
                        searchValueText = member.MemberCode;
                    }
                    else
                    {
                        searchValueText = member.MemberMobile;
                    }

                    sqlCommand.Parameters.AddWithValue("@searchvalue", searchValueText);

                    sqlCommand.Connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var objDateTime = (DateTime)reader[4];
                        string memberDob = objDateTime.ToString("MM/dd/yyyy");

                        Member resultMember = new Member
                        {
                            MemberCode = reader[0] + "",
                            MemberName = reader[1] + "",
                            MemberAddress = reader[2] + "",
                            MemberMobile = reader[3] + "",
                            MemberDob = memberDob,
                            MemberType = reader[5] + "",
                            MemberTimeslot = reader[6] + "",
                            MemberCourt = reader[7] + "",
                            MemberStatus = (bool)reader[8],
                            SelectedTimeSlotId = reader[9] + ""
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

        public Member MemberUpdateSearch(string profileId)
        {
            Member objMember = new Member { MemberCode = profileId };
            List<Member> memberSearch = MemberSearch(objMember, "MemberCode");
            return memberSearch[0];
        }

        public string MemberUpdate(Member member)
        {
            string sqlQuery =
                "update MemberMaster set [MemberName]=@membername,[MemberAddress]=@memberaddress,[MemberMobile]=@membermobile,[MemberType]=@membertype,[MemberTimeslot]=@membertimeslot,[MemberCourt]=@membercourt,[IsActive]=@memberstatus where [MemberCode]=@membercode";
            SqlCommand command = null;
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    command = new SqlCommand(sqlQuery, connection);
                    //DateTime memberDateTime = DateTime.ParseExact(member.MemberDob, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    command.Connection.Open();
                    {
                        command.Parameters.AddWithValue("@membercode", member.MemberCode);
                        command.Parameters.AddWithValue("@membername", member.MemberName);
                        command.Parameters.AddWithValue("@memberaddress", member.MemberAddress);
                        command.Parameters.AddWithValue("@membermobile", member.MemberMobile);
                        //command.Parameters.AddWithValue("@memberdob", memberDateTime);
                        command.Parameters.AddWithValue("@membertype", member.MemberType);
                        command.Parameters.AddWithValue("@membertimeslot", member.MemberTimeslot);
                        command.Parameters.AddWithValue("@membercourt", member.MemberCourt);
                        command.Parameters.AddWithValue("@memberstatus", member.MemberStatus);
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

        public List<Timeslot> TimeSlotMaster()
        {
            string sqlSelectQuery = "";
            sqlSelectQuery =
                "select Slotid, SlotName from TimeSlotMaster where IsActive=1";

            SqlCommand sqlCommand = null;
            List<Timeslot> objTimeslotsList = new List<Timeslot>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {

                    sqlCommand = new SqlCommand(sqlSelectQuery, connection);
                    sqlCommand.Connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        Timeslot objTimeslot = new Timeslot()
                        {
                            SlotId = reader[0] + "",
                            SlotName = reader[1] + ""
                        };
                        objTimeslotsList.Add(objTimeslot);
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

            return objTimeslotsList;
        }

        public List<Member> MembersListReport()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand();
            List<Member> membersList = new List<Member>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    connection.Open();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Sp_getmemberslistreport";
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objDateTime = (DateTime)ds.Tables[0].Rows[i][4];
                        string memberDob = objDateTime.ToString("dd-MMM-yyyy");

                        Member objMember = new Member()
                        {
                            MemberCode = (string)ds.Tables[0].Rows[i][1],
                            MemberName = (string)ds.Tables[0].Rows[i][2],
                            MemberMobile = ds.Tables[0].Rows[i][3].ToString(),
                            MemberDob = memberDob,
                            MemberTimeslot = (string)ds.Tables[0].Rows[i][5],
                            MemberCourt = (string)ds.Tables[0].Rows[i][6],
                            MemberType = (string)ds.Tables[0].Rows[i][7],
                            MemberStatus = (bool)ds.Tables[0].Rows[i][8]
                        };
                        membersList.Add(objMember);
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

            return membersList;
        }

        public List<Member> ListSlotMembers(string slotId, string courtId)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand();
            List<Member> membersList = new List<Member>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    connection.Open();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Sp_getslotmemberslist";
                    sqlCommand.Parameters.Add(new SqlParameter("@p_InSlotId", slotId));
                    sqlCommand.Parameters.Add(new SqlParameter("@p_InCourtId", courtId));
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objDateTime = (DateTime)ds.Tables[0].Rows[i][4];
                        string memberDob = objDateTime.ToString("dd-MMM-yyyy");

                        Member objMember = new Member()
                        {
                            MemberCode = (string)ds.Tables[0].Rows[i][1],
                            MemberName = (string)ds.Tables[0].Rows[i][2],
                            MemberMobile = ds.Tables[0].Rows[i][3].ToString(),
                            MemberDob = memberDob,
                            MemberTimeslot = (string)ds.Tables[0].Rows[i][5],
                            MemberCourt = (string)ds.Tables[0].Rows[i][6],
                            MemberType = (string)ds.Tables[0].Rows[i][7],
                            MemberStatus = (bool)ds.Tables[0].Rows[i][8]
                        };
                        membersList.Add(objMember);
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

            return membersList;
        }
        public List<Availability> MemberSlotAvailability()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand();
            List<Availability> slotAvailabilities = new List<Availability>();
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    connection.Open();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "sp_getmemberslotavailability";
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Availability objAvailability = new Availability()
                        {
                            MemberCourt = ds.Tables[0].Rows[i][0] + "",
                            SlotName = ds.Tables[0].Rows[i][1] + "",
                            MemberCount = ds.Tables[0].Rows[i][2] + "",
                            Capacity = ds.Tables[0].Rows[i][3] + "",
                            SlotId = ds.Tables[0].Rows[i][4] + "",
                        };
                        slotAvailabilities.Add(objAvailability);
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

            return slotAvailabilities;
        }

        public string MemberDeRegister(string memberCode)
        {
            string sqlQuery =
                "update MemberMaster set [IsActive]=@memberstatus where [MemberCode]=@membercode";
            SqlCommand command = null;
            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    command = new SqlCommand(sqlQuery, connection);
                    command.Connection.Open();
                    {
                        command.Parameters.AddWithValue("@membercode", memberCode);
                        command.Parameters.AddWithValue("@memberstatus", false);
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

        public List<ReceiptPrint> PrintPaymentReceipt(string receiptId)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand();
            List<ReceiptPrint> receiptsPrint = new List<ReceiptPrint>();

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(CommonUtility.Sqlconnectionstring))
                {
                    connection.Open();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "Sp_getreceiptprintdata";
                    sqlCommand.Parameters.AddWithValue("@RECEIPTNUMBER", receiptId);
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var objDateTime = (DateTime)ds.Tables[0].Rows[i][5];
                        var objMemberType = (string)ds.Tables[0].Rows[i][4];

                        var resMemberType = "Membership fees";

                        if (!"REGULAR".Equals(objMemberType))
                        {
                            resMemberType = "Coaching Fees";
                        }

                        ReceiptPrint objReceiptPrint = new ReceiptPrint()
                        {
                            MemberCode = (string)ds.Tables[0].Rows[i][1],
                            MemberName = (string)ds.Tables[0].Rows[i][2],
                            MemberMobile = ds.Tables[0].Rows[i][3] + "",
                            MemberType = resMemberType,

                            PaymentDate = objDateTime.ToString("dd-MMM-yyyy"),
                            PaymentPeriod = "Month of - " + objDateTime.ToString("MMMM"),
                            PaymentAmount = ds.Tables[0].Rows[i][6] + "",

                            PaymentMode = (string)ds.Tables[0].Rows[i][7],
                            ReceiptNumber = ds.Tables[0].Rows[i][8] + "",
                            SysRefNumber = ds.Tables[0].Rows[i][11] + "",
                            Timeslot = (string)ds.Tables[0].Rows[i][9],

                            CollectedBy = (string)ds.Tables[0].Rows[i][10],
                        };

                        if ("".Equals(objReceiptPrint.SysRefNumber))
                        {
                            objReceiptPrint.SysRefNumber = objReceiptPrint.ReceiptNumber;
                        }

                        receiptsPrint.Add(objReceiptPrint);
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

            return receiptsPrint;
        }
    }
}