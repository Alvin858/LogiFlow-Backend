using LogiFlow.Application.DTOs.Tracking;
using LogiFlow.Application.Interfaces;
using LogiFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LogiFlow.Application.Services;

public class TrackingService : ITrackingService
{
    private readonly IApplicationDbContext _context;

    public TrackingService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TrackingDto?> GetByTrackingNumberAsync(
        string trackingNumber,
        CancellationToken cancellationToken = default)
    {
        var tracking = await _context.ShipmentTracking
            .AsNoTracking()
            .Include(x => x.Shipment)
            .Where(x => x.Shipment.TrackingNumber == trackingNumber)
            .OrderByDescending(x => x.UpdatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        return tracking == null ? null : MapToDto(tracking);
    }

    public async Task<List<TrackingDto>> GetShipmentTrackingAsync(
        int shipmentId,
        CancellationToken cancellationToken = default)
    {
        var tracking = await _context.ShipmentTracking
            .AsNoTracking()
            .Include(x => x.Shipment)
            .Where(x => x.ShipmentId == shipmentId)
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        return tracking.Select(MapToDto).ToList();
    }

    public async Task<TrackingDto> CreateAsync(
        CreateTrackingDto dto,
        CancellationToken cancellationToken = default)
    {
        var shipment = await _context.Shipments
            .FirstOrDefaultAsync(
                x => x.Id == dto.ShipmentId,
                cancellationToken);

        if (shipment == null)
        {
            throw new InvalidOperationException(
                "Shipment not found.");
        }

        var tracking = new ShipmentTracking
        {
            ShipmentId = dto.ShipmentId,
            Status = dto.Status,
            Location = dto.Location,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Remarks = dto.Remarks,
            UpdatedAtUtc = DateTime.UtcNow
        };

        _context.ShipmentTracking.Add(tracking);

        shipment.Status = dto.Status;
        shipment.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        tracking.Shipment = shipment;

        return MapToDto(tracking);
    }

    public async Task<TrackingDto?> UpdateAsync(
        int id,
        UpdateTrackingDto dto,
        CancellationToken cancellationToken = default)
    {
        var tracking = await _context.ShipmentTracking
            .Include(x => x.Shipment)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (tracking == null)
        {
            return null;
        }

        tracking.Status = dto.Status;
        tracking.Location = dto.Location;
        tracking.Latitude = dto.Latitude;
        tracking.Longitude = dto.Longitude;
        tracking.Remarks = dto.Remarks;
        tracking.UpdatedAtUtc = DateTime.UtcNow;

        tracking.Shipment.Status = dto.Status;
        tracking.Shipment.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(tracking);
    }

    public async Task<List<TrackingDto>> GetHistoryAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var tracking = await _context.ShipmentTracking
            .AsNoTracking()
            .Include(x => x.Shipment)
            .Where(x =>
                x.Id == id ||
                x.ShipmentId == id)
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ToListAsync(cancellationToken);

        return tracking.Select(MapToDto).ToList();
    }

    private static TrackingDto MapToDto(
        ShipmentTracking tracking)
    {
        return new TrackingDto
        {
            Id = tracking.Id,
            ShipmentId = tracking.ShipmentId,
            TrackingNumber =
                tracking.Shipment?.TrackingNumber ?? string.Empty,
            Status = tracking.Status,
            Location = tracking.Location,
            Latitude = tracking.Latitude,
            Longitude = tracking.Longitude,
            Remarks = tracking.Remarks,
            UpdatedAtUtc = tracking.UpdatedAtUtc
        };
    }
}