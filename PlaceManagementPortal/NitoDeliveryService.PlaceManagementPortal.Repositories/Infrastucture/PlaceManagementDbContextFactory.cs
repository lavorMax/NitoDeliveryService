using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture
{
    public class PlaceManagementDbContextFactory : IOverridingDbContextFactory<PlaceManagementDbContext>
    {
        private PlaceManagementDbContext _scopedContext;

        private readonly string _connectionStringTemplate;
        private readonly IAuth0Client _authClient;

        private int _overrideClientId = -1;

        public PlaceManagementDbContextFactory(IAuth0Client authClient, IConfiguration configuration)
        {
            _connectionStringTemplate = configuration.GetConnectionString("PlaceManagementDbConnection");
            _authClient = authClient;
        }

        public PlaceManagementDbContext CreateDbContext()
        {
            if (_scopedContext != null)
            {
                return _scopedContext;
            }

            if (_overrideClientId != -1)
            {
                var overridenConnectionString = string.Format(_connectionStringTemplate, _overrideClientId);

                var overrideOptionsBuilder = new DbContextOptionsBuilder<PlaceManagementDbContext>()
                .UseSqlServer(overridenConnectionString);

                _scopedContext = new PlaceManagementDbContext(overrideOptionsBuilder.Options);

                return _scopedContext;
            }

            var userMetadata = _authClient.GetMetadata().Result;

            var connectionString = string.Format(_connectionStringTemplate, userMetadata.ClientId);

            var optionsBuilder = new DbContextOptionsBuilder<PlaceManagementDbContext>()
                    .UseSqlServer(connectionString);

            _scopedContext = new PlaceManagementDbContext(optionsBuilder.Options);

            return _scopedContext;
        }

        public void OverrideClientId(int clientId)
        {
            _overrideClientId = clientId;
        }
    }
}
