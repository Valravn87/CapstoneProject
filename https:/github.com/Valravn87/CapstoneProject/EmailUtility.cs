using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace CapstoneProject
{
	public class EmailUtility
	{

		public static void sendMail(String Toemail, String body, String subject)
		{
			// Command line argument must the the SMTP host.
			SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
			client.UseDefaultCredentials = true;
			client.Credentials = new System.Net.NetworkCredential("costanza1349@gmail.com", "Se1nf3ld!");

			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			client.EnableSsl = true;
			MailAddress from = new MailAddress("costanza1349@gmail.com");
			MailAddress to = new MailAddress(Toemail);
			MailMessage message = new MailMessage(from, to);
			message.Bcc.Add("ca11887@hotmail.com");
			message.Subject = subject;
			message.Body = body;

			message.BodyEncoding = System.Text.Encoding.UTF8;
			client.Send(message);

			message.Dispose();
			Console.WriteLine("Goodbye.");
		}
	}
}