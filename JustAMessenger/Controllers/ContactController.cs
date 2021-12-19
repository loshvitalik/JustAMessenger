using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using JustAMessenger.Models;

namespace JustAMessenger.Controllers
{
	public class ContactController : ApiController
	{
		private ApplicationDbContext context = new ApplicationDbContext();
		[Route("api/Contact/GetUserContacts")]
		public IEnumerable<ContactDto> Get(string userId)
		{
			var contactIds = context.ContactRelations.Where(r => r.IsConfirmed && r.InitiatorUserId == userId)
				.Select(r => r.ReceiverUserId).ToList();
			var contacts = context.Users.Where(u => contactIds.Contains(u.Id)).Select(u => new ContactDto(u.Id, u.UserName)).ToList();
			return contacts;
		}

		// GET api/<controller>/5
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<controller>
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<controller>/5
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<controller>/5
		public void Delete(int id)
		{
		}
	}
}