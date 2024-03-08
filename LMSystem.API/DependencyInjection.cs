using FirebaseAdmin.Messaging;
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

            services.AddScoped<IEmailTemplateReader, EmailTemplateReader>();

            services.AddScoped<IMailService, MailService>();
            services.AddTransient<IMailService, MailService>();

            services.AddScoped<IRatingCourseRepository, RatingCourseRepository>();
            services.AddScoped<IRatingCourseService, RatingCourseService>();

            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<ISectionService, SectionService>();

            services.AddScoped<IStepRepository, StepRepository>();
            services.AddScoped<IStepService, StepService>();

            services.AddScoped<IRegistrationCourseRepository, RegistrationCourseRepository>();
            services.AddScoped<IRegistrationCourseService, RegistrationCourseService>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IReportProblemRepository, ReportProblemRepository>();
            services.AddScoped<IReportProblemService, ReportProblemService>();

            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuestionService, QuestionService>();

            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IQuizService, QuizService>();

            services.AddScoped<IFirebaseRepository, FirebaseRepository>();


            return services;
        }
    }
}
