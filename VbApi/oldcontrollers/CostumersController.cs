using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vb.Api.olldcontroller;

[Route("api/[controller]")]
[ApiController]
[NonController]
public class CostumersController : ControllerBase
{
    private readonly VbDbContext dbContext;

    public CostumersController(VbDbContext dbContext)
    {
        this.dbContext = dbContext;
    }



    // GET: api/<CostumerController>
    [HttpGet]
    public async Task<List<Customer>> Get()
    {
        return await dbContext.Set<Customer>()
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<Customer> Get(int id)
    {
        var customer = await dbContext.Set<Customer>()
            .Include(x => x.Accounts)
            .Include(x => x.Addresses)
            .Include(x => x.Contacts)
            .Where(x => x.CustomerNumber == id).FirstOrDefaultAsync();

        return customer;
    }

    [HttpPost]
    public async Task Post([FromBody] Customer customer)
    {
        await dbContext.Set<Customer>().AddAsync(customer);
        await dbContext.SaveChangesAsync();
    }

    [HttpPut("{id}")]
    public async Task Put(int id, [FromBody] Customer customer)
    {
        var fromdb = await dbContext.Set<Customer>().Where(x => x.CustomerNumber == id).FirstOrDefaultAsync();
        fromdb.FirstName = customer.FirstName;
        fromdb.LastName = customer.LastName;
        await dbContext.SaveChangesAsync();
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        var fromdb = await dbContext.Set<Customer>().Where(x => x.CustomerNumber == id).FirstOrDefaultAsync();
        fromdb.IsActive = false;
        await dbContext.SaveChangesAsync();
    }
}
