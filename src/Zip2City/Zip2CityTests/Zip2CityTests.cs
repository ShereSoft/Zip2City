using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
                    errors.Add($"ZIP Code '{d[0]}' should return '{d[1]}' (city) and '{d[2]}' (state).");
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
    }
}
