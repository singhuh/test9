﻿using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test9.Services
{
    public class MailjetEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailjetOptions _mailjetOptions;
        public MailjetEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _mailjetOptions = _configuration.GetSection("MailJet").Get<MailjetOptions>();

            MailjetClient client = new MailjetClient(_mailjetOptions.ApiKey, _mailjetOptions.SecretKey);
            {
                var Version = ApiVersion.V3_1;
            };

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "aspnetproject@protonmail.com"},
        {"Name", "Aa"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email}, 
         { "Name", "Aa" }
        }
       }
      }, {
       "Subject",
       subject
      }, {
       "HTMLPart",
       htmlMessage
      },
     }
             });
           var response= await client.PostAsync(request);
            //if (response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
            //    Console.WriteLine(response.GetData());
            //}
            //else
            //{
            //    Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
            //    Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
            //    Console.WriteLine(response.GetData());
            //    Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            //}
        }
    }
}

