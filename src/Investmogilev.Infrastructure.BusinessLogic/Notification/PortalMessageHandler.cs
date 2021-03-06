﻿// // -----------------------------------------------------------------------
// // <copyright file="PortalMessageHandler.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Notification
{
	#region Using

	using System;
	using System.Collections.Generic;
	using Investmogilev.Infrastructure.Common;
	using Investmogilev.Infrastructure.Common.Model.Common;
	using Investmogilev.Infrastructure.Common.Model.User;

	#endregion

	public class PortalMessageHandler
	{
		private readonly PortalNotificationHub _notification;

		public PortalMessageHandler()
		{
			_notification = new PortalNotificationHub();
		}

		public void PushMessage(MessageQueue message)
		{
			if (RepositoryContext.Current.GetOne<MessageQueue>(m => m._id == message._id) != null)
			{
				var originalMessage = RepositoryContext.Current.GetOne<MessageQueue>(m => m._id == message._id);
				if (!originalMessage.IsSended && message.IsSended)
				{
					message.SendedDate = DateTime.Now;
				}

				if (!originalMessage.IsReaded && message.IsReaded)
				{
					message.ReciveTime = DateTime.Now;
				}

				if (message.Body.Appendix == null)
				{
					message.Body.Appendix = originalMessage.Body.Appendix;
				}

				RepositoryContext.Current.Update(message);
			}
			else
			{
				RepositoryContext.Current.Add(message);
			}
		}

		public IEnumerable<MessageQueue> Inbox(string userName)
		{
			return
				RepositoryContext.Current.All<MessageQueue>(
					m => (m.To == userName || m.Cc.Contains(userName)) && m.IsSended && m.Type == MessageType.Portal);
		}

		public IEnumerable<MessageQueue> Unread(string userName)
		{
			return
				RepositoryContext.Current.All<MessageQueue>(
					m => (m.To == userName || m.Cc.Contains(userName)) && !m.IsReaded && m.IsSended && m.Type == MessageType.Portal);
		}

		public IEnumerable<MessageQueue> Readed(string userName)
		{
			return
				RepositoryContext.Current.All<MessageQueue>(
					m => (m.To == userName || m.Cc.Contains(userName)) && !m.IsReaded && m.IsSended && m.Type == MessageType.Portal);
		}

		public IEnumerable<MessageQueue> SendedUnread(string userName)
		{
			return
				RepositoryContext.Current.All<MessageQueue>(
					m => (m.From == userName) && !m.IsReaded && m.IsSended && m.Type == MessageType.Portal);
		}

		public IEnumerable<MessageQueue> Sended(string userName)
		{
			return
				RepositoryContext.Current.All<MessageQueue>(m => (m.From == userName) && m.IsSended && m.Type == MessageType.Portal);
		}

		public MessageQueue Message(string userName, string id)
		{
			return
				RepositoryContext.Current.GetOne<MessageQueue>(
					m => m.From == userName && m._id == id && m.Type == MessageType.Portal);
		}

		public MessageQueue ReadMessage(string userName, string id)
		{
			var message =
				RepositoryContext.Current.GetOne<MessageQueue>(
					m => m.To == userName && m._id == id && m.Type == MessageType.Portal && m.IsSended);
			if (message != null)
			{
				message.IsReaded = true;
				PushMessage(message);
			}
			else
			{
				message = RepositoryContext.Current.GetOne<MessageQueue>(
					m => m.Cc.Contains(userName) && m._id == id && m.Type == MessageType.Portal && m.IsSended);
			}
			return message;
		}

		public IEnumerable<MessageQueue> Draft(string userName)
		{
			return
				RepositoryContext.Current.All<MessageQueue>(m => (m.From == userName) && !m.IsSended && m.Type == MessageType.Portal);
		}

		public void AddAppendix(string userName, string messageId, AdditionalInfo info)
		{
			MessageQueue message = Message(userName, messageId);
			if (message == null)
			{
				message = new MessageQueue();
				message._id = messageId;
				PushMessage(message);
			}

			if (message.Body == null)
			{
				message.Body = new MessageBody();
			}

			if (message.Body.Appendix == null)
			{
				message.Body.Appendix = new List<AdditionalInfo>();
			}

			message.Body.Appendix.Add(info);
			RepositoryContext.Current.Update(message);
		}

		public AdditionalInfo Appendix(string userName, string messageid, string appendixId)
		{
			return Message(userName, messageid).Body.Appendix.Find(a => a._id == appendixId);
		}
	}
}