using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustAMessenger.Models
{
	public class ContactRelation
	{
		public Guid Id { get; set; }
		public string InitiatorUserId { get; set; }
		public string ReceiverUserId { get; set; }
		public bool IsConfirmed { get; set; }

		public ContactRelation()
		{
			Id = Guid.NewGuid();
		}

		public ContactRelation(string initiatorUserId, string receiverUserId, bool isConfirmed = false)
		{
			Id = Guid.NewGuid();
			InitiatorUserId = initiatorUserId;
			ReceiverUserId = receiverUserId;
			IsConfirmed = isConfirmed;
		}


	}
}