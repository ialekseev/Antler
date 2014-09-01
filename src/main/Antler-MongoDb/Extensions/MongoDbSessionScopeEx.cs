using System;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;

namespace SmartElk.Antler.MongoDb.Extensions
{
    public static class MongoDbSessionScopeEx
    {
        public static void MarkAsNew(this ISessionScope sessionScope, object entity)
        {
            Requires.NotNull(entity, "entity");
            Mark(sessionScope, scope => scope.MarkAsNew(entity));
        }

        public static void MarkAsUpdated(this ISessionScope sessionScope, object entity)
        {
            Requires.NotNull(entity, "entity");
            Mark(sessionScope, scope => scope.MarkAsUpdated(entity));
        }

        public static void MarkAsDeleted(this ISessionScope sessionScope, object entity)
        {
            Requires.NotNull(entity, "entity");
            Mark(sessionScope, scope => scope.MarkAsDeleted(entity));
        }


        private static void Mark(ISessionScope sessionScope, Action<MongoDbSessionScope> mark)
        {
            var mongoDbSessionScope = sessionScope as MongoDbSessionScope;
            Assumes.True(mongoDbSessionScope != null, "MongoDbSessionScope was expected!");
            mark(mongoDbSessionScope);                        
        }
    }
}
