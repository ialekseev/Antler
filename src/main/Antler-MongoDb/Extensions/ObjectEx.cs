using System;
using MongoDB.Bson;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Common.Extensions;

namespace SmartElk.Antler.MongoDb.Extensions
{
    public static class ObjectEx
    {
        public static BsonValue AsIdValue(this object id)
        {
            Requires.NotNull(id, "id");

            if (id is Guid) return (Guid)id;
            if (id is string) return (string)id;
            if (id is int) return (int)id;
            if (id is long) return (long)id;
            if (id is ObjectId) return (ObjectId)id;

            throw new InvalidOperationException(string.Format("Unsupported _id type : {0}", id.GetType().Name));
        }

        public static object ExtractId(this object entity, string idPropertyName)
        {
            Requires.NotNull(entity, "entity");
            Requires.NotNullOrEmpty(idPropertyName, "idPropertyName");
            
            return entity.GetPropertyValue(idPropertyName);
        }
    }
}
