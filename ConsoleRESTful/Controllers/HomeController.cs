using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleRESTful.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ConsoleRESTful.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [FormatFilter]
    public class HomeController : Controller
    {
        private readonly ApplicationContext _db;
        public HomeController(ApplicationContext context)
        {
            _db = context;
            /*List<Project> projects = new List<Project>()
            {
                new Project
                {
                    Name = "MobileStore",
                    Filepath = "C:/Users/User/source/repos/MobileStore",
                    ProjectType = "MVC"
                },
                new Project
                {
                    Name = "Haikyuu",
                    Filepath = "C:/Users/User/source/repos/Haikyuu",
                    ProjectType = "MVC"
                },
                new Project
                {
                    Name = "ConsoleRESTful",
                    Filepath = "C:/Users/User/source/repos/ConsoleRESTful",
                    ProjectType = "REST API"
                }
            };

            _db.AddRange(projects);
            _db.SaveChanges();*/
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> Get()
        {
            return await _db.Projects.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> Get(int id)
        {
            Project project = await _db.Projects.FirstOrDefaultAsync(i => i.Id == id);
            if (project == null)
                return NotFound();

            return new ObjectResult(project);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> Post(Project project)
        {
            if (project == null)
                return BadRequest();

            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            return Ok(project);
        }

        [HttpPut]
        public async Task<ActionResult<Project>> Put(Project project)
        {
            if (project == null)
                return BadRequest();
            if (!_db.Projects.Any(i => i.Id == project.Id))
                return NotFound();

            _db.Projects.Update(project);
            await _db.SaveChangesAsync();
            return Ok(project);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> Delete(int id)
        {
            Project project = _db.Projects.FirstOrDefault(i => i.Id == id);
            if (project == null)
                return NotFound();

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
            return Ok(project);
        }
    }
}
