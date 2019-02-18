using CapstoneProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapstoneProject.Models
{
	[MetadataType(typeof(PersonValidation))]
	public partial class Person
	{
		public Person(RegisterViewModel register)
		{
			FirstName = register.FirstName;
			LastName = register.LastName;
			PhoneNum = register.PhoneNum;
			Email = register.Email;
			RestaurantId = register.RestaurantId;
		}

		public string Name { get { return FirstName + " " + LastName; } }

		public void UpdateSchedule(EmployeeScheduleVM scheduleVM, CapstoneDBEntities db)
		{
			db.ScheduleDays.RemoveRange(ScheduleDays);
			ScheduleDays.Clear();
			if (scheduleVM.SundayStart.HasValue)
				ScheduleDays.Add(new ScheduleDay
			{
				DayOfWeek = 0,
				StartTime = scheduleVM.SundayStart - scheduleVM.SundayStart.Value.Date,
			});
			if (scheduleVM.MondayStart.HasValue)
				ScheduleDays.Add(new ScheduleDay
			{
				DayOfWeek = 1,
				StartTime = scheduleVM.MondayStart - scheduleVM.MondayStart.Value.Date,
			});
			if (scheduleVM.TuesdayStart.HasValue)
				ScheduleDays.Add(new ScheduleDay
			{
				DayOfWeek = 2,
				StartTime = scheduleVM.TuesdayStart - scheduleVM.TuesdayStart.Value.Date,
			});
			if (scheduleVM.WednesdayStart.HasValue)
				ScheduleDays.Add(new ScheduleDay
				{
					DayOfWeek = 3,
					StartTime = scheduleVM.WednesdayStart - scheduleVM.WednesdayStart.Value.Date,
				});
			if (scheduleVM.ThursdayStart.HasValue)
				ScheduleDays.Add(new ScheduleDay
			{
				DayOfWeek = 4,
				StartTime = scheduleVM.ThursdayStart - scheduleVM.ThursdayStart.Value.Date,
			});
			if (scheduleVM.FridayStart.HasValue)
				ScheduleDays.Add(new ScheduleDay
			{
				DayOfWeek = 5,
				StartTime = scheduleVM.FridayStart - scheduleVM.FridayStart.Value.Date,
			});
			if (scheduleVM.SaturdayStart.HasValue)
				ScheduleDays.Add(new ScheduleDay
			{
				DayOfWeek = 6,
				StartTime = scheduleVM.SaturdayStart - scheduleVM.SaturdayStart.Value.Date,
			});
		}
	}

	public class PersonValidation
	{
		[Required]
		[Display(Name = "First Name")]
		[StringLength(255, MinimumLength = 3)]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Last Name")]
		[StringLength(255, MinimumLength = 3)]
		public string LastName { get; set; }

		[Required]
		[Display(Name = "Phone Number")]
		[StringLength(255, MinimumLength = 10)]
		public string PhoneNum { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Balance Owed")]
		[DataType(DataType.Currency)]
		public decimal BalanceOwed { get; set; }
	}
}