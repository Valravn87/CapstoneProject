using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapstoneProject.Models
{
	[MetadataType(typeof(AlertValidation))]
	public partial class Alert
	{
		[Display(Name = "# Red Items")]
		public int NumRedItems { get { return ItemAlerts.Count(x => x.StockCode == 3); } }

		[Display(Name = "# Yellow Items")]
		public int NumYellowItems { get { return ItemAlerts.Count(x => x.StockCode == 2); } }

		public string Responder { get { return (Person != null) ? Person.Name : null; } }

		public void SendAlert(Person employee, string restaurantName, UrlHelper helper)
		{
			string email = employee.Email;			
			string subject = "Alert posted from " + restaurantName;
			string path = helper.Action("Respond", "Alerts", new { id = Id, employeeId = employee.Id});
			string domain = helper.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
			string url = domain + path;
			string body = employee.FirstName + ",\n\n";
			body += "We could use the following items if you have the chance to get them before your shift today:\n";
			foreach (var item in ItemAlerts)
			{
				body += item.RestaurantItem.Item.Name + "\n";
			}	
			body += "\nClick on the following link to respond:\n\n" + url;
			EmailUtility.sendMail(email, body, subject);
		}
	}

	public class AlertValidation
	{
		[Display(Name = "Time Created")]
		public DateTime TimeCreated { get; set; }

		[Display(Name = "Time Responded")]
		public DateTime? TimeResponded { get; set; }

		[Display(Name = "Alert Items")]
		public virtual ICollection<ItemAlert> ItemAlerts { get; set; }
	}
}