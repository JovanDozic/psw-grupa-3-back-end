﻿using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.UseCases.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/tourReview")]
    public class TourReviewController : BaseApiController
    {
        private readonly ITourReviewService _tourReviewService;

        public TourReviewController(ITourReviewService tourReviewService)
        {
            _tourReviewService = tourReviewService;
        }

        [HttpPost]
        public ActionResult<TourReviewDto> Create([FromBody] TourReviewDto tourReview)
        {
            var result = _tourReviewService.Create(tourReview);
            return CreateResponse(result);
        }
    }
}