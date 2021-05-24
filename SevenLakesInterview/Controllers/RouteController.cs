using Serilog;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SevenLakesInterview.Helpers;
using SevenLakesInterview.Models;

namespace SevenLakesInterview.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IRouteHelper _routeHelper;
        public RouteController(ILogger logger, IRouteHelper routeHelper)
        {
            _logger = logger;
            _routeHelper = routeHelper;
        }

        [HttpGet]
        public string Get()
        {
            _logger.Information("RouteController Get API :: Invoked");
            return "You have reached RouteController";
		}

        [HttpPost]
        public string Post([FromBody]List<Route> input)
        {
            _logger.Information("RouteController Post API :: Invoked");
            var output = new List<FlattenedRoute>();
            try
            {
                output = _routeHelper.FlattenRouteInput(input);
            }
            catch (Exception ex)
            {
                _logger.Error($"RouteController Post API Failed with Exception: {ex}");
            }
            _logger.Information("RouteController Post API :: Completed");
            return JsonConvert.SerializeObject(output);
        }
    }
}
