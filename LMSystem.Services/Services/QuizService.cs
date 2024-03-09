using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        public QuizService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<ResponeModel> AddQuiz(AddQuizModel model)
        {
            return await _quizRepository.AddQuiz(model);
        }

        public async Task<ResponeModel> DeleteQuiz(int quizId)
        {
            return await _quizRepository.DeleteQuiz(quizId);
        }

        public async Task<PagedList<Quiz>> GetAllQuiz(PaginationParameter paginationParameter)
        {
            return await _quizRepository.GetAllQuiz(paginationParameter);
        }
        public async Task<Quiz> GetQuizDetailByIdAsync(int quizId)
        {
            return await _quizRepository.GetQuizDetailByIdAsync(quizId);
        }

        public async Task<ResponeModel> UpdateQuiz(UpdateQuizModel quizModel)
        {
            return await _quizRepository.UpdateQuiz(quizModel);
        }
    }
}
