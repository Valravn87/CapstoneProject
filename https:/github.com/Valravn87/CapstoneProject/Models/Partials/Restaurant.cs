using CapstoneProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapstoneProject.Models
{
	public partial class Restaurant
	{
		public Restaurant(RestaurantVM rvm)
		{
			Address = rvm.Address;
			City = rvm.City;
			State = rvm.State;
			ZipCode = rvm.ZipCode;
			PhoneNum = rvm.PhoneNum;
			LocationName = rvm.LocationName;
		}

		public void UpdateFromVM(RestaurantVM rvm)
		{
			Address = rvm.Address;
			City = rvm.City;
			State = rvm.State;
			ZipCode = rvm.ZipCode;
			PhoneNum = rvm.PhoneNum;
			LocationName = rvm.LocationName;
		}
	}
}