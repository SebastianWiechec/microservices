using PreweightApi;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using PreweightApi.Models;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace PreweightTests
{
    public class IntegrationTests : IClassFixture<TestFixture<Startup>>
    {

        private HttpClient Client;
        private string requestUrl = "/api/Preweight";
        //https://localhost:44385/api/Preweight

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
            var act = JsonConvert.DeserializeObject<List<Preweight>>(jsonString);

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
            var act = JsonConvert.DeserializeObject<Preweight>(jsonString);

            // Assert
            Assert.Equal(1, act.IDPreweight);
            Assert.Equal(1, act.IDRecipe);
            Assert.Equal(1, act.IDMaterial);
            Assert.Equal(2.299999952316284, act.Quantity);

        }

        [Fact]
        public async Task TestPostItem()
        {
            // Dodanie nowego wiersza

            var requestBody = new
            {
                Url = requestUrl,
                Body = new Preweight
                {
                    IDPreweight = 99,
                    IDRecipe = 3,
                    IDMaterial = 3,
                    Quantity = 1,
                }
            };

            // Act
            var response = await Client.PostAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));
            var response2 = await Client.GetAsync(requestUrl + "/99");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Preweight>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.IDPreweight, act.IDPreweight);
            Assert.Equal(requestBody.Body.IDRecipe, act.IDRecipe);
            Assert.Equal(requestBody.Body.IDMaterial, act.IDMaterial);
            Assert.Equal(requestBody.Body.Quantity, act.Quantity);
        }




        [Fact]
        public async Task TestPutItemAsync()
        {
            // Update nowego wiersza
            var requestBody = new
            {
                Url = requestUrl + "/10",
                Body = new
                {
                    IDPreweight = 10,
                    IDRecipe = 3,
                    IDMaterial = 3,
                    Quantity = 1,
                }
            };

            // Act
            var response = await Client.PutAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));

            var response2 = await Client.GetAsync(requestUrl + "/10");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Preweight>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.IDPreweight, act.IDPreweight);
            Assert.Equal(requestBody.Body.IDMaterial, act.IDMaterial);
            Assert.Equal(requestBody.Body.Quantity, act.Quantity);
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
