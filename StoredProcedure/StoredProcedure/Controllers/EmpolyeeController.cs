using Microsoft.AspNetCore.Mvc;
using StoredProcedure.Data;

namespace StoredProcedure.Controllers;

public class EmployeeController : Controller
{
    public StoredProcDbContext _context;
    public IConfiguration _configuration {get; }
    public EmployeeController
    (
        StoredProcDbContext context,
        IConfiguration configuration
    )
    {
        _context = context;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }

}