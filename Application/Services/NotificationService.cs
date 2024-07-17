using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class NotificationService
	{
		private readonly ISendNotification _notificationSender;
		public NotificationService(ISendNotification notificationSender) 
		{
			_notificationSender = notificationSender;
		}
		public async Task<bool> NotifyTeacher(string email, string notificationMessage)
		{
			string body = "EduTrack - Empower your learning, Achieve your goals!\n\n";
			body += notificationMessage;
			body += "\n\n\tThank you, \n\tEduTrack Team!";
			return await _notificationSender.SendNotificationTo(email, body);
		}
	}
}
