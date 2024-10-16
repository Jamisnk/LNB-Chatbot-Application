using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace Chatbot_Application
{
    public partial class TimeOff : Form
    {
        string empID;
        SqlConnection conn = null;   // One Connection to Database
        SqlDataReader readerTaskList = null;  // One Reader for Task List

        public TimeOff(string text)
        {
            InitializeComponent();
            empID = text;
            conn = new SqlConnection(@"Data Source=DESKTOP-RG7KVHB\SQLEXPRESS;Initial Catalog=ChatbotDB;Integrated Security=True;TrustServerCertificate=True;");
            conn.Open();

            string query = "SELECT DISTINCT ShiftID, Shift_Date, Shift_Start, Shift_End FROM Shift WHERE EmployeeID = @EmpID AND Worked = @Worked";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EmpID", empID);
            cmd.Parameters.AddWithValue("@Worked", 1);

            readerTaskList = cmd.ExecuteReader();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("Table1");
            ds.Tables.Add(dt);
            ds.Load(readerTaskList, LoadOption.PreserveChanges, ds.Tables[0]);
            readerTaskList.Close();
            dataGridView1.DataSource = ds.Tables[0];
        }

        public Form FormToShowOnClosing { get; set; }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            textBox1.Text = monthCalendar1.SelectionRange.Start.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Boolean UserValid;
            string connString = @"Data Source=DESKTOP-RG7KVHB\SQLEXPRESS;Initial Catalog=ChatbotDB;Integrated Security=True;TrustServerCertificate=True;";

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connString))
                {
                    string query = "SELECT Shift_Date from Shift WHERE Shift_Date = @ShiftDate AND EmployeeID = @EmpID";
                    SqlCommand cmd = new SqlCommand(query, myConnection);
                    cmd.Parameters.AddWithValue("@ShiftDate", textBox1.Text);
                    cmd.Parameters.AddWithValue("@EmpID", empID);

                    myConnection.Open();
                    SqlDataReader readerReturnValue = cmd.ExecuteReader();

                    if (readerReturnValue.HasRows == true)
                    {
                        UserValid = true;
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result = MessageBox.Show("Would you like to take " + textBox1.Text + " off?", "Confirmation", buttons);
                        if (result == DialogResult.No)
                        {
                            this.Close();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(textBox2.Text))
                            {
                                myConnection.Close();
                                using (SqlConnection conn = new SqlConnection(connString))
                                {
                                    conn.Open();
                                    string query2 = "SELECT ShiftID from Shift WHERE Shift_Date = @ShiftDate AND EmployeeID = @EmpID";
                                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                                    cmd2.Parameters.AddWithValue("@ShiftDate", textBox1.Text);
                                    cmd2.Parameters.AddWithValue("@EmpID", empID);

                                    SqlDataReader readerReturnValue2 = cmd2.ExecuteReader();
                                    DataTable table = new DataTable();
                                    table.Load(readerReturnValue2);
                                    string x = table.Rows[0][0].ToString();
                                    readerReturnValue2.Close();

                                    string query3 = "INSERT INTO Request (RequestDate, Approved, Reason, ShiftID) VALUES (@RequestDate, @Approved, @Reason, @ShiftID)";
                                    SqlCommand cmd3 = new SqlCommand(query3, conn);
                                    cmd3.Parameters.AddWithValue("@RequestDate", monthCalendar1.TodayDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                                    cmd3.Parameters.AddWithValue("@Approved", 0);
                                    cmd3.Parameters.AddWithValue("@Reason", textBox2.Text);
                                    cmd3.Parameters.AddWithValue("@ShiftID", x);
                                    cmd3.ExecuteNonQuery();

                                    string query4 = "UPDATE Shift SET Worked = @Worked WHERE ShiftID = @ShiftID";
                                    SqlCommand cmd4 = new SqlCommand(query4, conn);
                                    cmd4.Parameters.AddWithValue("@Worked", 0);
                                    cmd4.Parameters.AddWithValue("@ShiftID", x);
                                    cmd4.ExecuteNonQuery();
                                }

                                MessageBox.Show("Confirmed");
                            }
                            else
                            {
                                MessageBox.Show("You need to provide a reason for this request");
                            }
                        }
                    }
                    else
                    {
                        UserValid = false;
                        MessageBox.Show("I'm sorry, you do not work this day so I can't make that request for you.");
                        textBox1.Text = "";
                    }

                    myConnection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }
    }
}
