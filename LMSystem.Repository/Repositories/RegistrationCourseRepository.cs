using LMSystem.Repository.Data;
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
    public class RegistrationCourseRepository : IRegistrationCourseRepository
    {
        private readonly LMOnlineSystemDbContext _context;

        public RegistrationCourseRepository(LMOnlineSystemDbContext context)
        {
            _context = context;
        }

        public async Task<ResponeModel> GetCompletedLearningCourseByAccountId(string accountId)
        {
            try
            {
                var completedCourseList = await _context.RegistrationCourses
                    .Where(r => r.AccountId == accountId
                            && r.IsCompleted == true)
                    .Select(r => new {
                        r.CourseId,
                        r.Course.Title,
                        r.Course.ImageUrl,
                        r.EnrollmentDate
                    })
                    .ToListAsync();

                if (completedCourseList == null)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No completed course were found for the specified account id"
                    };
                }

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "List completed course successfully",
                    DataObject = completedCourseList
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while get the completed course list" };
            }
        }

        public async Task<ResponeModel> GetRegisterCourseListByAccountId(string accountId)
        {
            try
            {
                var registerCourseList = await _context.RegistrationCourses
                    .Where(r => r.AccountId == accountId)
                    .ToListAsync();
                
                if (registerCourseList == null)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No register course were found for the specified account id"
                    };
                }

                return new ResponeModel { 
                    Status = "Success", 
                    Message = "List resgister course successfully", 
                    DataObject = registerCourseList 
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while get the register course list" };
                }
        }

        public async Task<bool> CheckRegistrationCourse(string accountId, int courseId)
        {
            var registrationExists = await _context.RegistrationCourses
        .AnyAsync(rc => rc.AccountId == accountId && rc.CourseId == courseId);

            return registrationExists;
        }


    }
}
