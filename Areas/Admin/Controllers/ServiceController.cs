using Maxim.Areas.Admin.ViewModels;
using Maxim.DAL;
using Maxim.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Maxim.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Service> services = await _context.services.ToListAsync();
            return View(services);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Service service = new Service()
            {
                Icon = vm.Icon,
                Title = vm.Title,
                Description = vm.Description,
            };
            await _context.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Service");
        }
        public async Task<IActionResult> Update(int id)
        {
            Service service = await _context.services.Where(c=>c.Id == id).FirstOrDefaultAsync();
            if (service == null) { return View(); }
            UpdateServiceVm vm = new UpdateServiceVm()
            {
                Id = service.Id,
                Title = service.Title,
                Icon = service.Icon,
                Description = service.Description,
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateServiceVm vm)
        {
            Service exist = await _context.services.Where(c => c.Id == vm.Id).FirstOrDefaultAsync();
            if(exist == null) { return View(); }
            exist.Title = vm.Title;
            exist.Description = vm.Description;
            exist.Icon = vm.Icon;
            await _context.SaveChangesAsync();  
            return RedirectToAction("Index","Service");
        }
        public async Task<IActionResult> Delete(int id)
        {
            Service service = await _context.services.Where(c=>c.Id == id).FirstOrDefaultAsync();
            if(service == null) { return View(); }
            _context.services.Remove(service);
            _context.SaveChangesAsync();
            return RedirectToAction("Index","Service");
        }
    }
}
