using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public static RatingDay GetRatingDay(AppDbContext dbContext, string date)
        {
            var groups = dbContext.DailyRatings
                .Include(x => x.Metric)
                .Where(x => x.RatingDate == date)
                .ToList();

            var day = new RatingDay
            {
                Date = date,
                Metrics = groups.OrderBy(g => g.Metric.Order)
                    .Select(r => new RatingMetric
                    {
                        Metric = r.Metric.Name,
                        Rating = r.Rating,
                        Notes = r.Notes
                    })
                    .ToList()
            };

            return day;
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public RatingController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public RatingDay Get(string date)
        {
            return RatingData.GetRatingDay(_dbContext, date);
        }

        [HttpPost("save")]
        public async Task<SaveRating.Response> Post([FromBody] SaveRating.Command command)
        {
            var newRating = new DailyRating
            {
                RatingDate = command.Date,
                MetricId = command.MetricId,
                Rating = command.Rating,
                Notes = command.Notes
            };

            try
            {
                await _dbContext.DailyRatings.AddAsync(newRating);
                _dbContext.SaveChanges();

                var newDay = RatingData.GetRatingDay(_dbContext, command.Date);

                return new SaveRating.Response
                {
                    DidPost = true,
                    NewDay = newDay
                };
            }
            catch (Exception ex)
            {
                return new SaveRating.Response
                {
                    DidPost = false,
                    NewDay = null
                };
            }
        }
    }

    public class SaveRating
    {
        public class Command
        {
            public string Date { get; set; }
            public int MetricId { get; set; }
            public int Rating { get; set; }
            public string Notes { get; set; }
        }

        public class Response
        {
            public bool DidPost { get; set; }
            public RatingDay NewDay { get; internal set; }
        }
    }
}
