using DoneApi;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using DoneApi.Models;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace DoneTests
{
    public class IntegrationTests : IClassFixture<TestFixture<Startup>>
    {

        private HttpClient Client;
        private string requestUrl = "/api/Done";
        //https://localhost:44385/api/Done

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
            var act = JsonConvert.DeserializeObject<List<Done>>(jsonString);

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
            var act = JsonConvert.DeserializeObject<Done>(jsonString);

            // Assert
            Assert.Equal(1, act.IDDone);
            Assert.Equal(1, act.IDRecipe);
            Assert.Equal(5, act.Quantity);

        }

        [Fact]
        public async Task TestPostItem()
        {
            // Dodanie nowego wiersza

            var requestBody = new
            {
                Url = requestUrl,
                Body = new Done
                {
                    IDDone = 99,
                    IDRecipe = 1,
                    Quantity = 33,
                }
            };

            // Act
            var response = await Client.PostAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));
            var response2 = await Client.GetAsync(requestUrl + "/99");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Done>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.IDDone, act.IDDone);
            Assert.Equal(requestBody.Body.IDRecipe, act.IDRecipe);
            Assert.Equal(requestBody.Body.Quantity, act.Quantity);
        }




        [Fact]
        public async Task TestPutItemAsync()
        {
            // Update nowego wiersza
            var requestBody = new
            {
                Url = requestUrl + "/3",
                Body = new
                {
                    IDDone = 3,
                    IDRecipe = 4,
                    Quantity = 44,
                }
            };

            // Act
            var response = await Client.PutAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));

            var response2 = await Client.GetAsync(requestUrl + "/3");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Done>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.IDDone, act.IDDone);
            Assert.Equal(requestBody.Body.IDRecipe, act.IDRecipe);
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
