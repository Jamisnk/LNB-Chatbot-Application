﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Chatbot_Application
{
    public partial class Chatbot : Form
    {
        string empID;
        public Chatbot(string EmployeeID, string firstName)
        {
            InitializeComponent();
            botReply.Text = "Hi " + firstName +  "! I'm the Chatbot, how can I help you?";
            empID = EmployeeID;
        }
        public class UserInfo
        {
            public string EmpID { get; set; }
        }

        public Form FormToShowOnClosing { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = userTextEntry.Text;
            bool confirmSelection = false;

                if (userTextEntry.Text.ToLower().Contains("time") || userTextEntry.Text.ToLower().Contains("absence") || userTextEntry.Text.ToLower().Contains("leave"))
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("Would you like to take time off?", "Confirmation", buttons);
                    if (result == DialogResult.No)
                    {
                        botReply.Text = "Ok, how else may I help you?";
                        userTextEntry.Text = String.Empty;
                    }
                    else
                    {
                        userTextEntry.Text = String.Empty;
                        var timeOffForm = new TimeOff(empID);
                        timeOffForm.FormToShowOnClosing = this;
                        timeOffForm.Show();
                    }
                }
                else if (userTextEntry.Text.ToLower().Contains("cover") || userTextEntry.Text.ToLower().Contains("work for") || userTextEntry.Text.ToLower().Contains("take over"))
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("Would you like to cover someone else's shift(s)?", "Confirmation", buttons);
                    if (result == DialogResult.No)
                    {
                        botReply.Text = "Ok, how else may I help you?";
                        userTextEntry.Text = String.Empty;
                    }
                    else
                    {
                        userTextEntry.Text = String.Empty;
                        var CoverTimeForm = new CoverTime(empID);
                        CoverTimeForm.FormToShowOnClosing = this;
                        CoverTimeForm.Show();
                    }
                }
                else
                {
                    botReply.Text = "I'm sorry, I don't quite understand. Try typing something else like 'time', 'cover', 'absence', or 'work for''";
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void employeeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            if(empID == "1")
            {
                this.Validate();
                this.employeeBindingSource.EndEdit();
                this.tableAdapterManager.UpdateAll(this.ChatbotDBDataSet1);
                MessageBox.Show("Employee Successfully Saved");

            }
            else
            {
                MessageBox.Show("You do not have permission to save new employees.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Chatbot_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_566_DBaseDataSet1.Employee' table. You can move, or remove it, as needed.
            this.employeeTableAdapter.Fill(this.ChatbotDBDataSet1.Employee);

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string filePath = @"C:\Users\jamis\Downloads\PowerBI Visualization.pbix";
            Process.Start(filePath);
        }
    }
}
