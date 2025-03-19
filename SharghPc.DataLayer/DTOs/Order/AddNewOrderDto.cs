using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharghPc.DataLayer.DTOs.Order
{
    public class AddNewOrderDto
    {
        public long CartId { get; set; }    
        public long UserId { get; set; }
        public long RequestpayId { get; set; }

    }
}
