using System;
using System.Text.Json.Serialization;

namespace MyDailies.Data.Entities
{
    public class DailyRating
    {
        public DateTime RatingDate { get; set; }

        public int MetricId { get; set; }

        public int Rating { get; set; }

        public string Notes { get; set; }

        [JsonIgnore]
        public Metric Metric { get; set; }
    }
}
