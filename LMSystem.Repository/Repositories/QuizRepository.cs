using LMSystem.Repository.Data;
using LMSystem.Repository.Helpers;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly LMOnlineSystemDbContext _context;

        public QuizRepository(LMOnlineSystemDbContext context)
        {
            _context = context;
        }

        public async Task<ResponeModel> AddQuiz(AddQuizModel model)
        {
            try
            {
                var quiz = new Quiz
                {
                    Title = model.Title,
                    Description = model.Description
                };

                _context.Quizzes.Add(quiz);
                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Added quiz successfully", DataObject = quiz };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while adding the quiz" };
            }
        }

        public async Task<ResponeModel> DeleteQuiz(int quizId)
        {
            try
            {
                var quizToDelete = await _context.Quizzes
                    .Include(s => s.Steps)
                    .Include(q => q.Questions)
                    .FirstOrDefaultAsync(c => c.QuizId == quizId);

                if (quizToDelete == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Quiz not found" };
                }

                // Remove CourseCategory associations
                _context.Steps.RemoveRange(quizToDelete.Steps);
                _context.Questions.RemoveRange(quizToDelete.Questions);

                // Remove the category itself
                _context.Quizzes.Remove(quizToDelete);

                await _context.SaveChangesAsync();

                return new ResponeModel { Status = "Success", Message = "Delete quiz successfully" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while delete the quiz" };
            }
        }

        public async Task<PagedList<Quiz>> GetAllQuiz(PaginationParameter paginationParameter)
        {
            if (_context == null)
            {
                return null;
            }
            var quizzes = _context.Quizzes
                                .Include(q => q.Questions)
                                .ThenInclude(a => a.AnswerHistories)
                                .Include(s => s.Steps)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(paginationParameter.Search))
            {
                quizzes = quizzes.Where(o => o.Title.Contains(paginationParameter.Search));
            }

            var allQuizzes = await quizzes.ToListAsync();

            return PagedList<Quiz>.ToPagedList(allQuizzes,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<Quiz> GetQuizDetailByIdAsync(int quizId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(a => a.AnswerHistories)
                .FirstOrDefaultAsync(c => c.QuizId == quizId);

            return quiz;
        }

        public async Task<ResponeModel> UpdateQuiz(UpdateQuizModel quizModel)
        {
            try
            {
                var existingQuiz = await _context.Quizzes
                    .Include(q => q.Questions)
                    .FirstOrDefaultAsync(x => x.QuizId == quizModel.QuizId);

                if (existingQuiz == null)
                {
                    return new ResponeModel { Status = "Error", Message = "Quiz not found" };
                }

                existingQuiz.Title = quizModel.Title ?? existingQuiz.Title;
                existingQuiz.Description = quizModel.Description ?? existingQuiz.Description;

                // Process each question in the list
                foreach (var questionModel in quizModel.Questions)
                {
                    var existingQuestion = existingQuiz.Questions
                        .FirstOrDefault(q => q.QuestionTitle == questionModel.QuestionTitle);

                    if (existingQuestion != null)
                    {
                        // Update existing question
                        existingQuestion.Anwser = questionModel.Anwser ?? existingQuestion.Anwser;
                        existingQuestion.CorrectAnwser = questionModel.CorrectAnwser ?? existingQuestion.CorrectAnwser;
                    }
                    else
                    {
                        // Add new question if the title is not null
                        if (!string.IsNullOrEmpty(questionModel.QuestionTitle))
                        {
                            var newQuestion = new Question
                            {
                                QuestionTitle = questionModel.QuestionTitle,
                                Anwser = questionModel.Anwser,
                                CorrectAnwser = (int)questionModel.CorrectAnwser
                            };
                            existingQuiz.Questions.Add(newQuestion);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return new ResponeModel { Status = "Success", Message = "Quiz updated successfully", DataObject = existingQuiz };
            }
            catch (Exception ex)
            {
                // Handle exception
                return new ResponeModel { Status = "Error", Message = $"An error occurred: {ex.Message}" };
            }
        }

    }
}
