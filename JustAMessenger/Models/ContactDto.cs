using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustAMessenger.Models
{
	public class ContactDto
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public ContactDto()
		{

		}

		public ContactDto(string userId, string userName)
		{
			Id = userId;
			Name = userName;
		}
	}
}