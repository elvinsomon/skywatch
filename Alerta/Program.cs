// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;


string origin = "skywatchalertingsystem@gmail.com";
string password = "pdkwrodsrklljqbl";

Console.Write("Ingrese el correo destino: ");
string destino = Console.ReadLine();

Console.WriteLine("Ingrese el nombre del cpu: ");
string cpuName = Console.ReadLine();

MailMessage oMailMessage = new MailMessage(origin, destino, "SkyWatch: ALERTA!", "El cpu " + cpuName + " a alcansado la metrica");

oMailMessage.IsBodyHtml = true;

SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
oSmtpClient.EnableSsl = true;
oSmtpClient.UseDefaultCredentials = false;
oSmtpClient.Host = ("smtp.gmail.com");
oSmtpClient.Port = 587;
oSmtpClient.Credentials = new System.Net.NetworkCredential(origin, password);

oSmtpClient.Send(oMailMessage);
oSmtpClient.Dispose();