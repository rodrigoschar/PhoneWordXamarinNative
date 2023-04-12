﻿using System;
using SQLite;

namespace Core.Models
{
	public class Coordinates
	{
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}

