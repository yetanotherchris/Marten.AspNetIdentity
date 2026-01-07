using System;
using System.Collections.Concurrent;

namespace Marten.AspNetIdentity.Tests.Integration
{
	public class DocumentStoreManager
	{
		private static readonly ConcurrentDictionary<string, IDocumentStore> _documentStores = new ConcurrentDictionary<string, IDocumentStore>();
		public static string ConnectionString => $"host=localhost;port=5432;database={DbName};username=aspnetidentity;password=aspnetidentity;";

		private static string DbName =>
		#if NET10_0
			"aspnetidentity_net10";
		#elif NET9_0
			"aspnetidentity_net9";
		#elif NET8_0
			"aspnetidentity_net8";
		#else
			"aspnetidentity";
		#endif

		public static IDocumentStore GetMartenDocumentStore(Type testClassType, string connectionString = null)
		{
			string documentStoreSchemaName = "";

			if (testClassType != null)
				documentStoreSchemaName = testClassType.Name;

			if (!_documentStores.ContainsKey(documentStoreSchemaName))
			{
				IDocumentStore docStore = Extensions.CreateDocumentStore(connectionString ?? ConnectionString, documentStoreSchemaName);
				_documentStores.TryAdd(documentStoreSchemaName, docStore);
			}

			return _documentStores[documentStoreSchemaName];
		}
	}
}