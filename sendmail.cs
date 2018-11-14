using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

// v2.0: ported from vbscript and expanded functionality

namespace sendmail {
	class sendmail {
		static string server;
		static string from, to, subject, body;
		static string attachFile, attachType;

		static void Send(string host, int port, string from, string to, string subject, string body, string attachFile, string attachType) {
			SmtpClient client = new SmtpClient(host, port);
			char[] delimChars = { '<', '>' };
			MailAddress addrFrom = null, addrTo = null;
			string[] addrs;

			addrs = from.Split(delimChars, StringSplitOptions.RemoveEmptyEntries);
			if (addrs.Count() == 1) {
				addrFrom = new MailAddress(addrs[0], null, System.Text.Encoding.UTF8);
			} else if (addrs.Count() == 2) {
				addrFrom = new MailAddress(addrs[1], addrs[0], System.Text.Encoding.UTF8);
			} else {
				Console.Write("Error: Malformed from: address");
				Environment.Exit(1);
			}
			addrs = to.Split(delimChars, StringSplitOptions.RemoveEmptyEntries);
			if (addrs.Count() == 1) {
				addrTo = new MailAddress(addrs[0], null, System.Text.Encoding.UTF8);
			} else if (addrs.Count() == 2) {
				addrTo = new MailAddress(addrs[1], addrs[0], System.Text.Encoding.UTF8);
			} else {
				Console.Write("Error: Malformed to: address");
				Environment.Exit(1);
			}
			MailMessage message = new MailMessage(addrFrom, addrTo);
			//			client.Credentials = new NetworkCredential("admin@ikkokkan.com","K@nr1n1n");
			client.ServicePoint.MaxIdleTime = 2;
			message.Subject = subject;
			message.SubjectEncoding = System.Text.Encoding.UTF8;
			message.Body = body;
			message.BodyEncoding = System.Text.Encoding.UTF8;
			if ((attachFile != "") && (attachType != "")) {
				try {
					message.Attachments.Add(new Attachment(attachFile, attachType));
				} catch (Exception aex) {
					Console.WriteLine(aex.Message);
				}
			}
			try {
				client.Send(message);
			} catch (ArgumentNullException ex) {
				Console.WriteLine(ex.Message);
			} catch (ObjectDisposedException ex) {
				Console.WriteLine(ex.Message);
			} catch (SmtpFailedRecipientsException ex) {
				Console.WriteLine(ex.Message);
			} catch (SmtpException ex) {
				Console.WriteLine(ex.Message);
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
			client.Dispose();
			message.Dispose();
		}

		static void Main(string[] args) {
			bool errorInput;
			int c;

			errorInput = false;
			server = ""; from = ""; to = "";  subject = ""; body = "";
			attachFile = "";
			for (c = 0; c < args.Length; c++) {
				if (args[c] == "-server") {
					server = args[c + 1];
					c++;
				} else if (args[c] == "-from") {
					from = args[c + 1];
					c++;
				} else if (args[c] == "-to") {
					to = args[c + 1];
					c++;
				} else if (args[c] == "-subject") {
					subject = args[c + 1];
					c++;
				} else if (args[c] == "-body") {
					body = args[c + 1];
					c++;
				} else if (args[c] == "-attach") {
					attachFile = args[c + 1];
					c++;
				} else if (args[c] == "-type") {
					attachType = args[c + 1];
					c++;
				}
			}

			if (server == "") errorInput = true;
			if (from == "") errorInput = true;
			if (to == "") errorInput = true;
			if (subject == "") errorInput = true;

			if (errorInput) {
				Console.WriteLine("sendmail v2.0 - (C)2010-2018 Y0tsuya");
				Console.WriteLine("senmail -server [server] -from [sender] -to [recipient] -subject [subject] -body [message] -attach [attachment] -type [MIME]");
				Console.WriteLine("-server: SMTP server");
				Console.WriteLine("-from: sender email");
				Console.WriteLine("-to: recipient email");
				Console.WriteLine("-subject: email subject");
				Console.WriteLine("-body: optional message body");
				Console.WriteLine("-attach: optional file attachment");
				Console.WriteLine("-type: file attachment MIME type");
				Environment.Exit(1);
			}

			Send(server, 25, from, to, subject, body, attachFile, attachType);
		}
	}
}
