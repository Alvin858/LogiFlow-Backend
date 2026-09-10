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

    // Member 2 - Shipment Management
    DbSet<Shipment> Shipments { get; }
    DbSet<ShipmentItem> ShipmentItems { get; }
    DbSet<ShipmentTracking> ShipmentTracking { get; }

    // Member 2 - Warehouse Management
    DbSet<Warehouse> Warehouses { get; }
    DbSet<WarehouseShipment> WarehouseShipments { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
