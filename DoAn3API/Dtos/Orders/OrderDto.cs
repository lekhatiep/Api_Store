using AutoMapper;
using Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Dtos.Orders
{
    [AutoMap(typeof(Order))]
    public class OrderDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public double GrandTotal { get; set; }

        public string Status { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }
    
    }
}
