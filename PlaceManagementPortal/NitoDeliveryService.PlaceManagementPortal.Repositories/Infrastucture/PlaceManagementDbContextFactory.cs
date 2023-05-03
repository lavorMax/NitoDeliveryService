using Microsoft.EntityFrameworkCore;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture
{
    public class PlaceManagementDbContextFactory : IDbContextFactory<PlaceManagementDbContext>
    {
        private readonly ITokenParser _tokenParser;
        private int _overrideClientId = -1;

        public PlaceManagementDbContextFactory(ITokenParser tokenParser)
        {
            _tokenParser = tokenParser;
        }

        public PlaceManagementDbContext CreateDbContext()
        {
            if(_overrideClientId != -1)
            {
                var overrideOptionsBuilder = new DbContextOptionsBuilder<PlaceManagementDbContext>()
                .UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database={_overrideClientId};Trusted_Connection=True;MultipleActiveResultSets=true");

                return new PlaceManagementDbContext(overrideOptionsBuilder.Options);
            }

            var dbName = _tokenParser.GetMetadata().ClientId;

            var optionsBuilder = new DbContextOptionsBuilder<PlaceManagementDbContext>()
                .UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database={dbName};Trusted_Connection=True;MultipleActiveResultSets=true");

            return new PlaceManagementDbContext(optionsBuilder.Options);
        }

        public void OverrideClientId(int clientId)
        {
            _overrideClientId = clientId;
        }
    }
}
