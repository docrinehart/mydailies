using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyDailies.Data;
using MyDailies.Data.Entities;

namespace MyDailies.Controllers
{
    public class RatingData
    {
        public IList<DailyRating> Ratings { get; set; }
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
        public RatingData Get()
        {
            var data = _dbContext.DailyRatings
                .Include(x => x.Metric)
                .OrderByDescending(x => x.RatingDate)
                .ToList();

            return new RatingData
            {
                Ratings = data
            };
        }
    }
}
