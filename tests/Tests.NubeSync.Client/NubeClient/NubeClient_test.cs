﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Tests.NubeSync.Client.NubeClient_test
{
    public class Add_table : NubeClientTestBase
    {
        [Fact]
        public async Task Adds_the_table_to_the_datastore()
        {
            DataStore.TableExistsAsync<TestItem>().Returns(true);

            await NubeClient.AddTableAsync<TestItem>();

            await DataStore.Received().AddTableAsync<TestItem>();
        }

        [Fact]
        public async Task Throws_when_table_is_not_found_in_the_datastore()
        {
            DataStore.TableExistsAsync<TestItem>().Returns(false);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await NubeClient.AddTableAsync<TestItem>());

            Assert.Equal("The table type TestItem cannot be found in the data store", ex.Message);
        }
    }

    public class Always : NubeClientTestBase
    {
        [Fact]
        public void Adds_the_installation_id_to_the_headers()
        {
            var installationIdHeader = HttpClient.DefaultRequestHeaders.Where(h => h.Key == "NUBE-INSTALLATION-ID").First();

            Assert.Equal(InstallationId, installationIdHeader.Value.First());
        }

        [Fact]
        public void Sets_the_server_base_address()
        {
            Assert.Equal(ClientConfiguration.Server.ToLower(), HttpClient.BaseAddress.ToString());
        }
    }
}