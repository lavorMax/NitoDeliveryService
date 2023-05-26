using Microsoft.EntityFrameworkCore;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using System;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture
{
    public class PlaceManagementDbContextFactory : IOverridingDbContextFactory<PlaceManagementDbContext>
    {
        private PlaceManagementDbContext _scopedContext;
        private readonly IAuth0Client _authClient;
        private int _overrideClientId = -1;

        public PlaceManagementDbContextFactory(IAuth0Client authClient)
        {
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
                var overrideOptionsBuilder = new DbContextOptionsBuilder<PlaceManagementDbContext>()
                .UseSqlServer($"Data Source=.\\SQLEXPRESS;Initial Catalog=ClientDB{_overrideClientId};Integrated Security=True;MultipleActiveResultSets=True;");

                _scopedContext = new PlaceManagementDbContext(overrideOptionsBuilder.Options);

                return _scopedContext;
            }

            var userMetadata = _authClient.GetMetadata().Result;

            var optionsBuilder = new DbContextOptionsBuilder<PlaceManagementDbContext>()
                    .UseSqlServer($"Data Source=.\\SQLEXPRESS;Initial Catalog=ClientDB{userMetadata.ClientId};Integrated Security=True;MultipleActiveResultSets=True;");

            _scopedContext = new PlaceManagementDbContext(optionsBuilder.Options);

            return _scopedContext;
        }

        public void OverrideClientId(int clientId)
        {
            _overrideClientId = clientId;
        }
    }
}
