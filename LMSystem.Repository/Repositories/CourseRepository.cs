﻿using LMSystem.Repository.Data;
using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Repository.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        // Assuming you have a DbContext or similar for data access
        private readonly LMOnlineSystemDbContext _context;

        public CourseRepository(LMOnlineSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Course> GetCourseDetailByIdAsync(int courseId)
        {
            var course = await _context.Courses
            .Include(c => c.Sections)
            .ThenInclude(s => s.Steps)
            .FirstOrDefaultAsync(c => c.CourseId == courseId);

            return course;
        }

        public async Task<IEnumerable<Course>> GetCoursesWithFilters(CourseFilterParameters filterParams)
        {
            var query = _context.Courses.AsQueryable();

            if (filterParams.CategoryIds != null && filterParams.CategoryIds.Any())
            {
                query = query.Where(c => c.CourseCategories.Any(cc => filterParams.CategoryIds.Equals(cc.CategoryId)));
            }

            if (filterParams.MinPrice.HasValue)
            {
                query = query.Where(c => c.Price >= filterParams.MinPrice.Value);
            }

            if (filterParams.MaxPrice.HasValue)
            {
                query = query.Where(c => c.Price <= filterParams.MaxPrice.Value);
            }

            // Apply pagination
            return await query
                .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetTopCoursesByStudentJoined(int numberOfCourses)
        {
            var topCourses = await _context.RegistrationCourses
                .GroupBy(rc => rc.CourseId)
                .OrderByDescending(g => g.Count())
                .Take(numberOfCourses)
                .Select(g => g.Key)
                .ToListAsync();

            var courses = await _context.Courses
                .Where(c => topCourses.Contains(c.CourseId))
                .ToListAsync();

            // Fill with random courses if needed
            if (courses.Count < numberOfCourses)
            {
                var additionalCourses = await _context.Courses
                    .Where(c => !topCourses.Contains(c.CourseId))
                    .OrderBy(c => Guid.NewGuid())
                    .Take(numberOfCourses - courses.Count)
                    .ToListAsync();

                courses.AddRange(additionalCourses);
            }

            return courses;
        }

        public async Task<IEnumerable<Course>> GetTopCoursesBySales(int numberOfCourses)
        {
            var topCourses = await _context.Orders
                .GroupBy(o => o.CourseId)
                .OrderByDescending(g => g.Count())
                .Take(numberOfCourses)
                .Select(g => g.Key)
                .ToListAsync();

            var courses = await _context.Courses
                .Where(c => topCourses.Contains(c.CourseId))
                .ToListAsync();

            if (courses.Count < numberOfCourses)
            {
                var additionalCourses = await _context.Courses
                    .Where(c => !topCourses.Contains(c.CourseId))
                    .OrderBy(c => Guid.NewGuid())
                    .Take(numberOfCourses - courses.Count)
                    .ToListAsync();

                courses.AddRange(additionalCourses);
            }

            return courses;
        }

        public async Task<IEnumerable<Course>> GetTopCoursesByRating(int numberOfCourses)
        {
            var topCourseIds = await _context.RatingCourses
                .Include(rc => rc.Registration) // Include Registration navigation property
                .ThenInclude(reg => reg.Course) // Then include Course navigation property from Registration
                .GroupBy(rc => rc.Registration.CourseId) // Group by CourseId from RegistrationCourse
                .Select(group => new { CourseId = group.Key, AverageRating = group.Average(rc => rc.RatingStar) }) // Assuming the property is named RatingValue
                .OrderByDescending(x => x.AverageRating)
                .Take(numberOfCourses)
                .Select(x => x.CourseId)
                .ToListAsync();

            var courses = await _context.Courses
                .Where(c => topCourseIds.Contains(c.CourseId))
                .ToListAsync();

            if (courses.Count < numberOfCourses)
            {
                var additionalCourses = await _context.Courses
                    .Where(c => !topCourseIds.Contains(c.CourseId))
                    .OrderBy(c => Guid.NewGuid())
                    .Take(numberOfCourses - courses.Count)
                    .ToListAsync();

                courses.AddRange(additionalCourses);
            }

            return courses;
        }

    }


}