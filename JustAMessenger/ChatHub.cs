using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustAMessenger.Models;
using Newtonsoft.Json;

namespace JustAMessenger
{
	public class ChatHub : Hub
	{
		private readonly ApplicationDbContext context = new ApplicationDbContext();
		public void Hello()
		{
			Clients.All.hello();
		}

		public void QueryUserContacts(string currentUserId)
		{
			var userIds = context.ContactRelations.Where(r => r.InitiatorUserId == currentUserId && r.IsConfirmed)
				.Select(r => r.ReceiverUserId).ToList();
			var contacts = context.Users.Where(u => userIds.Contains(u.Id))
				.Select(u => new ContactDto {Id = u.Id, Name = u.UserName}).OrderBy(m => m.Name).ToList();
			Clients.Caller.printContacts(contacts);
		}

		public void QueryUserRequests(string currentUserId)
		{
			var userIds = context.ContactRelations.Where(r => r.ReceiverUserId == currentUserId && !r.IsConfirmed)
				.Select(r => r.InitiatorUserId).ToList();
			var contacts = context.Users.Where(u => userIds.Contains(u.Id))
				.Select(u => new ContactDto { Id = u.Id, Name = u.UserName }).OrderBy(m=>m.Name).ToList();
			Clients.Caller.printRequests(contacts);
		}

		public void QueryMessages(string currentUserId, string currentContactId)
		{
			var messages = context.Messages.Where(m =>
				(m.SenderId == currentUserId && m.ReceiverId == currentContactId) ||
				(m.ReceiverId == currentUserId && m.SenderId == currentContactId)).OrderBy(m=>m.Timestamp).ToList();
			Clients.Caller.printMessages(messages);

		}

		public void Send(string senderId, string receiverId, string text)
		{
			if (senderId == Guid.Empty.ToString() || receiverId == Guid.Empty.ToString() || string.IsNullOrEmpty(text)) return;
			var message = new Message(context, text, senderId, receiverId);
			context.Messages.Add(message);
			context.SaveChanges();
			Clients.All.addNewMessageToPage(message);
		}

		public void AddContactRequest(string senderId, string receiverName)
		{
			if (senderId == Guid.Empty.ToString() || string.IsNullOrEmpty(receiverName)) return;
			var receiverContact = context.Users.FirstOrDefault(u => u.UserName == receiverName);
			if (receiverContact == null)
			{
				Clients.Caller.contactRequestAdded(receiverName, false);
				return;
			};
			var contactRelationItem = context.ContactRelations.FirstOrDefault(r => r.InitiatorUserId == senderId && r.ReceiverUserId == receiverContact.Id);
			if (contactRelationItem != null)
			{
				Clients.Caller.contactRequestAdded(receiverName, false);
				return;
			};
			var newContactRelationItem = new ContactRelation(senderId, receiverContact.Id);
			context.ContactRelations.Add(newContactRelationItem);
			context.SaveChanges();
			Clients.Caller.contactRequestAdded(receiverName, true);
		}

		public void ConfirmContactRequest(string senderId, string receiverId)
		{
			var contactRelationEntity = context.ContactRelations.FirstOrDefault(cr =>
				cr.ReceiverUserId == senderId && cr.InitiatorUserId == receiverId && !cr.IsConfirmed);
			if (contactRelationEntity == null) return;
			contactRelationEntity.IsConfirmed = true;
			context.ContactRelations.Add(new ContactRelation(senderId, receiverId, true));
			context.SaveChanges();
			Clients.Caller.contactRequestFulfilled();
		}

		public void RejectContactRequest(string senderId, string receiverId)
		{
			var contactRelationEntity = context.ContactRelations.FirstOrDefault(cr =>
				cr.ReceiverUserId == senderId && cr.InitiatorUserId == receiverId && !cr.IsConfirmed);
			if (contactRelationEntity == null) return;
			context.ContactRelations.Remove(contactRelationEntity);
			context.SaveChanges();
			Clients.Caller.contactRequestFulfilled();
		}

		public void SaveMessagesToGDrive(string currentUserId, string currentContactId)
		{
			var messages = context.Messages.Where(m =>
				(m.SenderId == currentUserId && m.ReceiverId == currentContactId) ||
				(m.ReceiverId == currentUserId && m.SenderId == currentContactId)).OrderBy(m => m.Timestamp).ToList();
			var serializedMessages = JsonConvert.SerializeObject(messages);
		}
	}
}