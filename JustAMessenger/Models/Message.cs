using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustAMessenger.Models
{
	public class Message
	{
		public Guid Id { get; set; }
		public string Text { get; set; }
		public DateTime Timestamp { get; set; }
		public string SenderId { get; set; }
		public string ReceiverId { get; set; }
		public string SenderName { get; set; }

		public Message()
		{
			Id = Guid.NewGuid();
		}

		public Message(ApplicationDbContext context, string text, string senderId, string receiverId)
		{
			Id = Guid.NewGuid();
			Text = text;
			Timestamp = DateTime.Now;
			SenderId = senderId;
			ReceiverId = receiverId;
			SenderName = context.Users.FirstOrDefault(u => u.Id == senderId)?.UserName ?? "Неизвестный отправитель";
		}
	}
}