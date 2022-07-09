using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MunchenClient.Config
{
	internal class CustomContractResolver : DefaultContractResolver
	{
		private readonly string propertyNameToExclude;

		internal CustomContractResolver(string propertyNameToExclude)
		{
			this.propertyNameToExclude = propertyNameToExclude;
		}

		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			IList<JsonProperty> source = base.CreateProperties(type, memberSerialization);
			return source.Where((JsonProperty p) => string.Compare(p.PropertyName, propertyNameToExclude, StringComparison.OrdinalIgnoreCase) != 0).ToList();
		}
	}
}
