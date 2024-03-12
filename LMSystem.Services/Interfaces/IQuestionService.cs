using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Interfaces
{
    public interface IQuestionService
   {
        public Task<ResponeModel> AddQuestion(AddQuestionModel model);
        public Task<ResponeModel> DeleteQuestion(int questionId);
        public Task<PagedList<Question>> GetAllQuestion(PaginationParameter paginationParameter);
        public Task<ResponeModel> UpdateQuestion(UpdateQuestionModel model);
    }
}
