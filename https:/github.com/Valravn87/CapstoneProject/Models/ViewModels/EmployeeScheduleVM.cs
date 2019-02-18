using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapstoneProject.Models.ViewModels
{
	public class EmployeeScheduleVM
	{
		public EmployeeScheduleVM() { }

		public EmployeeScheduleVM(Person employee)
		{
			EmployeeId = employee.Id;
			Name = employee.Name;
			var newTime = new DateTime();
			SundayStart = newTime + employee.ScheduleDays.FirstOrDefault(x => x.DayOfWeek == 0)?.StartTime;
			MondayStart = newTime + employee.ScheduleDays.FirstOrDefault(x => x.DayOfWeek == 1)?.StartTime;
			TuesdayStart = newTime + employee.ScheduleDays.FirstOrDefault(x => x.DayOfWeek == 2)?.StartTime;
			WednesdayStart = newTime + employee.ScheduleDays.FirstOrDefault(x => x.DayOfWeek == 3)?.StartTime;
			ThursdayStart = newTime + employee.ScheduleDays.FirstOrDefault(x => x.DayOfWeek == 4)?.StartTime;
			FridayStart = newTime + employee.ScheduleDays.FirstOrDefault(x => x.DayOfWeek == 5)?.StartTime;
			SaturdayStart = newTime + employee.ScheduleDays.FirstOrDefault(x => x.DayOfWeek == 6)?.StartTime;
			if (employee.UserProfiles.Any())
			{
				IsManager = true;
			}
			else
			{
				IsManager = false;
			}
		}
		public int EmployeeId { get; set; }
		public string Name { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
		[Display(Name = "Sunday")]
		public DateTime? SundayStart { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
		[Display(Name = "Monday")]
		public DateTime? MondayStart { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
		[Display(Name = "Tuesday")]
		public DateTime? TuesdayStart { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
		[Display(Name = "Wednesday")]
		public DateTime? WednesdayStart { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
		[Display(Name = "Thursday")]
		public DateTime? ThursdayStart { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
		[Display(Name = "Friday")]
		public DateTime? FridayStart { get; set; }

		[DataType(DataType.Time)]
		[DisplayFormat(DataFormatString = "{0:h:mm tt}", ApplyFormatInEditMode = true)]
		[Display(Name = "Saturday")]
		public DateTime? SaturdayStart { get; set; }

		public bool IsManager { get; set; }
	}
}