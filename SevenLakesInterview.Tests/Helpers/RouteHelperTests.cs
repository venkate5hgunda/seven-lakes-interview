using System.Collections.Generic;
using System.Linq;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using SevenLakesInterview.Helpers;
using SevenLakesInterview.Models;
using Shouldly;
using Xunit;

namespace SevenLakesInterview.Tests.Helpers
{
    public class RouteHelperTests
    {
        private RouteHelper _subjectUnderTest;
        private readonly ILogger _logger;

        public RouteHelperTests()
        {
            _logger = new LoggerConfiguration().WriteTo.TestCorrelator().CreateLogger();
        }

        [Fact] // POSITIVE SCENARIO
        public void FlattenRouteInput_Should_Return_Proper_Output_When_Input_Valid_NotNull()
        {
            // ARRANGE
            _subjectUnderTest = new RouteHelper(_logger);

            using (TestCorrelator.CreateContext())
            {
                // ACT
                var output = _subjectUnderTest.FlattenRouteInput(_methodInput);

                // ASSERT
                output.Count.ShouldBe(_methodOutput.Count);
                TestCorrelator.GetLogEventsFromCurrentContext().Count().ShouldBe(0);
            }
        }

        [Fact] // NEGATIVE SCENARIO
        public void FlattenRouteInput_Should_Log_Warning_When_Input_Null()
        {
            // ARRANGE
            var nullInputWarning = "FlattenRouteInput received a NULL Input";
            _subjectUnderTest = new RouteHelper(_logger);

            using (TestCorrelator.CreateContext())
            {
                var output = _subjectUnderTest.FlattenRouteInput(null);

                output.ShouldBe(new List<FlattenedRoute>());
                TestCorrelator.GetLogEventsFromCurrentContext().Single(x=>x.Level==LogEventLevel.Warning).MessageTemplate.Text.ShouldContain(nullInputWarning);
            }
        }

        [Fact] // NEGATIVE SCENARIO
        public void FlattenRouteInput_Should_Log_Error_When_Method_Invalid()
        {
            // ARRANGE
            var exceptionMessage = "Flattening the Input failed with NullReferenceException";
            _subjectUnderTest = new RouteHelper(_logger);

            using (TestCorrelator.CreateContext())
            {
                var output = _subjectUnderTest.FlattenRouteInput(_methodInputInvalid);

                output.ShouldBe(new List<FlattenedRoute>());
                TestCorrelator.GetLogEventsFromCurrentContext().Single(x => x.Level == LogEventLevel.Error).MessageTemplate.Text.ShouldContain(exceptionMessage);
            }
        }


        // UNIT TEST DATA
        private readonly List<Route> _methodInput = new List<Route>
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
        private readonly List<FlattenedRoute> _methodOutput = new List<FlattenedRoute>
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
        private readonly List<Route> _methodInputInvalid = new List<Route>
        {
            new Route
            {
                RouteName = "route-1",
                Stops = new List<Stop>
                {
                    new Stop
                    {
                        StopName = "stop-1-route-1",
                        Objects = null // Object should not be NULL, as constructor doesn't allow it.
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
    }
}
