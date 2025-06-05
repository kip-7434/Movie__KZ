using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Recommendation.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Names { get; set; }
		public string Email { get; set; }
		public string PrimaryRole { get; set; }
		public string AltPhoneNumber { get; set; }
		public bool Active { get; set; } = true;
	}

}
