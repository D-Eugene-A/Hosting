// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.TestHost
{
    public static class HostBuilderTestServerExtensions
    {
        /// <summary>
        /// Builds and Starts the Host using a TestServer.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="featureCollection"></param>
        /// <returns></returns>
        public static async Task<TestServer> StartTestServerAsync(this IHostBuilder hostBuilder, IFeatureCollection featureCollection = null)
        {
            var testServer = new TestServer(featureCollection ?? new FeatureCollection());
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IServer>(testServer);
            });
            var host = hostBuilder.Build();
            await host.StartAsync();
            return testServer;
        }

        /// <summary>
        /// Builds and Starts the Host using a TestServer.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static async Task<IHost> StartTestHostAsync(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IServer, TestServer>();
            });
            var host = hostBuilder.Build();
            await host.StartAsync();
            return host;
        }

        /// <summary>
        /// Retrieves the TestServer from the host services.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static TestServer GetTestServer(this IHost host)
        {
            return (TestServer)host.Services.GetRequiredService<IServer>();
        }

        /// <summary>
        /// Retrieves the test client from the TestServer in the host services.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static HttpClient GetTestClient(this IHost host)
        {
            return host.GetTestServer().CreateClient();
        }
    }
}
