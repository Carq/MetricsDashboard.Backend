using System;
using TilesDashboard.Core.Type;
using TilesDashboard.Core.Type.TileData;

namespace TilesDashboard.Core.Domain.Entities
{
    public class IntegerData : ITileData
    {
        public IntegerData(int value, DateTimeOffset addedOn)
        {
            Value = value;
            AddedOn = addedOn;
        }

        public int Value { get; private set; }

        public DateTimeOffset AddedOn { get; }

        public ValueObject GetData()
        {
            throw new NotImplementedException();
        }
    }
}
