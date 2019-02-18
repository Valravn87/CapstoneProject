using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapstoneProject.Models.ViewModels
{
	public class RestaurantVM
	{
		public RestaurantVM() { }

		public RestaurantVM(Restaurant rest)
		{
			Id = rest.Id;
			GroupId = rest.GroupId;
			Address = rest.Address;
			City = rest.City;
			State = rest.State;
			ZipCode = rest.ZipCode;
			PhoneNum = rest.PhoneNum;
			LocationName = rest.LocationName;
			GroupName = rest.RestaurantGroup.Name;
		}

		public int Id { get; set; }

		[Display(Name = "Group Id")]
		public int GroupId { get; set; }

		[Required]
		public string Address { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string State { get; set; }

		[Required]
		[Display(Name = "Zip Code")]
		public string ZipCode { get; set; }

		[Required]
		[Display(Name = "Phone Number")]
		public string PhoneNum { get; set; }

		[Display(Name = "Location")]
		public string LocationName { get; set; }

		[Display(Name = "Group Name")]
		public string GroupName { get; set; }

		[Display(Name = "Create Group")]
		public bool CreateGroup { get; set; }

		[Display(Name = "City/State")]
		public string CityState { get { return City + ", " + State; } }

		public string Name { get { return (LocationName != null) ? 
					GroupName + " - " + LocationName : GroupName; } }
	}
}