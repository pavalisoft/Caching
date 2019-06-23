# Caching
[Pavalisoft.Caching](https://www.nuget.org/packages/Pavalisoft.Caching/) is an open source caching extension for .NET Standard written in C#, which provides single unified API for both [MemoryCache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-2.2) and [DistributedCache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-2.2) implementations.

The main goal of the [Pavalisoft.Caching](https://www.nuget.org/packages/Pavalisoft.Caching/) package is to make developer's life easier to handle even very complex caching scenarios and concentrate on functionality. It's additional feature [CacheManager](https://pavalisoft.github.io/Caching/class_pavalisoft_1_1_caching_1_1_cache_manager.html) supports various cache providers and implements many advanced features which can be used in single project/application.

With [Pavalisoft.Caching](https://www.nuget.org/packages/Pavalisoft.Caching/), it is possible to implement multiple layers of caching with multiple cache providers in one place, e.g. [MemoryCache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-2.2) and [DistributedCache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-2.2), in just a few lines of code.

The below diagram explains the [Pavalisoft.Caching](https://www.nuget.org/packages/Pavalisoft.Caching/) API and its usage.
![](CacheManager.svg)

## Documentation & Samples
Complete Documentation is available at https://pavalisoft.github.io/Caching/ for [Pavalisoft.Caching](https://github.com/pavalisoft/Caching) API
Refer https://github.com/pavalisoft/Caching/tree/master/Samples for reference implementations

## Cache Manager Usage

1. Define the [Cache Stores](https://pavalisoft.github.io/Caching/class_pavalisoft_1_1_caching_1_1_cache_settings.html#abacbfb422d22fd66190f2350901b8797) and [Partitions](https://pavalisoft.github.io/Caching/class_pavalisoft_1_1_caching_1_1_cache_settings.html#a132238e26cb3fd3005fb2ebdcc36a36f) in Caching configuration section in appSettings.json.

```json
{
  "Caching": {
    "Stores": [
      {
        "Name": "InMemory",
        "Type": "Pavalisoft.Caching.InMemory.InMemoryCacheStoreType, Pavalisoft.Caching.InMemory",
        "StoreConfig": "{\"ExpirationScanFrequency\":\"00:05:00\"}"
      },
      {
        "Name": "DistributedInMemory",
        "Type": "Pavalisoft.Caching.InMemory.MemoryDistributedCacheStoreType,Pavalisoft.Caching.InMemory",
        "SerializerType": "Pavalisoft.Caching.Serializers.JsonSerializer,Pavalisoft.Caching",
        "StoreConfig": "{\"ExpirationScanFrequency\":\"00:05:00\"}"
      },
      {
        "Name": "SqlServer",
        "Type": "Pavalisoft.Caching.SqlServer.SqlServerDistributedCacheStoreType,Pavalisoft.Caching.SqlServer",
        "SerializerType": "Pavalisoft.Caching.Serializers.JsonSerializer,Pavalisoft.Caching",
        "StoreConfig": "{\"ExpiredItemsDeletionInterval\":\"00:05:00\", \"ConnectionString\":\"Data Source=localhost;Initial Catalog=DistributedCache;Integrated Security=True\", \"SchemaName\":\"store\", \"TableName\":\"Cache\", \"DefaultSlidingExpiration\":\"00:05:00\"}"
      },
      {
        "Name": "MySql",
        "Type": "Pavalisoft.Caching.MySql.MySqlDistributedCacheStoreType,Pavalisoft.Caching.MySql",
        "SerializerType": "Pavalisoft.Caching.Serializers.JsonSerializer,Pavalisoft.Caching",
        "StoreConfig": "{\"ExpiredItemsDeletionInterval\":\"00:05:00\", \"ConnectionString\":\"Data Source=localhost;User Id=root;Password=root;Allow User Variables=true\", \"SchemaName\":\"store\", \"TableName\":\"Cache\", \"DefaultSlidingExpiration\":\"00:05:00\"}"
      },
      {
        "Name": "Redis",
        "Type": "Pavalisoft.Caching.Redis.RedisDistributedCacheStoreType,Pavalisoft.Caching.Redis",
        "SerializerType": "Pavalisoft.Caching.Serializers.JsonSerializer,Pavalisoft.Caching",
        "StoreConfig": "{\"Configuration\":\"00:05:00\", \"InstanceName\":\"localhost\"}"
      }
    ],
    "Partitions": [
      {
        "Name": "FrequentData",
        "StoreName": "InMemory",
        "SlidingExpiration": "00:05:00"
      },
      {
        "Name": "DistributedFrequentData",
        "StoreName": "DistributedInMemory",
        "SlidingExpiration": "00:05:00"
      },
      {
        "Name": "MySqlLocalizationData",
        "StoreName": "MySql",
        "Priority": "NeverRemove"
      },
      {
        "Name": "LocalizationData",
        "StoreName": "SqlServer",
        "Priority": "NeverRemove"
      },
      {
        "Name": "MasterData",
        "StoreName": "Redis",
        "SlidingExpiration": "00:05:00"
      }
    ]
  }
}
```

2. Add [Pavalisoft.Caching](https://www.nuget.org/packages/Pavalisoft.Caching/) to services. Also add the [Pavalisoft.Caching.InMemory](https://www.nuget.org/packages/Pavalisoft.Caching.InMemory/) , [Pavalisoft.Caching.MySql](https://www.nuget.org/packages/Pavalisoft.Caching.MySql/) , [Pavalisoft.Caching.Redis](https://www.nuget.org/packages/Pavalisoft.Caching.Redis/) and [Pavalisoft.Caching.SqlServer](https://www.nuget.org/packages/Pavalisoft.Caching.SqlServer/) Cache store implementations to services based on usage.

```csharp
...
//Import the below namespace to use InMemory and DitributedInMemory cache store implementations
using Pavalisoft.Caching.InMemory;
//Import the below namespace to use MySql cache store implementation
using Pavalisoft.Caching.MySql;
//Import the below namespace to use Redis Cache Store implementation
using Pavalisoft.Caching.Redis;
//Import the below namespace to use SqlServer Cache Store implementation
using Pavalisoft.Caching.SqlServer;
...

namespace Pavalisoft.Caching.Sample
{
    public class Startup
    {
		...
        public Startup(IConfiguration configuration)
        {
			...
            Configuration = configuration;
			...
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ...
            // Adds CacheManager servcice to services
            services.AddCaching()
                // Adds InMemory and Distributed InMemory Cache Store implementations to CacheManager
                .AddInMemoryCache()
                // Adds MySql Cache Store implementations to CacheManager
                .AddMySqlCache()
                // Adds Redis Cache Store implementations to CacheManager
                .AddRedisCache()
                // Adds SqlServer Cache Store implementations to CacheManager
                .AddSqlServerCache();
			...
        }
		...
    }
}
```

3. Use [CacheManager](https://pavalisoft.github.io/Caching/class_pavalisoft_1_1_caching_1_1_cache_manager.html) methods to add, get, refresh and remove items in Cache.

```csharp
// Add required using statements
using Pavalisoft.Caching;
using Pavalisoft.Caching.Interfaces;
using Microsoft.Extensions.Primitives;
using System;

namespace Pavalisoft.Caching.Sample
{
	public class CachingSample
	{
		private const string CachePartitionName = "FrequentData";		
		private readonly ICacheManager _cacheManager;
		public CachingSample(ICacheManager cacheManager)
		{
			_cacheManager = cacheManager;
		}

		public AppUser GetAppUser(HttpContext httpContext)
		{
			var userName = httpContext.User.Identity.Name;
			AppUser appUser;
			
			// Try to get the appUser from cache
			if (!_cacheManager.TryGetValue(CachePartitionName, userName, out appUser))
			{
				// If not available in Cache then create new instance of AppUser
				appUser = new AppUser(userName);

				// Add appUser object to Cache
				_cacheManager.Set(CachePartitionName, userName, appUser);                `
			}
			return appUser;
		}
	}
}
```

## Cache Tag Helper Usage

1. Define the [Cache Stores](https://pavalisoft.github.io/Caching/class_pavalisoft_1_1_caching_1_1_cache_settings.html#abacbfb422d22fd66190f2350901b8797) and [Partitions](https://pavalisoft.github.io/Caching/class_pavalisoft_1_1_caching_1_1_cache_settings.html#a132238e26cb3fd3005fb2ebdcc36a36f) in Caching configuration section in appSettings.json.

```json
{
  "Caching": {
    "Stores": [
      {
        "Name": "InMemory",
        "Type": "Pavalisoft.Caching.InMemory.InMemoryCacheStoreType, Pavalisoft.Caching.InMemory",
        "StoreConfig": "{\"ExpirationScanFrequency\":\"00:05:00\"}"
      },
      {
        "Name": "DistributedInMemory",
        "Type": "Pavalisoft.Caching.InMemory.MemoryDistributedCacheStoreType,Pavalisoft.Caching.InMemory",
        "SerializerType": "Pavalisoft.Caching.Serializers.JsonSerializer,Pavalisoft.Caching",
        "StoreConfig": "{\"ExpirationScanFrequency\":\"00:05:00\"}"
      },
      {
        "Name": "SqlServer",
        "Type": "Pavalisoft.Caching.SqlServer.SqlServerDistributedCacheStoreType,Pavalisoft.Caching.SqlServer",
        "SerializerType": "Pavalisoft.Caching.Serializers.JsonSerializer,Pavalisoft.Caching",
        "StoreConfig": "{\"ExpiredItemsDeletionInterval\":\"00:05:00\", \"ConnectionString\":\"Data Source=localhost;Initial Catalog=DistributedCache;Integrated Security=True\", \"SchemaName\":\"store\", \"TableName\":\"Cache\", \"DefaultSlidingExpiration\":\"00:05:00\"}"
      },
      {
        "Name": "MySql",
        "Type": "Pavalisoft.Caching.MySql.MySqlDistributedCacheStoreType,Pavalisoft.Caching.MySql",
        "SerializerType": "Pavalisoft.Caching.Serializers.JsonSerializer,Pavalisoft.Caching",
        "StoreConfig": "{\"ExpiredItemsDeletionInterval\":\"00:05:00\", \"ConnectionString\":\"Data Source=localhost;User Id=root;Password=root;Allow User Variables=true\", \"SchemaName\":\"store\", \"TableName\":\"Cache\", \"DefaultSlidingExpiration\":\"00:05:00\"}"
      },
      {
        "Name": "Redis",
        "Type": "Pavalisoft.Caching.Redis.RedisDistributedCacheStoreType,Pavalisoft.Caching.Redis",
        "SerializerType": "Pavalisoft.Caching.Serializers.JsonSerializer,Pavalisoft.Caching",
        "StoreConfig": "{\"Configuration\":\"00:05:00\", \"InstanceName\":\"localhost\"}"
      }
    ],
    "Partitions": [
      {
        "Name": "FrequentData",
        "StoreName": "InMemory",
        "SlidingExpiration": "00:05:00"
      },
      {
        "Name": "DistributedFrequentData",
        "StoreName": "DistributedInMemory",
        "SlidingExpiration": "00:05:00"
      },
      {
        "Name": "MySqlLocalizationData",
        "StoreName": "MySql",
        "Priority": "NeverRemove"
      },
      {
        "Name": "LocalizationData",
        "StoreName": "SqlServer",
        "Priority": "NeverRemove"
      },
      {
        "Name": "MasterData",
        "StoreName": "Redis",
        "SlidingExpiration": "00:05:00"
      }
    ]
  }
}
```

2. Add [Pavalisoft.Caching](https://www.nuget.org/packages/Pavalisoft.Caching/) to services. Also add the [Pavalisoft.Caching.InMemory](https://www.nuget.org/packages/Pavalisoft.Caching.InMemory/) , [Pavalisoft.Caching.MySql](https://www.nuget.org/packages/Pavalisoft.Caching.MySql/) , [Pavalisoft.Caching.Redis](https://www.nuget.org/packages/Pavalisoft.Caching.Redis/) and [Pavalisoft.Caching.SqlServer](https://www.nuget.org/packages/Pavalisoft.Caching.SqlServer/) Cache store implementations to services based on usage.

```csharp
...
//Import the below namespace to use InMemory and DitributedInMemory cache store implementations
using Pavalisoft.Caching.InMemory;
//Import the below namespace to use MySql cache store implementation
using Pavalisoft.Caching.MySql;
//Import the below namespace to use Redis Cache Store implementation
using Pavalisoft.Caching.Redis;
//Import the below namespace to use SqlServer Cache Store implementation
using Pavalisoft.Caching.SqlServer;
...

namespace Pavalisoft.Caching.Sample
{
    public class Startup
    {
		...
        public Startup(IConfiguration configuration)
        {
			...
            Configuration = configuration;
			...
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ...
            // Adds CacheManager servcice to services
            services.AddCaching()
                // Adds InMemory and Distributed InMemory Cache Store implementations to CacheManager
                .AddInMemoryCache()
                // Adds MySql Cache Store implementations to CacheManager
                .AddMySqlCache()
                // Adds Redis Cache Store implementations to CacheManager
                .AddRedisCache()
                // Adds SqlServer Cache Store implementations to CacheManager
                .AddSqlServerCache();
			...
        }
		...
    }
}
```

3. To use `pavalisoft-cache` tag helper, add the below to _ViewImport.cshtml

```csharp
@addTagHelper *, Pavalisoft.Caching.TagHelpers
```

4. Use the `pavalisoft-cache` tag helper in the view wherever required.

```html
<div class="col-md-12">
    <h2>Pavalisoft Cache TagHelper</h2>
    <div class="row">
        <pavalisoft-cache cache-partition="FrequentData">
            This will be cached in server memory. expires-after 05 minutes
        </pavalisoft-cache>
    </div>
    <div class="row">
        <pavalisoft-cache cache-partition="DistributedFrequentData">
            This will be cached in server distributed memory. expires-sliding (05 minutes)
        </pavalisoft-cache>
    </div>
    <div class="row">
        <pavalisoft-cache cache-partition="MySqlLocalizationData">
            This will be cached in MySQL distributed memory. expires-after 05 minutes
        </pavalisoft-cache>
    </div>
    <div class="row">
        <pavalisoft-cache cache-partition="LocalizationData">
            This will be cached in Sql Server distributed memory. expires-after 05 minutes
        </pavalisoft-cache>
    </div>
    <div class="row">
        <pavalisoft-cache cache-partition="MasterData">
            This will be cached in Redis distributed memory. expires-sliding (05 minutes)
        </pavalisoft-cache>
    </div>
</div>
```

## Serializers
Below are the list of serializers options available, which can be used to serialize and deserialize object from and to Distributed Cache Stores.

- [**BinaryFormatter**](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.formatters.binary.binaryformatter?view=netstandard-2.0) - Requires the classes decoarated with `SerializableAttribute` to store into Distributed Cache Stores.
- [**Newtonsoft.Json**](https://github.com/JamesNK/Newtonsoft.Json) - Uses Newtonsoft.Json to serialize a class without `SerializableAttribute`.

## Builds
Get latest builds from [nuget](https://www.nuget.org/packages/Pavalisoft.Caching/)

| Package | Version |
| :--- | :---: |
| [Pavalisoft.Caching](https://github.com/pavalisoft/Caching/tree/master/Source/Pavalisoft.Caching) | [1.2](https://www.nuget.org/packages/Pavalisoft.Caching/1.2) |
| [Pavalisoft.Caching.TagHelpers](https://github.com/pavalisoft/Caching/tree/master/Source/Pavalisoft.Caching.TagHelpers/) | [1.0](https://www.nuget.org/packages/Pavalisoft.Caching.TagHelpers/1.0) |
| [Pavalisoft.Caching.InMemory](https://github.com/pavalisoft/Caching/tree/master/Source/Pavalisoft.Caching.InMemory/) | [1.0](https://www.nuget.org/packages/Pavalisoft.Caching.InMemory/1.0) |
| [Pavalisoft.Caching.Redis](https://github.com/pavalisoft/Caching/tree/master/Source/Pavalisoft.Caching.Redis/) | [1.0](https://www.nuget.org/packages/Pavalisoft.Caching.Redis/1.0) |
| [Pavalisoft.Caching.MySql](https://github.com/pavalisoft/Caching/tree/master/Source/Pavalisoft.Caching.MySql/) | [1.0](https://www.nuget.org/packages/Pavalisoft.Caching.MySql/1.0) |
| [Pavalisoft.Caching.SqlServer](https://github.com/pavalisoft/Caching/tree/master/Source/Pavalisoft.Caching.SqlServer/) | [1.0](https://www.nuget.org/packages/Pavalisoft.Caching.SqlServer/1.0) |

## Contributing
**Getting started with Git and GitHub**

 * [Setting up Git for Windows and connecting to GitHub](http://help.github.com/win-set-up-git)
 * [Forking a GitHub repository](http://help.github.com/fork-a-repo)
 * [The simple guide to GIT guide](http://rogerdudler.github.com/git-guide)
 * [Open an issue](https://github.com/pavalisoft/caching/issues) if you encounter a bug or have a suggestion for improvements/features

Once you're familiar with Git and GitHub, clone the repository and start contributing.
