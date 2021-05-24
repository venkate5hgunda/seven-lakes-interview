using System.Collections.Generic;
using SevenLakesInterview.Models;

namespace SevenLakesInterview.Helpers
{
    public interface IRouteHelper
    {
        public List<FlattenedRoute> FlattenRouteInput(List<Route> routeInput);
    }
}
