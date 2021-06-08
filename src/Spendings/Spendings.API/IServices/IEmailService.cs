using SpendingsApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingsApi.IServices
{
    public interface IEmailService
    {
        void Send(Email email);
       string CreateHTMLTableAsync(List<Spendings> spendings);
    }

    public class EmailServiceDecorator : IEmailService
    {
        protected readonly IEmailService emailService;
        protected ISpendingsService spendingsService;

        public EmailServiceDecorator(IEmailService emailService, ISpendingsService service)
        {
            this.emailService = emailService;
            spendingsService = service;
        }

        public string CreateHTMLTableAsync(List<Spendings> spendings)
        {

            string stringHTML = $@"<!DOCTYPE html>
                <html>
                <head>
                <meta name='viewport' content='width=device-width, initial-scale=1'>
                <style>
                
                table {{
                border-collapse: collapse;
                border-spacing: 0;
                width: 100%;
                border: 1px solid #ddd;
                }}

                th, td {{
                text-align: left;
                padding: 16px;
                }}
            
                tr:nth-child(even) {{
                background-color: #f2f2f2;
                }}

                </style>
                </head>
            
                <body>

                <h2>Zestawienie Twoich kosztów</h2>
                <p>Oto lista Twoich kosztów za wynajem samochodów</p>

                <table style='border-collapse: collapse;
                border-spacing: 0;
                width: 100%;
                border: 1px solid #ddd;'>
                
                <tr>
                <th style='text-align: left;padding: 16px;'>Model samochodu</th>
                <th  style='text-align: left; padding: 16px;'>Koszt</th>
                <th style='text-align: left; padding: 16px;'>Data</th>
                <th style='text-align: left; padding: 16px;'>Cena</th>
                </tr>";

            foreach (var element in spendings)
            {
                var model = spendingsService.SetNames(element.CarID, element.CostID);

                stringHTML += $@"  <tr>
                                    <td style='text-align: left;padding: 16px;'>{model.Item1.Result}</td>
                                    <td style='text-align: left;padding: 16px;'>{model.Item2.Result}</td>
                                    <td style='text-align: left;padding: 16px;'>{element.Date}</td>
                                    <td style='text-align: left;padding: 16px;'>{element.Price}</td>
                                  </tr>";
            }

            stringHTML += $@"</table></body></html>";

            return stringHTML;
        }

        public void Send(Email email)
        {
            throw new NotImplementedException();
        }
    }
}
