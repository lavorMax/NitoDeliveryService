using Microsoft.EntityFrameworkCore;
using NitoDeliveryService.PlaceManagementPortal.Repositories.Interfaces;
using System;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture
{
    public class PlaceManagementDbContextFactory : IOverridingDbContextFactory<PlaceManagementDbContext>
    {
        private readonly ITokenParser _tokenParser;
        private int _overrideClientId = -1;

        public PlaceManagementDbContextFactory(ITokenParser tokenParser)
        {
            _tokenParser = tokenParser;
        }

        public PlaceManagementDbContext CreateDbContext()
        {
            try
            {
                if (_overrideClientId != -1)
                {
                    var overrideOptionsBuilder = new DbContextOptionsBuilder<PlaceManagementDbContext>()
                    .UseSqlServer($"Data Source=.\\SQLEXPRESS;Initial Catalog=ClientDB{_overrideClientId};Integrated Security=True;MultipleActiveResultSets=True;");

                    return new PlaceManagementDbContext(overrideOptionsBuilder.Options);
                }

                var dbName = _tokenParser.GetMetadata().ClientId;

                var optionsBuilder = new DbContextOptionsBuilder<PlaceManagementDbContext>()
                    .UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database={dbName};Trusted_Connection=True;MultipleActiveResultSets=true");

                return new PlaceManagementDbContext(optionsBuilder.Options);
            }catch(Exception e)
            {
                throw;
            }
        }

        public void OverrideClientId(int clientId)
        {
            _overrideClientId = clientId;
        }
    }
}
