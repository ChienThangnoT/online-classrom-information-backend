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
                            && r.IsCompleted==true)
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

        public async Task<ResponeModel> GetRegisterCourseListByParentAccountId(string accountId)
        {
            try
            {
                // First, get the parent email associated with the given accountId.
                var parentEmail = await _context.Account
                    .Where(a => a.Id == accountId)
                    .Select(a => a.Email)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(parentEmail))
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No parent email associated with the provided account ID."
                    };
                }

                // Now find all accounts where the ParentEmail matches the account's email.
                var childAccountIds = await _context.Account
                    .Where(a => a.ParentEmail == parentEmail)
                    .Select(a => a.Id)
                    .ToListAsync();

                // Retrieve the list of courses registered by the child accounts.
                var registerCourseList = await _context.RegistrationCourses
                    .Where(rc => childAccountIds.Contains(rc.AccountId))
                    .Include(rc => rc.Course)
                    .Select(rc => new
                    {
                        RegistrationId = rc.RegistrationId,
                        CourseId = rc.CourseId,
                        AccountId = rc.AccountId,
                        EnrollmentDate = rc.EnrollmentDate,
                        IsCompleted = rc.IsCompleted,
                        LearningProgress = rc.LearningProgress,
                        CourseTitle = rc.Course.Title,
                        CourseDescription = rc.Course.Description,
                        CourseImg = rc.Course.ImageUrl
                    })
                    .ToListAsync();

                if (!registerCourseList.Any())
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No registered courses found for the children of the provided account ID."
                    };
                }

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "List of registered courses for the children of the provided account ID retrieved successfully.",
                    DataObject = registerCourseList
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while retrieving the registered course list." };
            }
        }

        public async Task<ResponeModel> GetCompletedLearningCourseByParentAccountId(string accountId)
        {
            try
            {
                // First, get the parent email associated with the given accountId.
                var parentEmail = await _context.Account
                    .Where(a => a.Id == accountId)
                    .Select(a => a.Email)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(parentEmail))
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No parent email associated with the provided account ID."
                    };
                }

                // Now find all accounts where the ParentEmail matches the account's email.
                var childAccountIds = await _context.Account
                    .Where(a => a.ParentEmail == parentEmail)
                    .Select(a => a.Id)
                    .ToListAsync();

                // Retrieve the list of courses registered by the child accounts.
                var completedCourseList = await _context.RegistrationCourses
                    .Where(rc => childAccountIds.Contains(rc.AccountId) && rc.IsCompleted == true)
                    .Include(rc => rc.Course)
                    .Select(rc => new
                    {
                        rc.CourseId,
                        rc.Course.Title,
                        rc.Course.ImageUrl,
                        rc.EnrollmentDate
                    })
                    .ToListAsync();

                if (!completedCourseList.Any())
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No completed course were found for the specified account id."
                    };
                }

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "List completed course successfully.",
                    DataObject = completedCourseList
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while get the completed course list" };
            }
        }

        public async Task<ResponeModel> GetUncompletedLearningCourseByParentAccountId(string accountId)
        {
            try
            {
                // First, get the parent email associated with the given accountId.
                var parentEmail = await _context.Account
                    .Where(a => a.Id == accountId)
                    .Select(a => a.Email)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(parentEmail))
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No parent email associated with the provided account ID."
                    };
                }

                // Now find all accounts where the ParentEmail matches the account's email.
                var childAccountIds = await _context.Account
                    .Where(a => a.ParentEmail == parentEmail)
                    .Select(a => a.Id)
                    .ToListAsync();

                // Retrieve the list of courses registered by the child accounts.
                var uncompletedCourseList = await _context.RegistrationCourses
                    .Where(rc => childAccountIds.Contains(rc.AccountId) && rc.IsCompleted == false)
                    .Include(rc => rc.Course)
                    .Select(rc => new
                    {
                        rc.CourseId,
                        rc.Course.Title,
                        rc.Course.ImageUrl,
                        rc.LearningProgress,
                        rc.EnrollmentDate
                    })
                    .ToListAsync();

                if (!uncompletedCourseList.Any())
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No uncompleted course were found for the specified account id."
                    };
                }

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "List uncompleted course successfully.",
                    DataObject = uncompletedCourseList
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while get the uncompleted course list" };
            }
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
                        CourseTitle = r.Course.Title, 
                        CourseDescription = r.Course.Description, 
                        CourseImg = r.Course.ImageUrl
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

        public async Task<ResponeModel> CheckRegistrationCourse(string accountId, int courseId)
        {
            try
            {
                var registrationCourse = await _context.RegistrationCourses
                    .Where(r => r.AccountId == accountId && r.CourseId == courseId)
                    .Select(r => new
                    {
                        r.RegistrationId,
                        r.CourseId,
                        r.AccountId,
                        r.EnrollmentDate,
                        r.IsCompleted,
                        r.LearningProgress
                    })
                    .FirstOrDefaultAsync();

                if (registrationCourse == null)
                {
                    return new ResponeModel
                    {
                        Status = "Error",
                        Message = "No registration course were found for the specified account id and course id"
                    };
                }

                return new ResponeModel
                {
                    Status = "Success",
                    Message = "Check registration course successfully",
                    DataObject = registrationCourse
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ResponeModel { Status = "Error", Message = "An error occurred while check registration course" };
            }
        }


    }
}
