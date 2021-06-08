using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserApi;
using Xunit;

namespace UserApiIntegrationTests.UserController
{
    public class UserControllerIntegrationTest : IClassFixture<TestFixture<Startup>>
    {

        private HttpClient Client;
        private string requestUrl = "/api/User";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fixture"></param>
        public UserControllerIntegrationTest(TestFixture<Startup> fixture)
        {
            Client = fixture.Client;
        }

        [Fact]
        public async Task TestGetAllAsync()
        {
            // Act
            var response = await Client.GetAsync(requestUrl);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestGetAsync()
        {
            // Act
            var response = await Client.GetAsync(requestUrl + "/dae05c4a-2b59-47d1-af61-d01dd60adcee");
            var responseString = response.Content.ReadAsStringAsync().Result;

            JObject o = JObject.Parse(responseString);

            JsonSerializer serializer = new JsonSerializer();
            IdentityUser identityUser = (IdentityUser)serializer.Deserialize(new JTokenReader(o), typeof(IdentityUser));

            string status = identityUser.Id;

            Assert.Equal("dae05c4a-2b59-47d1-af61-d01dd60adcee", status);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestPostItem()
        {
            var queryString = new Dictionary<string, string>()
                                    {
                                        { "password", "123456789aA" }
                                    };
            // Arrange
            var requestBody = new
            {
                Url = requestUrl,
                Body = new IdentityUser
                {
                    Email = "IntegrationTest@gmail.com",
                    UserName = "IntegrationTest",
                }
            };

            var requestUri = QueryHelpers.AddQueryString(requestBody.Url, queryString);
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Content = ContentHelper.GetStringContent(requestBody.Body);

            // Act
            var response = await Client.PostAsync(requestUri, request.Content);


            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestPutItemAsync()
        {
            var requestBody = new
            {
                Url = requestUrl+ "/508ae23e-8ff9-4d58-8466-dab5153ed391",
                Body = new
                {
                    Id = "508ae23e-8ff9-4d58-8466-dab5153ed391",
                    Email = "IntegrationTest@gmail.com",
                    UserName = "IntegrationTestPut",
                }
            };

            // Act
            var response = await Client.PutAsync(requestBody.Url, ContentHelper.GetStringContent(requestBody.Body));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestDeleteItemAsync()
        {
            // Arrange
            //var queryString = new Dictionary<string, string>()
            //                        {
            //                            { "password", "123456789aA" }
            //                        };
            //// Arrange
            //var requestBody = new
            //{
            //    Url = requestUrl,
            //    Body = new IdentityUser
            //    {
            //        Email = "IntegrationTestDelete@gmail.com",
            //        UserName = "IntegrationTestDelete",
            //    }
            //};

            //var requestUri = QueryHelpers.AddQueryString(requestBody.Url, queryString);
            //var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            //request.Content = ContentHelper.GetStringContent(requestBody.Body);

            //// Act
            //var response = await Client.PostAsync(requestUri, request.Content);

            // Act
            //var postResponse = await Client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            //var jsonFromPostResponse = await postResponse.Content.ReadAsStringAsync();

            //var singleResponse = JsonConvert.DeserializeObject<IdentityUser>(jsonFromPostResponse);

            var deleteResponse = await Client.DeleteAsync(string.Format(requestUrl + "/508ae23e-8ff9-4d58-8466-dab5153ed391"));

            // Assert
            //postResponse.EnsureSuccessStatusCode();
            //USTAWIC SPRAWDZANIE
            //  Assert.False(singleResponse.Id);

            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
