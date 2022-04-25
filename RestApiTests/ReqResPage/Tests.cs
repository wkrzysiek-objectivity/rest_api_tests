using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using FluentAssertions;
using System.Net;
using RestApiTests.UsersList;

namespace RestApiTests
{
    public class Tests
    {
        [Test]
        public void Test1_GetStatusCode()
        {
            var restclient = new RestClient(Settings.reqresApi);
            var request = new RestRequest("/api/users?page=2", Method.GET);
            var response = restclient.Execute(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Test]
        public void Test2_CheckFirstUser()
        {
            var responseUser = new User
            {
                id = 1,
                email = "george.bluth@reqres.in",
                first_name = "George",
                last_name = "Bluth",
                avatar = "https://reqres.in/img/faces/1-image.jpg"
            };

            var restclient = new RestClient(Settings.reqresApi);
            var request = new RestRequest("/api/users/1", Method.GET);
            var response = restclient.Execute(request);
            ResponseModelUsersList responseObject = JsonConvert.DeserializeObject<ResponseModelUsersList>(response.Content);

            responseUser
                .Should()
                .BeEquivalentTo(responseObject.data);
        }

        [Test]
        public void Test3_CreateNewUser()
        {
            var restclient = new RestClient(Settings.reqresApi);
            var request = new RestRequest("/api/users", Method.POST);

            request.AddParameter("name", "Wojciech");
            request.AddParameter("job", "Tester");

            var response = restclient.Execute(request);
            UpdateUserRespone responseObject = JsonConvert.DeserializeObject<UpdateUserRespone>(response.Content);
            
            Assert.AreEqual(responseObject.name, "Wojciech");
            Assert.AreEqual(responseObject.job, "Tester");
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public void Test4_UpdateUser()
        {
            var restclient = new RestClient(Settings.reqresApi);
            var request = new RestRequest("/api/users/1", Method.PUT);

            request.AddParameter("name", "Wojciech");
            request.AddParameter("job", "Tester");

            var response = restclient.Execute(request);
            UpdateUserRespone responseObject = JsonConvert.DeserializeObject<UpdateUserRespone>(response.Content);

            Assert.AreEqual(responseObject.name, "Wojciech");
            Assert.AreEqual(responseObject.job, "Tester");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}