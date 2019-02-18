using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapstoneProject.Models.ViewModels
{
	public class EmployeeAlertVM
	{
		public int AlertId { get; set; }
		public int EmployeeId { get; set; }
		public string EmployeeName { get; set; }

		public EmployeeAlertVM() { }

		public EmployeeAlertVM(Alert alert, Person emp)
		{
			AlertId = alert.Id;
			EmployeeId = emp.Id;
			EmployeeName = emp.Name;
		}
	}
}