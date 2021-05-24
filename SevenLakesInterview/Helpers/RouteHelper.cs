using System;
using System.Collections.Generic;
using SevenLakesInterview.Models;
using ILogger = Serilog.ILogger;

namespace SevenLakesInterview.Helpers
{
    public class RouteHelper : IRouteHelper
    {
        private readonly ILogger _logger;
        public RouteHelper(ILogger logger)
        {
            _logger = logger.ForContext<RouteHelper>();
        }
        public List<FlattenedRoute> FlattenRouteInput(List<Route> input)
        {
            var watch = new System.Diagnostics.Stopwatch(); // Performance Monitoring
            if (input == null)
            {
                _logger.Warning("FlattenRouteInput received a NULL Input");
                return new List<FlattenedRoute>();
            }
            var routeOutput = new List<FlattenedRoute>();
            watch.Start();
            try
            {
                foreach (var route in input)
                {
                    foreach (var stop in route.Stops)
                    {
                        foreach (var objct in stop.Objects)
                        {
                            routeOutput.Add(new FlattenedRoute
                            {
                                RouteName = route.RouteName,
                                StopName = stop.StopName,
                                ObjectName = objct.ObjectName,
                                ObjectType = objct.ObjectType
                            });
                        }
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                _logger.Error($"Flattening the Input failed with NullReferenceException: {ex}");
            }
            watch.Stop();
            _logger.Debug($"FlattenRouteInput Execution Time: {watch.ElapsedMilliseconds}ms");
            return routeOutput;
        }
    }
}
