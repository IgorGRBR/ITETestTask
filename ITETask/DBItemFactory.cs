using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ITETask
{
    class DBItemFactory
    {
        private string[] parameters;

        public DBItemFactory(in string[] param)
        {
            parameters = param;
        }

        public DBItem build(in string[] values)
        {
            DBItem result = new DBItem();
            for(int i = 0; i < values.Count() - 2; i++) //Subtracting 2 because of extra amount and sum rows
            {
                assignValue(result, parameters[i], values[i]);
            }
            result.Amount = int.Parse(values[values.Count() - 2]);
            result.Sum = float.Parse(values[values.Count() - 1]);
            return result;
        }

        private void assignValue(DBItem item, in string name, in string value)
        {
            switch (name)
            {
                case "date":
                    item.Date = value;
                    break;
                case "organisation":
                    item.Organisation = value;
                    break;
                case "city":
                    item.City = value;
                    break;
                case "country":
                    item.Country = value;
                    break;
                case "manager":
                    item.Manager = value;
                    break;
                default:
                    Console.WriteLine("Error");
                    break;
            }
        }
    }
}
