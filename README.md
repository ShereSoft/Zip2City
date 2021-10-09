# Zip2City
Provides fast, in-memory lookup of the postal service city/state names by zip code. All 50 states, DC, PR, VI, and AE. Data current as of September 28, 2021. No external library dependencies. No external calls.

[![](https://img.shields.io/nuget/v/Zip2City.svg)](https://www.nuget.org/packages/Zip2City/)
[![](https://img.shields.io/nuget/dt/Zip2City)](https://www.nuget.org/packages/Zip2City/)

* Light-weight (430KB)
* No external library dependencies
* No external calls

### .GetDefaultCityState(zipcode)
```csharp
var zipcode = "90210";
var cityAndState = Zip2City.GetDefaultCityState(zipcode);  // returns null when there's no data

var city = cityAndState[0];  // "BEVERLY HILLS"
var state = cityAndState[1];  // "CA"
```
