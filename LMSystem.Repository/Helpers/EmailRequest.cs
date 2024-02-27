using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Helpers
{
    public class EmailRequest
    {
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public List<IFormFile>? AttachmentFilePaths { get; set; }
    }
}
