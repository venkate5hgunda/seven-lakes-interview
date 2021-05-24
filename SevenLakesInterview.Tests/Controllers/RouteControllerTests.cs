using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using SevenLakesInterview.Controllers;
using SevenLakesInterview.Helpers;
using SevenLakesInterview.Models;
using Shouldly;
using Xunit;
using Object = SevenLakesInterview.Models.Object;

namespace SevenLakesInterview.Tests.Controllers
{
    public class RouteControllerTests
    {
        private RouteController _subjectUnderTest;
        private readonly ILogger _logger;
        private readonly IRouteHelper _routeHelper;
        public RouteControllerTests()
        {
            _logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
            _routeHelper = Mock.Of<IRouteHelper>();
        }

        [Fact] // POSITIVE SCENARIO
        public void Get_Api_Should_Return_Default_String()
        {
            // ARRANGE
            var outputMessage = "You have reached RouteController";
            var invokeLog = "RouteController Get API :: Invoked";
            _subjectUnderTest = new RouteController(_logger,_routeHelper);

            using (TestCorrelator.CreateContext())
            {
                // ACT
                var output = _subjectUnderTest.Get();

                // ASSERT
                output.ShouldBe(outputMessage);
                TestCorrelator.GetLogEventsFromCurrentContext().Single(x => x.Level == LogEventLevel.Information).MessageTemplate.Text.ShouldContain(invokeLog);
            }
        }

        [Fact] // POSITIVE SCENARIO
        public void Post_Api_Should_Return_Valid_Response()
        {
            var invokeLog = "RouteController Post API :: Invoked";
            var completedLog = "RouteController Post API :: Completed";
            Mock.Get(_routeHelper).Setup(x => x.FlattenRouteInput(_postApiInput)).Returns(_postApiOutput);
            _subjectUnderTest = new RouteController(_logger, _routeHelper);

            using (TestCorrelator.CreateContext())
            {
                var output = _subjectUnderTest.Post(_postApiInput);

                output.ShouldBe(JsonConvert.SerializeObject(_postApiOutput));
                TestCorrelator.GetLogEventsFromCurrentContext().Count(x => x.Level == LogEventLevel.Information).ShouldBe(2);
                TestCorrelator.GetLogEventsFromCurrentContext().Count(x =>
                    (x.Level == LogEventLevel.Information && x.MessageTemplate.Text.Contains(invokeLog))).ShouldBe(1);
                TestCorrelator.GetLogEventsFromCurrentContext().Count(x =>
                    (x.Level == LogEventLevel.Information && x.MessageTemplate.Text.Contains(completedLog))).ShouldBe(1);
            }
        }

        [Fact] // NEGATIVE SCENARIO
        public void Post_Api_Should_Catch_And_Log_Exception_When_FlattenRouteInput_Fails()
        {
            var invokeLog = "RouteController Post API :: Invoked";
            var completedLog = "RouteController Post API :: Completed";
            var customExceptionMessage = "RouteHelper Custom Exception";
            Mock.Get(_routeHelper).Setup(x => x.FlattenRouteInput(_postApiInput)).Throws(new Exception(customExceptionMessage));
            _subjectUnderTest = new RouteController(_logger, _routeHelper);

            using (TestCorrelator.CreateContext())
            {
                var output = _subjectUnderTest.Post(_postApiInput);

                output.ShouldBe(JsonConvert.SerializeObject(new List<FlattenedRoute>()));
                TestCorrelator.GetLogEventsFromCurrentContext().Count(x => x.Level == LogEventLevel.Information).ShouldBe(2);
                TestCorrelator.GetLogEventsFromCurrentContext().Count(x =>
                    (x.Level == LogEventLevel.Information && x.MessageTemplate.Text.Contains(invokeLog))).ShouldBe(1);
                TestCorrelator.GetLogEventsFromCurrentContext().Count(x =>
                    (x.Level == LogEventLevel.Information && x.MessageTemplate.Text.Contains(completedLog))).ShouldBe(1);
                TestCorrelator.GetLogEventsFromCurrentContext().Single(x=>x.Level==LogEventLevel.Error).MessageTemplate.Text.ShouldContain(customExceptionMessage);
            }
        }


        // UNIT TEST DATA
        private readonly List<Route> _postApiInput = new List<Route>
        {
            new Route
            {
                RouteName = "route-1",
                Stops = new List<Stop>
                {
                    new Stop
                    {
                        StopName = "stop-1-route-1",
                        Objects = new List<Object>
                        {
                            new Object
                            {
                                ObjectName = "object-name-1-stop-1-route-1",
                                ObjectType = "object-type-1-stop-1-route-1"
                            },
                            new Object
                            {
                                ObjectName = "object-name-2-stop-1-route-1",
                                ObjectType = "object-type-2-stop-1-route-1"
                            }
                        }
                    },
                    new Stop
                    {
                        StopName = "stop-2-route-1",
                        Objects = new List<Object>
                        {
                            new Object
                            {
                                ObjectName = "object-name-1-stop-2-route-1",
                                ObjectType = "object-type-1-stop-2-route-1"
                            },
                            new Object
                            {
                                ObjectName = "object-name-2-stop-2-route-1",
                                ObjectType = "object-type-2-stop-2-route-1"
                            }
                        }
                    }
                }
            },
            new Route
            {
                RouteName = "route-2",
                Stops = new List<Stop>
                {
                    new Stop
                    {
                        StopName = "stop-1-route-2",
                        Objects = new List<Object>
                        {
                            new Object
                            {
                                ObjectName = "object-name-1-stop-1-route-2",
                                ObjectType = "object-type-1-stop-1-route-2"
                            },
                            new Object
                            {
                                ObjectName = "object-name-2-stop-1-route-2",
                                ObjectType = "object-type-2-stop-1-route-2"
                            }
                        }
                    },
                    new Stop
                    {
                        StopName = "stop-2-route-2",
                        Objects = new List<Object>
                        {
                            new Object
                            {
                                ObjectName = "object-name-1-stop-2-route-2",
                                ObjectType = "object-type-1-stop-2-route-2"
                            },
                            new Object
                            {
                                ObjectName = "object-name-2-stop-2-route-2",
                                ObjectType = "object-type-2-stop-2-route-2"
                            }
                        }
                    }
                }
            }
        };
        private readonly List<FlattenedRoute> _postApiOutput = new List<FlattenedRoute>
        {
            new FlattenedRoute
            {
                RouteName = "route-1",
                StopName = "stop-1",
                ObjectType = "object-type-1-stop-1-route-1",
                ObjectName = "object-name-1-stop-1-route-1"
            },
            new FlattenedRoute
            {
                RouteName = "route-1",
                StopName = "stop-1",
                ObjectType = "object-type-2-stop-1-route-1",
                ObjectName = "object-name-2-stop-1-route-1"
            },
            new FlattenedRoute
            {
                RouteName = "route-1",
                StopName = "stop-2",
                ObjectType = "object-type-1-stop-2-route-1",
                ObjectName = "object-name-1-stop-2-route-1"
            },
            new FlattenedRoute
            {
                RouteName = "route-1",
                StopName = "stop-2",
                ObjectType = "object-type-2-stop-2-route-1",
                ObjectName = "object-name-2-stop-2-route-1"
            },
            new FlattenedRoute
            {
                RouteName = "route-2",
                StopName = "stop-1",
                ObjectType = "object-type-1-stop-1-route-2",
                ObjectName = "object-name-1-stop-1-route-2"
            },
            new FlattenedRoute
            {
                RouteName = "route-2",
                StopName = "stop-1",
                ObjectType = "object-type-2-stop-1-route-2",
                ObjectName = "object-name-2-stop-1-route-2"
            },
            new FlattenedRoute
            {
                RouteName = "route-2",
                StopName = "stop-2",
                ObjectType = "object-type-1-stop-2-route-2",
                ObjectName = "object-name-1-stop-2-route-2"
            },
            new FlattenedRoute
            {
                RouteName = "route-2",
                StopName = "stop-2",
                ObjectType = "object-type-2-stop-2-route-2",
                ObjectName = "object-name-2-stop-2-route-2"
            }
        };
    }
}
