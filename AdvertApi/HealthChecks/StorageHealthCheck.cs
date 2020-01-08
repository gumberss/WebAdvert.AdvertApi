using AdvertApi.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace AdvertApi.HealthChecks
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IAdvertStorageService _storageService;

        public StorageHealthCheck(IAdvertStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var isStorageOk = await _storageService.CheckHealthAsync();

            return isStorageOk 
                ? HealthCheckResult.Healthy("The API is working fine!")
                : HealthCheckResult.Unhealthy("API is down!");
        }
    }
}
