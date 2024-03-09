using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Interfaces
{
    public interface IQuizRepository
    {
        public Task<ResponeModel> AddQuiz(AddQuizModel model);
        public Task<ResponeModel> DeleteQuiz(int quizId);
        public Task<PagedList<Quiz>> GetAllQuiz(PaginationParameter paginationParameter);
        public Task<Quiz> GetQuizDetailByIdAsync(int quizId);
        public Task<ResponeModel> UpdateQuiz(UpdateQuizModel quizModel);


    }
}
