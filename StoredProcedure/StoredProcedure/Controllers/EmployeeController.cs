using Microsoft.AspNetCore.Mvc;
using StoredProcedure.Data;
using StoredProcedure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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

    public IEnumerable<Employee> SearchResult(string firstName = null, string lastName = null, string gender = null, int? salary = null)
    {
        var result = _context.Employees
            .FromSqlRaw("EXEC spSearchEmployees @FirstName, @LastName, @Gender, @Salary",
                new Microsoft.Data.SqlClient.SqlParameter("@FirstName", (object)firstName ?? DBNull.Value),
                new Microsoft.Data.SqlClient.SqlParameter("@LastName", (object)lastName ?? DBNull.Value),
                new Microsoft.Data.SqlClient.SqlParameter("@Gender", (object)gender ?? DBNull.Value),
                new Microsoft.Data.SqlClient.SqlParameter("@Salary", (object)salary ?? DBNull.Value))
            .ToList();
        return result;
    }
    [HttpGet]
    public IActionResult DynamicSQL(string firstName = null, string lastName = null, string gender = null, int? salary = null)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "dbo.spSearchEmployees";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            
            command.Parameters.AddWithValue("@FirstName", (object)firstName ?? DBNull.Value);
            command.Parameters.AddWithValue("@LastName", (object)lastName ?? DBNull.Value);
            command.Parameters.AddWithValue("@Gender", (object)gender ?? DBNull.Value);
            command.Parameters.AddWithValue("@Salary", (object)salary ?? DBNull.Value);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Employee> model = new List<Employee>();
            while (reader.Read())
            {
                var details = new Employee();
                details.FirstName = reader["FirstName"].ToString();
                details.LastName = reader["LastName"].ToString();
                details.Gender = reader["Gender"].ToString();
                details.Salary = Convert.ToInt32(reader["Salary"]);
                model.Add(details);
            }
            return View(model);
        }
    }
}