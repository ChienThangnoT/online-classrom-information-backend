using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LMSystem.Repository.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        // Assuming you have a DbContext or similar for data access
        private readonly LMOnlineSystemDbContext _context;

        public QuestionRepository(LMOnlineSystemDbContext context)
        {
            _context = context;
        }

        public async Task<ResponeModel> AddQuestion(AddQuestionModel model)
        {
            try
            {
                var question = new Question
                {
                    QuizId = model.QuizId,
                    QuestionTitle = model.QuestionTitle,
                    CorrectAnwser = model.CorrectAnwser,
                    Anwser = model.Anwser
                };

                _context.Questions.Add(question);
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Added question successfully", DataObject = question };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while adding the question" };
            }
        }

        public async Task<ResponeModel> DeleteQuestion(int questionId)
        {
            try
            {
                var questionToDelete = await _context.Questions
                    .Include(c => c.AnswerHistories)
                    .FirstOrDefaultAsync(c => c.QuestionId == questionId);

                if (questionToDelete == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Category not found" };
                }

                // Remove CourseCategory associations
                _context.AnswerHistories.RemoveRange(questionToDelete.AnswerHistories);

                // Remove the category itself
                _context.Questions.Remove(questionToDelete);

                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Delete question successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while delete the question" };
            }
        }

        public async Task<PagedList<Question>> GetAllQuestion(PaginationParameter paginationParameter)
        {
            if (_context == null)
            {
                return null;
            }
            var questions = _context.Questions
                                    .Include (c => c.AnswerHistories)
                                    .AsQueryable();

            if (!string.IsNullOrEmpty(paginationParameter.Search))
            {
                questions = questions.Where(o => o.QuestionTitle.Contains(paginationParameter.Search));
            }

            var allQuestions = await questions.ToListAsync();

            return PagedList<Question>.ToPagedList(allQuestions,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<ResponeModel> UpdateQuestion(UpdateQuestionModel model)
        {
            try
            {
                var existingQuestion = await _context.Questions.FirstOrDefaultAsync(x => x.QuestionId == model.QuestionId);
                if (existingQuestion == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Question not found" };
                }

                if (model.QuestionTitle != null)
                {
                    existingQuestion.QuestionTitle = model.QuestionTitle;

                }
                else
                {
                    existingQuestion.QuestionTitle = existingQuestion.QuestionTitle;
                }

                if (model.CorrectAnwser != null)
                {
                    existingQuestion.CorrectAnwser = (int)model.CorrectAnwser;
                }
                else
                {
                    existingQuestion.CorrectAnwser = existingQuestion.CorrectAnwser;
                }

                if (model.Anwser != null)
                {
                    existingQuestion.Anwser = model.Anwser;
                }
                else
                {
                    existingQuestion.Anwser = existingQuestion.Anwser;
                }


                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Update question successfully", DataObject = existingQuestion };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while update the question" };
            }
        }
    }
}
