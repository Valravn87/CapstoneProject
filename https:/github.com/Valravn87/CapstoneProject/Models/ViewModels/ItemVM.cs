using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapstoneProject.Models.ViewModels
{
	public class ItemVM
	{
		public string Name { get; set; }
		public bool NewItem { get; set; }
		public int RestaurantId { get; set; }
		public int ItemId { get; set; }
	}
}