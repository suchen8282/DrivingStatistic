
Assumsion:
* Time zone is ignored.
* Country broader is not changed.

Left out:
1. API Input Validation
* Valid all input parameters on RestAPI, fx. Put range on properties in interface class, otherwise add validation on controller. Clean up input parameter validation on BLL.
* Add annotation “Age above” in interface class.
* Define month as string instead of integer in DistanceQuery.

2.  GPS Validation
* Create ValidateDriverCurrentLocationAsync() to remove wrong location data in database before calculation. If the currentLocation is too much difference with the same drivers previous timestamp, remove that record from database and exclude it from calculation. ValidateDriverCurrentLocationAsync() should placed in background worker as stated later.

3. Finish external API
* Current API returned 403 forbidden at the moment and only accept one request per second. Find another external API call, add retry if it failed.

4. Create Index
* Create index for DriverId, IsCountrySoleved and IsDriverAgeResolved.
* Create Composite index for DriverAgeAtTimeStamp, Country and TimeStamp. 

5. Create BackgroundService
* Create ResolveStatsBackGroundWorker to ResolveCountryAsync(),  ResolveAgeAsync() and ValidateDriverCurrentLocationAsync() every 5 mins. 

6. Unit test
