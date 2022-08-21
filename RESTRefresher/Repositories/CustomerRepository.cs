using System.Collections.Concurrent;
using RESTRefresher.Data;
using RESTRefresher.Models;

namespace RESTRefresher.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private static ConcurrentDictionary<string, Customer> customersCache;

    private ApplicationDbContext db;

    public Task<Customer?> CreateAsync(Customer c)
    {
        throw new NotImplementedException();
    }

    public Task<bool?> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Customer>> RetrieveAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> RetrieveAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Customer?> UpdateAsync(string id, Customer c)
    {
        throw new NotImplementedException();
    }
}