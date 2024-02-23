using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{

    public class OrderVM
	{
		public OrderHeader OrderHeader {  get; set; }
		public IEnumerable<OrderDetail> OrderDetail { get; set; }

	}
}
