using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LMSystem.Repository.Data
{
    public class AuthenticationResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string JwtToken { get; set; }
        [Ignore]
        [IgnoreDataMember]
        [JsonIgnore]
        public Task<String>? VerifyEmailToken { get; set; }
        public DateTime? Expired { get; set; }
        public string JwtRefreshToken { get; set; }
    }
}
