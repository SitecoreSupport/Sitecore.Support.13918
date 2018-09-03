using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Azure;
using Sitecore.ContentSearch.Azure.Query;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.ContentSearch.Security;

namespace Sitecore.Support.XA.Foundation.Search.Providers.Azure
{
  public class CloudSearchProviderIndex : Sitecore.Support.ContentSearch.Azure.CloudSearchProviderIndex
  {
    public CloudSearchProviderIndex(string name, string connectionStringName, string totalParallelServices, IIndexPropertyStore propertyStore) : base(name, connectionStringName, totalParallelServices, propertyStore)
    {
    }

    public override IProviderSearchContext CreateSearchContext(SearchSecurityOptions options = SearchSecurityOptions.EnableSecurityCheck)
    {
      base.CreateSearchContext(options);
      return new Sitecore.Support.ContentSearch.Azure.CloudSearchSearchContext(InitializeServiceCollectionClient(), options);
    }

    protected virtual ServiceCollectionClient InitializeServiceCollectionClient()
    {
      ServiceCollectionClient client = new ServiceCollectionClient();
      client.Register(typeof(LinqToCloudIndex<>), builder => builder.CreateInstance(typeof(LinqToCloudIndex<>), builder.TypeParameters, builder.Args));
      client.Register(typeof(AbstractSearchIndex), builder => this);
      client.Register(typeof(QueryStringBuilder), builder => new QueryStringBuilder(new FilterQueryBuilder(), new SearchQueryBuilder(), true));
      return client;
    }
  }
}