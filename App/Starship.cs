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
            int result = Distance != 0 ? -1 : 0;

            if (this.MegalightsPerHour > 0 && this.Consumables > 0)
            {
                double hours_to_cover = (double) Distance / this.MegalightsPerHour;
                double number_of_resupplies = hours_to_cover / this.Consumables;
                result = (int)Math.Ceiling (number_of_resupplies);
                result = result != 0 ? result - 1 : result;
            }

            return result;
        }
    }
}
