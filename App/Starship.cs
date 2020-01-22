using System;

namespace App
{
    public class Starship
    {
        public string Name { get; }
        public uint MegalightsPerHour { get; }
        public uint Consumables { get; }

        public Starship (string Name, uint MegalightsPerHour, uint Consumables)
        {
            this.Name = Name;
            this.MegalightsPerHour = MegalightsPerHour;
            this.Consumables = Consumables;
        }

        public int CalculateNumberOfStopsForDistance (uint Distance)
        {
            return int.MinValue;
        }
    }
}
