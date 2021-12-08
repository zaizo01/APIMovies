using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationBirthDay
{
    public class JokeService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private const string ApiUrl =
            "https://localhost:44386/api/BirthDayPerson";

        public JokeService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<string> GetJokeAsync()
        {
            try
            {
                List<Person> model = null;
                var client = new HttpClient();
                var task = client.GetAsync(ApiUrl)
                  .ContinueWith((taskwithresponse) =>
                  {
                      var response = taskwithresponse.Result;
                      var jsonString = response.Content.ReadAsStringAsync();
                      jsonString.Wait();
                      model = JsonConvert.DeserializeObject<List<Person>>(jsonString.Result);

                      foreach (var person in model)
                      {

                          var today = DateTime.Today;
                          var birthDayPerson = person.BirthDay;
                          if (today.Date.Day == birthDayPerson.Date.Day &&
                              today.Date.Month == birthDayPerson.Date.Month)
                          {
                              Console.WriteLine($"{person.Name} is you bithDay");

                              var msg = new MimeMessage();
                              msg.From.Add(new MailboxAddress("BirthDay Notification", "notificationbirthday01@gmail.com"));
                              msg.To.Add(new MailboxAddress("Cristopher Zaiz", "e.zaizortega@gmail.com"));
                              msg.Subject = "BirthDay Notification Today";
                              //msg.Body = new TextPart("plain")
                              //{
                              //    Text = $"{person.Name}'s birthday today, send him a message."
                              //};

                              BodyBuilder builder = new BodyBuilder();
                              string text = string.Empty;

                              using (StreamReader reader = new StreamReader("TemplateEmail.html"))
                              {
                                  text = reader.ReadToEnd();
                              }

                              text = text.Replace("{personName}", person.Name);
                              builder.HtmlBody = text;
                              msg.Body = builder.ToMessageBody();

                              using (var client = new SmtpClient())
                              { 
                                client.Connect("smtp.gmail.com", 465, true);
                                client.Authenticate("notificationbirthday01@gmail.com", "notification*01");
                                client.Send(msg);
                                client.Disconnect(true);
                              }
                          }
                          else Console.WriteLine("No birth today.");
                      }

                  });
                task.Wait();

                return "";

            }
            catch (Exception ex)
            {
                return $"Error: {ex}";
            }
        }
    }
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
