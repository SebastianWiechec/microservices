using WarehouseApi;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using WarehouseApi.Models;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace WarehouseTests
{
    public class IntegrationTests : IClassFixture<TestFixture<Startup>>
    {

        private HttpClient Client;
        private string requestUrl = "/api/Warehouse";
        //https://localhost:44385/api/Warehouse

        public IntegrationTests(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task TestGetAllAsync()
        {
            // Act
            var response = await Client.GetAsync(requestUrl);
            string jsonString = response.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<List<Warehouse>>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(act.Count > 0);
        }

        [Fact]
        public async Task TestGetAsync()
        {
            // Act
            var response = await Client.GetAsync(requestUrl + "/1");

            string jsonString = response.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Warehouse>(jsonString);

            // Assert
            Assert.Equal(1, act.IDWarehouse);
            Assert.Equal(2, act.IDMaterial);
            Assert.Equal(253.5, act.Quantity);
           
        }

        [Fact]
        public async Task TestPostItem()
        {
            // Dodanie nowego wiersza
        
            var requestBody = new 
            {
                Url = requestUrl,
                Body = new Warehouse
                {
                    IDWarehouse = 23,
                    IDMaterial = 7,
                    Quantity = 231,
                }
            };

            // Act
            var response = await Client.PostAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));
            var response2 = await Client.GetAsync(requestUrl + "/23");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Warehouse>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.IDWarehouse, act.IDWarehouse);
            Assert.Equal(requestBody.Body.IDMaterial, act.IDMaterial);
            Assert.Equal(requestBody.Body.Quantity, act.Quantity);
        }




        [Fact]
        public async Task TestPutItemAsync()
        {
            // Update nowego wiersza
            var requestBody = new
            {
                Url = requestUrl + "/2",
                Body = new
                {
                    IDWarehouse = 2,
                    IDMaterial = 5,
                    Quantity = 111,
                }
            };

            // Act
            var response = await Client.PutAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));

            var response2 = await Client.GetAsync(requestUrl + "/2");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Warehouse>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.IDWarehouse, act.IDWarehouse);
            Assert.Equal(requestBody.Body.IDMaterial, act.IDMaterial);
            Assert.Equal(requestBody.Body.Quantity, act.Quantity);
        }

        [Fact]
        public async Task TestDeleteItemAsync()
        {
            var response = await Client.DeleteAsync(requestUrl + "/23");

            // Assert
            response.EnsureSuccessStatusCode();
            var response2 = await Client.GetAsync(requestUrl + "/23");
            //USTAWIC SPRAWDZANIE
            //  Assert.False(singleResponse.Id);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response2.IsSuccessStatusCode);

        }
        [Fact]
        public async Task Return_404_Result()
        {
            // Act
            var response = await Client.GetAsync(String.Empty);

            // Assert
            //response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task Return_400_Result()
        {
            // Act
            var response = await Client.DeleteAsync(requestUrl + "/{id_url}");

            // Assert
            //response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task Return_500_Result()
        {
            // Act
            var response = await Client.DeleteAsync(requestUrl + "/0");

            // Assert
            //response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
