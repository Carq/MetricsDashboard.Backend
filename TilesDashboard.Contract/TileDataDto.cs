﻿using TilesDashboard.Contract.Enums;

namespace TilesDashboard.Contract
{
    public class TileDataDto
    {
        public string Name { get; set; }

        public TileTypeDto Type { get; set; }

        public object Data { get; set; }
    }
}