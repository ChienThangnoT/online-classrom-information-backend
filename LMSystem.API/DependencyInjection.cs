using LMSystem.Repository.Interfaces;
using LMSystem.Repository.Repositories;
using LMSystem.Services.Interfaces;
using LMSystem.Services.Services;

namespace LMSystem.API
{
    static class DependencyInjection
    {
         public static IServiceCollection AddApiWebService(this IServiceCollection services)
        {
            //Add Dependenci Injection, Life cycle DI: AddSingleton(), AddTransisent(), AddScoped()
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseService, CourseService>();

            services.AddScoped<IWishListRepository, WishListRepository>();
            services.AddScoped<IWishListService, WishListService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<ISectionService, SectionService>();

            services.AddScoped<IStepRepository, StepRepository>();
            services.AddScoped<IStepService, StepService>();

            return services;
        }
    }
}
