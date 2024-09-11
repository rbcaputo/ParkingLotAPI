﻿using ParkingLotAPI.Dtos.Min;
using static ParkingLotAPI.Data.Constants.SizeFareMods;

namespace ParkingLotAPI.Dtos.Lot
{
	public class VehicleGetDto
	{
		public string LicensePlate { get; set; } = string.Empty;

		public VehicleSizes Size { get; set; }

		public string Brand { get; set; } = string.Empty;

		public string Model { get; set; } = string.Empty;

		public string Color { get; set; } = string.Empty;

		public ICollection<ParkingGetMinDto> Parkings { get; set; } = [];
	}
}
