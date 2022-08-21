using Microsoft.EntityFrameworkCore;

namespace RESTRefresher.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    {
        
    }
}