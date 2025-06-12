using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercici5.Models
{
    public class Product
    {
        public string ProductName { get; set; }
        public int Capacity { get; set; }
        public int Consume { get; set; }
        public int CurrentCharge { get; set; } = 0;
        public int ChargesDone { get; set; } = 0;
    }
}
