namespace LMSystem.Services.Interfaces
{
    public interface IEmailTemplateReader
    {
        Task<string> GetTemplate(string template);
    }
}