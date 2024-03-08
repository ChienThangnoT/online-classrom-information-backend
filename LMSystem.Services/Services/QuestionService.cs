using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
        public async Task<ResponeModel> AddQuestion(AddQuestionModel model)
        {
            return await _questionRepository.AddQuestion(model);
        }

        public async Task<ResponeModel> DeleteQuestion(int questionId)
        {
            return await _questionRepository.DeleteQuestion(questionId);
        }

        public async Task<PagedList<Question>> GetAllQuestion(PaginationParameter paginationParameter)
        {
            return await _questionRepository.GetAllQuestion(paginationParameter);
        }

        public async Task<ResponeModel> UpdateQuestion(UpdateQuestionModel model)
        {
            return await _questionRepository.UpdateQuestion(model);
        }
    }
}
