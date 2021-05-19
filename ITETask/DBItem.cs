using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITETask
{

    class DBItem
    {
        public String Date { get; set; }
        public String Organisation { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public String Manager { get; set; }
        public int Amount { get; set; }
        public float Sum { get; set; }
    }
}
