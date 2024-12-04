# Answers to technical questions

## 1. How long did you spend on the coding assignment? What would you add to your solution if you had more time? If you didn't spend much time on the coding assignment then use this as an opportunity to explain what you would add.
In total coding I've probably spend about 7-8 hours (including this list of answers). The assignment is simple, but many things can
be added.
This is the type of assignment where you could spend multiple days working on it and still find things to do.

Things that could be considered:
- Authentication/Authorization - important for security. I could have added the simplest JWT implementation and pass JWTs generated with the `dotnet user-jwts` command (for development purposes), 
	 but in practice Authentication/Authorization is usually more complicated and depends on business requirements. 
	 For example, the 2 APIs provided use API Key Authentication and they also use the key for authorization.
-  Rate limiting - for many APIs making sure the endpoint don't get abused is important. A very simple, but also limited solution can be easily implemented with the `Microsoft.AspNetCore.RateLimiting` middleware in ASP.NET Core. 
	This is one of the options available for implementing this. If the API is exposed through an API Gateway (eg: Azure API Management) rate limiting can be implemented through policies at that level and prevent the calls before they reach the API instance.					
- Observability - the solution is already using the .NET Core `ILogger` - this is a good start to capture logs and exceptions. Based on existing team choices we can extend this logging and integrate with log analytics tool which will make 
	checking issues and debugging easier. We could export our logs to Azure AppInsights for example or if we choose we could integrate with the new Open Telemetry standard where we could have multiple exporters (Jaeger, Zipkin, Prometheus etc)  
- Versioning - this is a simple API with one endpoint at the moment. For larger API that keep evolving and with multiple consumers, versioning is a best practice that allows us to release possibly breaking changes without affecting the consumers.
- Caching - based on the needs (business requirements) caching could be added. .NET Core 9 introduces the `HybridCache` which is brings together the older `IMemoryCache` and `IDistributedCache`. This can improve performance and also decrease the number of HTTP calls to the chosen API.
- CI/CD
- IaC - depends on the deployment choices. Could be Terraform (multiple providers available) or Bicep (or directly ARM templates) for Azure for example. 


## 2. What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it.
Not sure if it's the most useful feature, but it's a handy one (given how many implementations are online) - C# 9 now implements the UUID V7.
So you need a unique, random key that would still allows you sort the data time based you can now get a V7 UUID without additional libraries.

```
Guid.CreateVersion7();
```

**NOTE:** not used in the project because I didn't have to. Not generating data that might be stored in a DB.

## 3. How would you track down a performance issue in production? Have you ever had to do this?
The first step to even approach such a task is to have implemented observability. 
At the previous job we used Azure AppInsights to trace the calls and try to understand where the bottleneck is.
One example of identifying and solving a production performance is experiencing slow response times from a search endpoint for one of the APIs.
I've used AppInsights to trace the calls and after looking at the dependencies and the duration of the calls it became clear that the bottleneck was the database.
The search queries were not optimized so the solution was to create some DB indexes.

## 4. What was the latest technical book you have read or tech conference you have been to? What did you learn?
On 10th of December I will go to [AI Community Day](https://www.aicommunityday.nl/information/) in Utrecht.
A past event that I've participated this year was a meetup organized by the "Dutch Azure Meetup" group - [Azure vs AWS: Battle of the Clouds](https://www.meetup.com/dutch-azure-meetup/events/300326315/).
The take away from the meetup discussion is that in terms of services, data centers, availability zones etc Azure and AWS are very much comparable.
The only difference seemed to be that Azure is maybe more developer friendly.

## 5. What do you think about this technical assessment?
Simple, but can lead to a lot of discussion points.
The 2 provided APIs are a bit confusing - ExchangeRatesAPI is not a ver RESTful API (returning 200 OK when it should have returned a error code), CoinMarketCap API looked better, but is double the price for the entry level paid access.


## 6. Please, describe yourself using JSON.

```
{
   "firstName": "Andrei",
   "lastName": "Croitoriu",
   "nationality": "Romanian",
   "dateOfBirth": "1982-05-01",
   "summary": "Romanian software engineer born and raised in Suceava, Romania, have 2 sisters - one living in Bucharest, Romania and the other in Nassjo, Sweden. Lived and worked for 7 years in Cyprus and in 2014 moved to The Netherlands. Living in Leiden with my Polish girlfriend with whom I've been with for over 14 years.",
   "links": [   
	"https://www.linkedin.com/in/acroitoriu/",
	"https://www.instagram.com/acroitoriu/",
	"https://www.strava.com/athletes/acroitoriu"
   ],
   "favoriteFood": "sushi",
   "favoriteDrink": "Matcha latte",
   "numberOfBikes": 3,
   "numberOfCountriesVisited": 28,
   "favoriteMusicGenre": "rock",
   "favoriteBand": "Queen",
   "favoriteMovie": "The Big Lebowski",
   "favoriteSportToPractice": "running",
   "favoriteSportToWatch": "mountain biking"   
}
```