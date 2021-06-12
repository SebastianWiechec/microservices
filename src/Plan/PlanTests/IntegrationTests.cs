using PlanApi;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using PlanApi.Models;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace PlanTests
{
    public class IntegrationTests : IClassFixture<TestFixture<Startup>>
    {

        private HttpClient Client;
        private string requestUrl = "/api/Plan";
        //https://localhost:44385/api/Plan

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
            var act = JsonConvert.DeserializeObject<List<Plan>>(jsonString);

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
            var act = JsonConvert.DeserializeObject<Plan>(jsonString);

            // Assert
            Assert.Equal(1, act.IDPlan);
            Assert.Equal(2, act.IDRecipe);
            Assert.Equal(12, act.Quantity);
            Assert.Equal(new DateTime(2021, 06, 07), act.Date);
            Assert.Equal("1A", act.Shift);
        }

        [Fact]
        public async Task TestPostItem()
        {
            // Dodanie nowego wiersza
            DateTime year = new DateTime(2022, 1, 1);
            var requestBody = new
            {
                Url = requestUrl,
                Body = new Plan
                {
                    IDPlan = 99,
                    IDRecipe = 1,
                    Quantity = 1,
                    Date = year,
                    Shift="1test",
                }
            };

            // Act
            var response = await Client.PostAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));
            var response2 = await Client.GetAsync(requestUrl + "/99");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Plan>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.IDPlan, act.IDPlan);
            Assert.Equal(requestBody.Body.IDRecipe, act.IDRecipe);
            Assert.Equal(requestBody.Body.Quantity, act.Quantity);
            Assert.Equal(requestBody.Body.Date, act.Date);
            Assert.Equal(requestBody.Body.Shift, act.Shift);
        }




        [Fact]
        public async Task TestPutItemAsync()
        {
            // Update nowego wiersza
            DateTime year = new DateTime(2022, 1, 1);
            var requestBody = new
            {
                Url = requestUrl + "/4",
                Body = new
                {
                    IDPlan = 4,
                    IDRecipe = 1,
                    Quantity = 1,
                    Date = year,
                    Shift = "2test",
                }
            };

            // Act
            var response = await Client.PutAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));

            var response2 = await Client.GetAsync(requestUrl + "/4");
            string jsonString = response2.Content.ReadAsStringAsync().Result;
            var act = JsonConvert.DeserializeObject<Plan>(jsonString);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(requestBody.Body.IDPlan, act.IDPlan);
            Assert.Equal(requestBody.Body.IDRecipe, act.IDRecipe);
            Assert.Equal(requestBody.Body.Quantity, act.Quantity);
            Assert.Equal(requestBody.Body.Date, act.Date);
            Assert.Equal(requestBody.Body.Shift, act.Shift);
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
