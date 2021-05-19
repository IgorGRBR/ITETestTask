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
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;
using System.Collections;

namespace ITETask
{
    public partial class MainForm : Form
    {
        private SqlConnection sqlConnection; //Database connection
        //columnPositions contains property names and their respective positions in dataGrid
        private static Dictionary<String, int> columnPositions = new Dictionary<string, int>{
            {"date", 0 },
            {"organisation", 1 },
            {"city", 2 },
            {"country", 3 },
            {"manager", 4 },
            //EDIT: Amount and sum are never used!
            //{"amount", 5 },
            //{"sum", 6 },
        };
        //Array of property names
        private static string[] properties = columnPositions.Select(x => x.Key).ToArray();

        public MainForm(SqlConnection connection)
        {
            InitializeComponent();
            sqlConnection = connection;
            dataGrid.DataSource = getListOfItems(properties);
            refreshColumns(properties);
        }

        //Retrieves a list of DBItems from database
        private List<DBItem> getListOfItems(in string[] columns)
        {
            List<DBItem> result = new List<DBItem>();

            DBItemFactory factory = new DBItemFactory(columns);

            string selectedColumns = string.Join(", ", columns.Select(x => "t." + x).ToArray());
            string orderRow = columns.Count() > 0 ? columns[0] : "amt";
            string trailingComma = columns.Count() > 0 ? "," : "";

            string queryString = "SELECT " + selectedColumns + trailingComma + @" sum(t.amount) as amt, sum(t.sum)
                             FROM dbo.MainTable t
                             GROUP BY " + selectedColumns + @"
                             ORDER BY " + orderRow + " ;";

            //This doesnt work :(
            //string queryString = "SELECT @PARAMS " + trailingComma + @" sum(t.amount) as amt, sum(t.sum)
            //                       FROM dbo.MainTable t
            //                       GROUP BY @PARAMS
            //                       ORDER BY @ORD_ROW ;";

            SqlCommand cmd = new SqlCommand(queryString, sqlConnection);
            //cmd.Parameters.AddWithValue("@PARAMS", selectedColumns);
            //cmd.Parameters.AddWithValue("@ORD_ROW", orderRow);
            

            sqlConnection.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string[] values = new string[columns.Count() + 2]; //Adding 2 because of amount and sum rows
                    for (int i = 0; i < values.Count(); i++)
                        values[i] = reader[i].ToString();
                    result.Add(factory.build(values));
                }    
            }
            sqlConnection.Close();

            return result;
        }

        //Updates columns visibility based on the list of names of visible columns
        private void refreshColumns(in string[] columns)
        {
            for (int i = 0; i < dataGrid.Columns.Count - 2; i++)
            {
                dataGrid.Columns[i].Visible = false;
            }
            foreach (var column in columns)
            {
                int pos = columnPositions[column];
                dataGrid.Columns[pos].Visible = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Sets all items in parametersBox checked
            for (int i = 0; i < parametersBox.Items.Count; i++)
            {
                parametersBox.SetItemChecked(i, true);
            }
        }

        //Updates dataGrid
        private void filterButton_Click(object sender, EventArgs e)
        {
            List<string> propsList = new List<string>();
            foreach (int ind in parametersBox.CheckedIndices)
            {
                propsList.Add(properties[ind]);
            }
            string[] props = propsList.ToArray();
            dataGrid.DataSource = getListOfItems(props);
            refreshColumns(props);
        }
    }
}
