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
        public async Task<ResponeModel> GetRegisterCourseListByAccountId(string accountId)
        {
            try
            {
                // Include the Course navigation property to access course details.
                var registerCourseList = await _context.RegistrationCourses
                    .Where(r => r.AccountId == accountId)
                    .Include(r => r.Course) // Eagerly load the Course details
                    .Select(r => new
                    {
                        RegistrationId = r.RegistrationId,
                        CourseId = r.CourseId,
                        AccountId = r.AccountId,
                        EnrollmentDate = r.EnrollmentDate,
                        IsCompleted = r.IsCompleted,
                        LearningProgress = r.LearningProgress,
                        // You can add more Course details as needed
                        CourseTitle = r.Course.Title, // Assuming Course has a Title property
                        CourseDescription = r.Course.Description // Assuming Course has a Description property
                    })
                    .ToListAsync();

                if (!registerCourseList.Any())
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No register course were found for the specified account id"
                    };
                }

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "List register course successfully",
                    DataObject = registerCourseList
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while getting the register course list" };
            }
        }

    }
}
