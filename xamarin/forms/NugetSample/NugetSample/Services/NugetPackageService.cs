using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NugetSample.Services
{
    public class NuGetPackageService : INuGetPackageService
    {
        private readonly SourceRepository _sourceRepository;

        public NuGetPackageService()
        {
            _sourceRepository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
        }

        public async Task<IEnumerable<IPackageSearchMetadata>> SearchNuGetPackages(string term, CancellationToken token)
        {
            var filter = new SearchFilter(false);
            
            // TODO: [rlittlesii: July 12, 2020] Make this a single instance in the DI registration.
            var resource = await _sourceRepository.GetResourceAsync<PackageSearchResource>(token).ConfigureAwait(false);

            return await resource.SearchAsync(term, filter, 0, 10, NullLogger.Instance, token).ConfigureAwait(false);
        }

        public async Task<IEnumerable<VersionInfo>> GetVersions(IPackageSearchMetadata packageSearchMetadata)
        {
            // TODO: [rlittlesii: July 12, 2020] Make this a single instance in the DI registration.
            var search = await _sourceRepository.GetResourceAsync<RawSearchResourceV3>();

            // TODO: [rlittlesii: July 12, 2020] Change over to the new fix for getting version information
            // https://github.com/NuGet/NuGet.Client/pull/3433
            var results = await search.Search(
                packageSearchMetadata.Title,
                new SearchFilter(false),
                0,
                1,
                NullLogger.Instance,
                CancellationToken.None);

            var list = new List<VersionInfo>();
            foreach (var result in results)
            {
                foreach (var version in result["versions"])
                {
                    Console.WriteLine($"{version["version"]} {version["downloads"]}");
                    list.Add(new VersionInfo(NuGetVersion.Parse(version["version"].ToString()), version["downloads"].ToString()));
                }
            }

            return list;
        }
    }
}