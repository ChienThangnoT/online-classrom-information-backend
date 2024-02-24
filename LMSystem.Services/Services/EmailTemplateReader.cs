using LMSystem.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class EmailTemplateReader : IEmailTemplateReader
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmailTemplateReader(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        //get template email
        // param template is name of file template
        public async Task<string> GetTemplate(string template)
        {
            string templateEmail = Path.Combine(_webHostEnvironment.ContentRootPath, template);

            string content = await File.ReadAllTextAsync(templateEmail);

            return content;
        }
    }
}
