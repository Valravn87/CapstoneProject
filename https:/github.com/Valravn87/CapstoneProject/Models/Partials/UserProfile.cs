using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapstoneProject.Models
{
	public partial class UserProfile
	{
		public UserProfile(RegisterViewModel register, string userId)
		{
			isOwner = register.isOwner;
			Person = new Person(register);
			Id = userId;
		}
	}
}