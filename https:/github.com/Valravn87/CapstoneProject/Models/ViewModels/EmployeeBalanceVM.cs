using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapstoneProject.Models.ViewModels
{
	public class EmployeeBalanceVM
	{
		public int EmployeeId { get; set; }
		public string EmployeeName { get; set; }

		[DataType(DataType.Currency)]
		public decimal BalanceOwed { get; set; }

		[Range(0, 1000)]
		[DataType(DataType.Currency)]
		[Display(Name = "Receipt Amount")]
		public decimal ReceiptAmt { get; set; }

		[Range(0, 1000)]
		[DataType(DataType.Currency)]
		[Display(Name = "Amount Paid")]
		public decimal ReimburseAmt { get; set; }

		[Range(0, 1000)]
		[DataType(DataType.Currency)]
		[Display(Name = "Bonus Amount")]
		public decimal BonusAmt { get; set; }

		public EmployeeBalanceVM() { }

		public EmployeeBalanceVM(Person employee)
		{
			EmployeeId = employee.Id;
			EmployeeName = employee.Name;
			BalanceOwed = employee.BalanceOwed;
		}
	}
}