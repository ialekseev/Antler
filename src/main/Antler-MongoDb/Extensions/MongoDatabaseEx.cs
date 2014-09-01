using MongoDB.Driver;

namespace SmartElk.Antler.MongoDb.Extensions
{
    public static class MongoDatabaseEx
    {
        public static MongoCollection<TEntity> GetCollection<TEntity>(this MongoDatabase session)
        {
            return session.GetCollection<TEntity>(typeof (TEntity).Name);
        }

        public static MongoCollection GetCollection(this MongoDatabase session, object entity)
        {
            return session.GetCollection(entity.GetType().Name);
        }
    }
}
