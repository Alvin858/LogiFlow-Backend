using LogiFlow.Application.Interfaces;
using LogiFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogiFlow.Infrastructure.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Driver> Drivers => Set<Driver>();

    // Member 3
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<RouteStop> RouteStops => Set<RouteStop>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<ProofOfDelivery> ProofsOfDelivery => Set<ProofOfDelivery>();
    public DbSet<Schedule> Schedules => Set<Schedule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ==========================================
        // ROLE
        // ==========================================
        modelBuilder.Entity<Role>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            e.HasIndex(x => x.Name)
                .IsUnique();
        });

        // ==========================================
        // USER
        // ==========================================
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Email)
                .HasMaxLength(256)
                .IsRequired();

            e.HasIndex(x => x.Email)
                .IsUnique();

            e.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.PasswordHash)
                .IsRequired();

            e.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ==========================================
        // REFRESH TOKEN
        // ==========================================
        modelBuilder.Entity<RefreshToken>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.TokenHash)
                .HasMaxLength(128)
                .IsRequired();

            e.HasIndex(x => x.TokenHash)
                .IsUnique();

            e.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==========================================
        // CUSTOMER
        // ==========================================
        modelBuilder.Entity<Customer>(e =>
        {
            e.HasKey(x => x.Id);

            e.HasIndex(x => x.UserId)
                .IsUnique();

            e.Property(x => x.CompanyName)
                .HasMaxLength(200)
                .IsRequired();

            e.Property(x => x.ContactPerson)
                .HasMaxLength(200)
                .IsRequired();

            e.Property(x => x.TaxNumber)
                .HasMaxLength(50);

            e.HasOne(x => x.User)
                .WithOne(x => x.Customer)
                .HasForeignKey<Customer>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==========================================
        // CUSTOMER ADDRESS
        // ==========================================
        modelBuilder.Entity<CustomerAddress>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.AddressLine1)
                .HasMaxLength(250)
                .IsRequired();

            e.Property(x => x.AddressLine2)
                .HasMaxLength(250);

            e.Property(x => x.City)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.State)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.PostalCode)
                .HasMaxLength(20)
                .IsRequired();

            e.Property(x => x.Country)
                .HasMaxLength(100)
                .IsRequired();

            e.HasIndex(x => new
            {
                x.CustomerId,
                x.IsDefault
            });

            e.HasOne(x => x.Customer)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ==========================================
        // VEHICLE
        // ==========================================
        modelBuilder.Entity<Vehicle>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.VehicleType)
                .HasMaxLength(100)
                .IsRequired();

            e.Property(x => x.RegistrationNumber)
                .HasMaxLength(30)
                .IsRequired();

            e.HasIndex(x => x.RegistrationNumber)
                .IsUnique();

            e.Property(x => x.CapacityKg)
                .HasPrecision(18, 2);

            e.Property(x => x.Status)
                .HasConversion<int>();
        });

        // ==========================================
        // DRIVER
        // ==========================================
        modelBuilder.Entity<Driver>(e =>
        {
            e.HasKey(x => x.Id);

            e.HasIndex(x => x.UserId)
                .IsUnique();

            e.HasIndex(x => x.LicenseNumber)
                .IsUnique();

            e.Property(x => x.LicenseNumber)
                .HasMaxLength(50)
                .IsRequired();

            e.HasOne(x => x.User)
                .WithOne(x => x.Driver)
                .HasForeignKey<Driver>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // =====================================================
        // MEMBER 3 - ROUTE
        // =====================================================
        modelBuilder.Entity<Route>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.StartLocation)
                .HasMaxLength(250)
                .IsRequired();

            e.Property(x => x.Destination)
                .HasMaxLength(250)
                .IsRequired();

            e.Property(x => x.DistanceKm)
                .HasPrecision(18, 2);

            e.Property(x => x.EstimatedDurationMinutes)
                .IsRequired();

            e.Property(x => x.Status)
                .HasConversion<int>();

            e.HasIndex(x => x.Status);

            e.HasMany(x => x.RouteStops)
                .WithOne(x => x.Route)
                .HasForeignKey(x => x.RouteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // =====================================================
        // MEMBER 3 - ROUTE STOP
        // =====================================================
        modelBuilder.Entity<RouteStop>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Address)
                .HasMaxLength(500)
                .IsRequired();

            e.Property(x => x.Latitude)
                .HasPrecision(9, 6);

            e.Property(x => x.Longitude)
                .HasPrecision(9, 6);

            // StopOrder must be unique inside a Route
            e.HasIndex(x => new
            {
                x.RouteId,
                x.StopOrder
            }).IsUnique();
        });

        // =====================================================
        // MEMBER 3 - DELIVERY
        // =====================================================
        modelBuilder.Entity<Delivery>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Status)
                .HasConversion<int>();

            e.Property(x => x.FailureReason)
                .HasMaxLength(1000);

            e.HasIndex(x => x.ShipmentId);
            e.HasIndex(x => x.DriverId);
            e.HasIndex(x => x.VehicleId);
            e.HasIndex(x => x.RouteId);
            e.HasIndex(x => x.Status);

            e.HasMany(x => x.ProofsOfDelivery)
                .WithOne(x => x.Delivery)
                .HasForeignKey(x => x.DeliveryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // =====================================================
        // MEMBER 3 - PROOF OF DELIVERY
        // =====================================================
        modelBuilder.Entity<ProofOfDelivery>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.ReceiverName)
                .HasMaxLength(200)
                .IsRequired();

            e.Property(x => x.SignaturePath)
                .HasMaxLength(500)
                .IsRequired();

            e.Property(x => x.PhotoPath)
                .HasMaxLength(500)
                .IsRequired();

            e.Property(x => x.Remarks)
                .HasMaxLength(1000);

            e.HasIndex(x => x.DeliveryId);
        });

        // =====================================================
        // MEMBER 3 - SCHEDULE
        // =====================================================
        modelBuilder.Entity<Schedule>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Status)
                .HasConversion<int>();

            e.HasIndex(x => x.ShipmentId);
            e.HasIndex(x => x.DriverId);
            e.HasIndex(x => x.VehicleId);
            e.HasIndex(x => x.StartTimeUtc);
            e.HasIndex(x => x.EndTimeUtc);
            e.HasIndex(x => x.Status);
        });

        // =====================================================
        // ROLE SEED DATA
        // =====================================================
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "Logistics Staff" },
            new Role { Id = 3, Name = "Driver" },
            new Role { Id = 4, Name = "Customer" }
        );
    }
}