using Laybuy.Helper;
using Laybuy.Models.Common;
using Laybuy.Models.User;
using Newtonsoft.Json;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Laybuy.StepDefinitions
{
    [Binding]
    public class UsersSteps
    {
        private APIResponse _apiResponse;
        private List<UsersResponse> _usersResponse;
        public UsersSteps(APIResponse apiResponse)
        {
            _apiResponse = apiResponse;
        }

        [When(@"I call the GET method from Users api")]
        public void WhenICallTheGETMethodFromUserApi()
        {
            string endpoint = "/users";
            _apiResponse.Response=RestHelper.GetRequest(endpoint);  
        }

        [Then(@"I should get a response with Status code (.*)")]
        public void ThenIShouldGetAResponseWithStatusCode(int expectedStatusCode)
        {
            int actualStatusCode = Convert.ToInt32(_apiResponse.Response.StatusCode);
            Assert.AreEqual(expectedStatusCode, actualStatusCode, "The responsed status code is not as expected code "+expectedStatusCode);
        }

        [Then(@"I should get (.*) records returned")]
        public void ThenIShouldGetRecordsReturned(int expectedRecordNumber)
        {
            _usersResponse = JsonConvert.DeserializeObject<List<UsersResponse>>(_apiResponse.Response.Content);

            Assert.AreEqual(expectedRecordNumber, _usersResponse.Count, "There are not "+ expectedRecordNumber+" records");
        }

        [Then(@"The user '(.*)' is contained in the list")]
        public void ThenTheUserIsContainedInTheList(string expectedUser)
        {
            Assert.True(_usersResponse.Any(x=>x.Name==expectedUser), $"The user '{expectedUser}' is not in the user list");
        }


        [Then(@"I should be able to retrieve data for certain user by valid userId")]
        public void ThenIShouldBeAbleToRetrieveDataBy(Table userIdTable)
        {
            foreach (var row in userIdTable.Rows)
            {
                var userId = row.Values.ToList()[0];
                var expectedUserName= row.Values.ToList()[1];                
                string endpoint = $"/users/{userId}";
                _apiResponse.Response= RestHelper.GetRequest(endpoint);
                UsersResponse returnedUser = JsonConvert.DeserializeObject<UsersResponse>(_apiResponse.Response.Content);
                Assert.AreEqual(returnedUser.Username, expectedUserName, "The return user is not as expected");
            }
        }

        [Then(@"I should be able to retrieve data for any single user by valid userId between (.*) and (.*)")]
        public void ThenIShouldBeAbleToRetrieveDataForAnySingleUserByValidUserIdBetweenAnd(int userId1, int userId2)
        {
            int userId=new Random().Next(userId1-1, userId2+1);
            string endpoint = $"/users/{userId}";
            _apiResponse.Response = RestHelper.GetRequest(endpoint);
            UsersResponse returnedUser = JsonConvert.DeserializeObject<UsersResponse>(_apiResponse.Response.Content);
            Assert.AreEqual(userId,returnedUser.Id, "The user Id who's been retrieved is not returned");
        }
        

        [Then(@"I should get a response with Status code (.*) by invalid userId")]
        public void ThenIShouldGetStatusCodeWithNoReturnedUserDataByInvalidUserId(int expectedStatusCode, Table userIdTable)
        {
            foreach (var row in userIdTable.Rows)
            {
                var userId = row.Values.ToList()[0];
                string endpoint = $"/users/{userId}";
                _apiResponse.Response = RestHelper.GetRequest(endpoint);             
                Assert.AreEqual(expectedStatusCode, Convert.ToInt32(_apiResponse.Response.StatusCode), "The Status code is not as expected code: "+ expectedStatusCode);
            }
        }
    }
}