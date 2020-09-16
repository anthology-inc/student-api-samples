using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetODataWebClientOidc.Models;

namespace DotNetODataWebClientOidc.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAsync();

        Task<Course> GetAsync(int id);
    }
}
