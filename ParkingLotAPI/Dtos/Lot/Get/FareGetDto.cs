﻿namespace ParkingLotAPI.Dtos.Lot.Get
{
	public class FareGetDto
	{
		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public decimal PricePerHour { get; set; }

		public bool IsCurrent { get; set; }
	}
}
