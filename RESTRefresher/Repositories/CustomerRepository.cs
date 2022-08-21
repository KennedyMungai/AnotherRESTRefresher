using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RESTRefresher.Data;
using RESTRefresher.Models;

namespace RESTRefresher.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private static ConcurrentDictionary<string, Customer>? customersCache;

    private ApplicationDbContext? db;

    public CustomerRepository(ApplicationDbContext applicationDbContext)
    {
        db = applicationDbContext;

        if (customersCache is null)
        {
            customersCache = new ConcurrentDictionary<string, Customer>(
                db.Customers.ToDictionary(c => c.CustomersId)
            );
        }
    }

    public async Task<Customer?> CreateAsync(Customer c)
    {
        c.CustomersId = c.CustomersId.ToUpper();

        EntityEntry<Customer> added = await db.Customers.AddAsync(c);
        int affected = await db.SaveChangesAsync();
        if (affected == 1)
        {
            if (customersCache is null)
            {
                return c;
            }

            return customersCache.AddOrUpdate(c.CustomerId, c, UpdateCache);
        }
        else
        {
            return null;
        }
    }

    public Task<bool?> DeleteAsync(string id)
    {
    }

    public Task<IEnumerable<Customer>> RetrieveAllAsync()
    {
        return Task.FromResult(customersCache is null ? Enumerable.Empty<Customer>() : customersCache.Values);
    }

    public Task<Customer?> RetrieveAsync(string id)
    {
        id = id.ToUpper();

        if(customersCache is null)
        {
            return null;
        }

        customersCache.TryGetValue(id, out Customer? c);

        return Task.FromResult(c);
    }

    public Task<Customer?> UpdateAsync(string id, Customer c)
    {
        throw new NotImplementedException();
    }

    public Customer UpdateCache(string id, Customer c)
    {
        Customer? old;

        if (customersCache is not null)
        {
            if (customersCache.TryGetValue(id, out old))
            {
                if (customersCache.TryUpdate(id, c, old))
                {
                    return c;
                }
            }
        }

        return null!;
    }
}