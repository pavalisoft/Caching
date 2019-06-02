using System;
using System.Collections.Generic;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace Pavalisoft.Caching.Stores
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomCacheOptions : Dictionary<string, object>, IOptions<CustomCacheOptions>
    {
        private double _compactionPercentage = 0.05;
        private long? _sizeLimit;

        /// <summary>
        /// 
        /// </summary>
        public ISystemClock Clock { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of time between successive scans for expired items.
        /// </summary>
        public TimeSpan ExpirationScanFrequency { get; set; } = TimeSpan.FromMinutes(1.0);

        /// <summary>Gets or sets the maximum size of the cache.</summary>
        public long? SizeLimit
        {
            get => _sizeLimit;
            set
            {
                long? nullable = value;
                long num = 0;
                if (nullable.GetValueOrDefault() < num & nullable.HasValue)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "value must be non-negative.");
                _sizeLimit = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount to compact the cache by when the maximum size is exceeded.
        /// </summary>
        public double CompactionPercentage
        {
            get => _compactionPercentage;
            set
            {
                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException(nameof(value), value, "value must be between 0 and 1 inclusive.");
                _compactionPercentage = value;
            }
        }

        CustomCacheOptions IOptions<CustomCacheOptions>.Value => this;
    }
}
