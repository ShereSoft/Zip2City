using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ShereSoft;

namespace Zip2CityTests
{
    [TestClass]
    public class Zip2CityTests
    {
        [TestMethod]
        public void GetDefaultCityState_ForValidZipCode_ReturnsDefaultCityAndState()
        {
            var data = new[]
            {
                new[] { "02495", "NONANTUM", "MA" },
                new[] { "12580", "STAATSBURG", "NY" },
                new[] { "13626", "COPENHAGEN", "NY" },
                new[] { "13803", "MARATHON", "NY" },
                new[] { "14029", "CENTERVILLE", "NY" },
                new[] { "16001", "BUTLER", "PA" },
                new[] { "16820", "AARONSBURG", "PA" },
                new[] { "18448", "OLYPHANT", "PA" },
                new[] { "18901", "DOYLESTOWN", "PA" },
                new[] { "22081", "MERRIFIELD", "VA" },
                new[] { "24038", "ROANOKE", "VA" },
                new[] { "24133", "PATRICK SPRINGS", "VA" },
                new[] { "24830", "ELBERT", "WV" },
                new[] { "32313", "TALLAHASSEE", "FL" },
                new[] { "34205", "BRADENTON", "FL" },
                new[] { "35976", "GUNTERSVILLE", "AL" },
                new[] { "39366", "VOSSBURG", "MS" },
                new[] { "42058", "LEDBETTER", "KY" },
                new[] { "45883", "SAINT HENRY", "OH" },
                new[] { "49876", "QUINNESEC", "MI" },
                new[] { "51239", "HULL", "IA" },
                new[] { "52805", "DAVENPORT", "IA" },
                new[] { "53801", "BAGLEY", "WI" },
                new[] { "55302", "ANNANDALE", "MN" },
                new[] { "57278", "WILLOW LAKE", "SD" },
                new[] { "60544", "PLAINFIELD", "IL" },
                new[] { "62638", "FRANKLIN", "IL" },
                new[] { "70342", "BERWICK", "LA" },
                new[] { "73840", "FARGO", "OK" },
                new[] { "74947", "MONROE", "OK" },
                new[] { "76472", "SANTO", "TX" },
                new[] { "76949", "SILVER", "TX" },
                new[] { "82646", "NATRONA", "WY" },
                new[] { "83539", "KOOSKIA", "ID" },
                new[] { "85318", "GLENDALE", "AZ" },
                new[] { "85710", "TUCSON", "AZ" },
                new[] { "86331", "JEROME", "AZ" },
                new[] { "87537", "HERNANDEZ", "NM" },
                new[] { "90003", "LOS ANGELES", "CA" },
                new[] { "96094", "WEED", "CA" },
                new[] { "97016", "CLATSKANIE", "OR" },
            };

            var errors = new List<string>();

            foreach (var d in data)
            {
                var result = Zip2City.GetDefaultCityState(d[0]);

                if (result == null || result.Length != 2 || result[0] != d[1] || result[1] != d[2])
                {
                    errors.Add($"ZIP Code '{d[0]}' should return '{d[1]}' (city) and '{d[2]}' (state), instead of {result[0]}, {result[1]}");
                }
            }

            Assert.AreEqual(0, errors.Count, String.Join("\r\n", errors));
        }

        [TestMethod]
        public void GetDefaultCityState_ForNonExistentZipCode_ReturnsNull()
        {
            var result = Zip2City.GetDefaultCityState("99999");
            Assert.IsNull(result, "Result should be null.");
        }

        [TestMethod]
        public void GetClosestCityState_ForNonExistentZipCode_ReturnsClosestMatch()
        {
            var result = Zip2City.GetClosestCityState("99999");
            Assert.IsNotNull(result, "Result should not be null.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDefaultCityState_ForInvalidZipCodeLength_ThrowsArgumentException()
        {
            Zip2City.GetDefaultCityState("1234");
            Assert.Fail("Method should throw an ArgumentException.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetDefaultCityState_ForInvalidCharacter_ThrowsArgumentException()
        {
            Zip2City.GetDefaultCityState("12E45");
            Assert.Fail("Method should throw an ArgumentException.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDefaultCityState_ForNull_ThrowsArgumentNullException()
        {
            Zip2City.GetDefaultCityState(null);
            Assert.Fail("Method should throw an ArgumentNullException.");
        }

        [TestMethod]
        public void GetAllCityStates_NeverReturnsNull()
        {
            var result = Zip2City.GetAllCityStates("99999");

            Assert.IsNotNull(result);
            Console.WriteLine(JsonSerializer.Serialize(new string[0][]));
        }

        [TestMethod]
        public void GetAllCityStates_ReturnsCityStateZip()
        {
            var result = Zip2City.GetClosestCityStates("99999");

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void GetAllCityStates_ForExistentZipCode_ReturnsOneOrMoreSets()
        {
            var result = Zip2City.GetAllCityStates("75261");

            Assert.IsInstanceOfType(result, typeof(IEnumerable<string[]>));

            foreach (var cityState in result)
            {
                Assert.IsNotNull(cityState[0], "It should have a city name.");
                Assert.AreEqual(2, cityState[1].Length, "It should be a state code");
            }

            Console.WriteLine(JsonSerializer.Serialize(result));
        }

        [TestMethod]
        public void GetRandomCityStateZip_ReturnsRandomSet()
        {
            var randomCityState = Zip2City.GetRandomCityStateZip();

            Assert.IsNotNull(randomCityState);
            Assert.IsInstanceOfType(randomCityState, typeof(string[]));

            Assert.IsNotNull(randomCityState[0], "It should have a city name.");
            Assert.AreEqual(2, randomCityState[1].Length, "It should be a state code");
            Assert.AreEqual(5, randomCityState[2].Length, "It should have a zip code.");

            Console.WriteLine(JsonSerializer.Serialize(randomCityState));
        }

        [TestMethod]
        public void GetRandomCityStateZip_ReturnsRandomSetWithRandomizer()
        {
            var randomCityState = Zip2City.GetRandomCityStateZip(new Random());

            Assert.IsNotNull(randomCityState);
            Assert.IsInstanceOfType(randomCityState, typeof(string[]));

            Assert.IsNotNull(randomCityState[0], "It should have a city name.");
            Assert.AreEqual(2, randomCityState[1].Length, "It should be a state code");
            Assert.AreEqual(5, randomCityState[2].Length, "It should have a zip code.");

            Console.WriteLine(JsonSerializer.Serialize(randomCityState));
        }

        [TestMethod]
        public void GetRandomCityStateZip_ReturnsSameSetWithSameRandomizer()
        {
            var seed = 123;
            var randomCityState1 = Zip2City.GetRandomCityStateZip(new Random(seed)).ToArray();

            Assert.IsNotNull(randomCityState1);

            var randomCityState2 = Zip2City.GetRandomCityStateZip(new Random(seed)).ToArray();

            Assert.IsNotNull(randomCityState2);

            Assert.IsTrue(randomCityState1.SequenceEqual(randomCityState2));
        }

        [TestMethod]
        [DataRow("77449")]
        [DataRow("11368")]
        [DataRow("60629")]
        [DataRow("79936")]
        [DataRow("90011")]
        [DataRow("92335")]
        [DataRow("08701")]
        [DataRow("75034")]
        [DataRow("37013")]
        [DataRow("94565")]
        [DataRow("78521")]
        [DataRow("30044")]
        public async Task GetDefaultCityState_Matches_CurrentDataAsync(string zipcode)
        {
            var defaultCityState = Zip2City.GetDefaultCityState(zipcode);

            try
            {
                var hc = new HttpClient();
                var content = new StringContent("zip=" + zipcode);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var response = await hc.PostAsync("https://tools.usps.com/tools/app/ziplookup/cityByZip", content);
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<result>(json);

                if (defaultCityState[0] != result.defaultCity)
                {
                    Assert.Fail($"Actual default city is {result.defaultCity} instead of {defaultCityState[0]}.");
                }

                if (defaultCityState[1] != result.defaultState)
                {
                    Assert.Fail($"Actual default state is {result.defaultState} instead of {defaultCityState[1]}.");
                }
            }
            catch (WebException ex)
            {
                Assert.Fail("Failed to retrieve data", ex);
            }
        }

        class result
        {
            public string resultStatus { get; set; }
            public string zip5 { get; set; }
            public string defaultCity { get; set; }
            public string defaultState { get; set; }
            public string defaultRecordType { get; set; }
            public listItem[] citiesList { get; set; }
            public listItem[] nonAcceptList { get; set; }

            public class listItem
            {
                public string city { get; set; }
                public string state { get; set; }

            }
        }
    }
}
