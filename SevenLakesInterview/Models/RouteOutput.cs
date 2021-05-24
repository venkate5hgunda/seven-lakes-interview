using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SevenLakesInterview.Models
{
    [ExcludeFromCodeCoverage]
    public class FlattenedRoute
    {
        public string RouteName { get; set; }
        public string StopName { get; set; }
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }
    }
}
