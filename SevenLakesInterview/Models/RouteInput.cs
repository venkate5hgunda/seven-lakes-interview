using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SevenLakesInterview.Models
{
    [ExcludeFromCodeCoverage]
    public class RouteInput
    {
        public List<Route> Routes { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class Route
    {
        public Route()
        {
            Stops = new List<Stop>();
        }
        public string RouteName { get; set; }
        public List<Stop> Stops { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class Stop
    {
        public Stop()
        {
            Objects = new List<Object>();
        }
        public string StopName { get; set; }
        public List<Object> Objects { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class Object
    {
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }
    }
}
