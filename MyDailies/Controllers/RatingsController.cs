using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDailies.Data;
using MyDailies.Data.Entities;
using static MyDailies.Controllers.RatingData;

namespace MyDailies.Controllers
{
    public class RatingData
    {
        public class RatingDay
        {
            public string Date { get; set; }
            public IEnumerable<RatingMetric> Metrics { get; set; }
        }

        public class RatingMetric
        {
            public string Metric { get; set; }
            public int Rating { get; set; } 
            public string Notes { get; set; }
        }

        public RatingDay Ratings { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public RatingsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public RatingDay Get(string date)
        {
            var groups = _dbContext.DailyRatings
                .Include(x => x.Metric)
                .Where(x => x.RatingDate == date)
                .ToList();

            var day = new RatingDay
            {
                Date = date,
                Metrics = groups.Select(r => new RatingMetric
                {
                    Metric = r.Metric.Name,
                    Rating = r.Rating,
                    Notes = r.Notes
                }).ToList()
            };

            return day;
        }
    }
}
