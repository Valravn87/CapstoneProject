using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapstoneProject.Models
{
	public partial class Item : IComparable<Item>
	{
		public int CompareTo(Item other)
		{
			if (Name == other.Name)
			{
				return Name.CompareTo(other.Name);
			}
			else
			{
				return Name.CompareTo(other.Name);
			}
		}
	}
}