using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace SmartElk.Antler.EntityFramework.Internal
{
    public class DataContext: DbContext, IDataContext
    {
        private readonly Assembly _assemblyWithMappings;

        public DataContext(string connectionString, Assembly assemblyWithMappings, bool enableLazyLoading): base(connectionString)
        {
            _assemblyWithMappings = assemblyWithMappings;
            this.Configuration.LazyLoadingEnabled = enableLazyLoading;
        }

        public DataContext(string connectionString, Assembly assemblyWithMappings)
            : this(connectionString, assemblyWithMappings, true)
        {            
        }
        
        public DataContext(Assembly assemblyWithMappings, bool enableLazyLoading)            
        {
            _assemblyWithMappings = assemblyWithMappings;
            this.Configuration.LazyLoadingEnabled = enableLazyLoading;
        }

        public DataContext(Assembly assemblyWithMappings) : this(assemblyWithMappings, true)
        {            
        }
        
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {                        
            RegisterMappings(modelBuilder);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }

        private void RegisterMappings(DbModelBuilder modelBuilder)
        {            
            var typesToRegister =
                _assemblyWithMappings.GetTypes().Where(
                    type =>
                    type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (object configurationInstance in typesToRegister.Select(Activator.CreateInstance))
            {
                modelBuilder.Configurations.Add((dynamic)configurationInstance);
            }
        }


        public void Clear()
        {
            this.Database.Delete();
            this.Database.Create();
        }
    }
}
