using LMSystem.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface IRegistrationCourseRepository
    {
        public Task<ResponeModel> GetRegisterCourseListByAccountId(string accountId);
    }
}
