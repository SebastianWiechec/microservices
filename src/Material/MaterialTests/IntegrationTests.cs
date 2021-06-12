using MaterialApi;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using MaterialApi.Models;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MaterialTests
{
    public class IntegrationTests : IClassFixture<TestFixture<Startup>>
    {

        private HttpClient Client;
        private string requestUrl = "/api/Material";
        //https://localhost:44385/api/Material

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
            var act = JsonConvert.DeserializeObject<List<Material>>(jsonString);

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
            var act = JsonConvert.DeserializeObject<Material>(jsonString);

            // Assert
            Assert.Equal(1, act.IDMaterial);
            Assert.Equal("M02310", act.MCode);
            Assert.Equal("NaF fluorek sodu", act.Name);
            Assert.Equal("kg", act.Unit);

        }

        [Fact]
        public async Task TestPostItem()
        {
            // Dodanie nowego wiersza

            var requestBody = new
            {
                Url = requestUrl,
                Body = new Material
                {
                    IDMaterial = 99,
                    MCode = "M-test",
                    Name = "NameTest",
                    Unit = "litr",
                }
            };

            // Act
            var response = await Client.PostAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));
            var response2 = await Client.GetAsync(requestUrl + "/99");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Material>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.IDMaterial, act.IDMaterial);
            Assert.Equal(requestBody.Body.MCode, act.MCode);
            Assert.Equal(requestBody.Body.Name, act.Name);
            Assert.Equal(requestBody.Body.Unit, act.Unit);
        }




        [Fact]
        public async Task TestPutItemAsync()
        {
            // Update nowego wiersza
            var requestBody = new
            {
                Url = requestUrl + "/7",
                Body = new
                {
                    IDMaterial = 7,
                    MCode = "M-test1",
                    Name = "NameTest1",
                    Unit = "litr1",
                }
            };

            // Act
            var response = await Client.PutAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));

            var response2 = await Client.GetAsync(requestUrl + "/7");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Material>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.IDMaterial, act.IDMaterial);
            Assert.Equal(requestBody.Body.MCode, act.MCode);
            Assert.Equal(requestBody.Body.Name, act.Name);
            Assert.Equal(requestBody.Body.Unit, act.Unit);
        }

        [Fact]
        public async Task TestDeleteItemAsync()
        {
            var response = await Client.DeleteAsync(requestUrl + "/99");

            // Assert
            response.EnsureSuccessStatusCode();
            var response2 = await Client.GetAsync(requestUrl + "/99");
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
