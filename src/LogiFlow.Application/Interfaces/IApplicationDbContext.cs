using LogiFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogiFlow.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Role> Roles { get; }
    DbSet<User> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Customer> Customers { get; }
    DbSet<CustomerAddress> CustomerAddresses { get; }
    DbSet<Vehicle> Vehicles { get; }
    DbSet<Driver> Drivers { get; }

    DbSet<Route> Routes { get; }
    DbSet<RouteStop> RouteStops { get; }
    DbSet<Delivery> Deliveries { get; }
    DbSet<ProofOfDelivery> ProofsOfDelivery { get; }
    DbSet<Schedule> Schedules { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
