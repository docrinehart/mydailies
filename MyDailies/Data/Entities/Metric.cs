using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyDailies.Data.Entities
{
    public class Metric
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<DailyRating> Ratings { get; set; }

        //protected override void ConfigureEntity(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Metric>().ToTable("kpi_metric")
        //        .HasKey(e => e.Id);
        //}
    }
}
