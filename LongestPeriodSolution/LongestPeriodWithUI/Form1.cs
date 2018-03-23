using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LongestPeriod;

namespace LongestPeriodWithUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = null;
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                path = file.FileName;
            }

            LongestPeriod.FindEmployeesThatHaveWorkedTheLongestTogether(path);

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Employeed ID #1");
            dataTable.Columns.Add("Employeed ID #2");
            dataTable.Columns.Add("Project IDs");
            dataTable.Columns.Add("Days worked");
            
            dataGridView1.DataSource = dataTable;

            StringBuilder projectIDs = new StringBuilder();
            for (int i = 0; i < LongestPeriod.EmployeesPairsAndProjectIds.ElementAt(LongestPeriod.EmployeesPairWhoWorkedTheMostTogetherIndex).Value.Count; i++)
            {
                projectIDs.Append(LongestPeriod.EmployeesPairsAndProjectIds.ElementAt(LongestPeriod.EmployeesPairWhoWorkedTheMostTogetherIndex).Value.ElementAt(i) + " ");
            }

            dataTable.Rows.Add(new object[] { LongestPeriod.FirstEmpID,
                LongestPeriod.SecondEmpID,
                projectIDs,
                LongestPeriod.MaxDays});
        }
    }
}
