using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Threading.Tasks;
using DotNetODataWebClientOidc.Services;

namespace DotNetODataWebClientOidc.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: Courses
        [AuthorizeForScopes(ScopeKeySection = "CampusNexus:CampusNexusScope")]
        public async Task<ActionResult> Index()
        {
            return View(await _courseService.GetAsync());
        }

        // GET: Courses/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await _courseService.GetAsync(id));
        }
    }
}