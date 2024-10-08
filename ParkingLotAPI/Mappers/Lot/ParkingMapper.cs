﻿using Microsoft.EntityFrameworkCore;
using ParkingLotAPI.Data;
using ParkingLotAPI.Dtos.Lot.Get;
using ParkingLotAPI.Dtos.Min;
using ParkingLotAPI.Models.Lot;
using ParkingLotAPI.Utils;

namespace ParkingLotAPI.Mappers.Lot
{
	public static class ParkingMapper
	{
		public static ParkingGetDto MapParkingModelToGetDto(ParkingModel parking)
		{
			return new()
			{
				Fare = FareMapper.MapFareModelToGetDto(parking.Fare),

				Vehicle = VehicleMapper.MapVehicleModelToMinGetDto(parking.Vehicle),

				EntryTime = parking.EntryTime,

				ExitTime = parking.ExitTime,

				Duration = parking.Duration,

				TotalPrice = parking.TotalPrice
			};
		}

		public static ParkingMinGetDto MapParkingModelToMinGetDto(ParkingModel parking)
		{
			return new()
			{
				LicensePlate = parking.Vehicle.LicensePlate,

				EntryTime = parking.EntryTime,

				ExitTime = parking.ExitTime,

				Duration = parking.Duration,

				TotalPrice = parking.TotalPrice
			};
		}

		public static async Task<ParkingModel> MapParkingPostToModel(string licensePlate, DataContext context, CancellationToken cancellation)
		{
			try
			{
				FareModel? fare = await context.Fares
					.Where(f => f.IsCurrent)
					.Include(f => f.Parkings)
					.FirstOrDefaultAsync(cancellation);

				if (fare == null)
					throw new InvalidOperationException($"{nameof(ParkingMapper)}: {nameof(MapParkingPostToModel)}: {nameof(fare)} cannot be null.");

				VehicleModel? vehicle = await context.Vehicles
					.Where(v => v.LicensePlate == licensePlate.Replace("-", "").ToUpper())
					.Include(v => v.Parkings)
					.FirstOrDefaultAsync(cancellation);

				return vehicle == null
					? throw new InvalidOperationException($"No vehicle found with the given license plate.")
					: new()
					{
						Fare = fare,

						FareId = fare.Id,

						Vehicle = vehicle,

						VehicleId = vehicle.Id,

						EntryTime = DateTime.Now
					};
			}
			catch
			{
				throw;
			}
		}

		public static void MapParkingPutToModel(ParkingModel parking)
		{
			parking.ExitTime = DateTime.Now;

			parking.Duration = CalculatorClass.CalculateDuration(parking);

			parking.TotalPrice = CalculatorClass.CalculateTotalPrice(parking);

			parking.Vehicle.IsParked = ValidatorClass.CheckIfVehicleIsParked(parking.Vehicle);
		}
	}
}
