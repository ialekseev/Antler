Antler ![alt text](http://www.gravatar.com/avatar/99c436bbd301be46a6e6daabc0dc1aa4.png "SmartElk/Antler")
======

Pluggable framework to work with different databases(SqlServer, Oracle, SQL CE, Sqlite etc.) and different ORMs(NHibernate, EntityFramework Code First) using the <b>same syntax</b>.

+ Supporting multiple storages at the same time.
+ Using common syntax to work with different storages, so we can easily substitute one storage with another.
+ Having strong architectural base including UnitOfWork/DataSession/Repository etc. notions.
+ Fully pluggable. For example, it's damn easy to choose which storage or IoC container to use.


Usage examples
--------------

Inserting Teams in database:
<pre>
UnitOfWork.Do(uow =>
                    {
                        uow.Repo<Team>().Insert(new Team() {Name = "Penguins", Description = "Hockey"});
                        uow.Repo<Team>().Insert(new Team() {Name = "Capitals", Description = "Hockey"});
                        uow.Repo<Team>().Insert(new Team() {Name = "Nets", Description = "Basketball"});
                    });
</pre>

Querying Teams from database:
<pre>
var found = UnitOfWork.Do(uow => uow.Repo<Team>().AsQueryable().Where(t => t.Description == "Hockey").OrderBy(t => t.Name).ToArray()); 
</pre>

Configuration examples
-----------------------
For example, let's configure application to use (NHibernate + Sqlite database) and Castle Windsor container:
<pre>
var configurator = new AntlerConfigurator();
configurator.UseWindsorContainer().UseStorage(NHibernateStorage.Use.WithDatabaseConfiguration(SQLiteConfiguration.Standard.InMemory()).WithMappings(assemblyWithMappings));
</pre>

Let's configure application to use (EntityFramework Code First + SqlServer) and Castle Windsor container:
<pre>
var configurator = new AntlerConfigurator();
configurator.UseWindsorContainer().UseStorage(EntityFrameworkStorage.Use.WithConnectionString(connectionString)
                                                                      .WithMappings(assemblyWithMappings));
</pre>


Installing NuGet packages
-------------------------

Core library: https://www.nuget.org/packages/Antler.Core/

### Adapters for IoC Containers

Windsor adapter: https://www.nuget.org/packages/Antler.Windsor/

StructureMap adapter: https://www.nuget.org/packages/Antler.StructureMap/

### Adapters for ORMs 

NHibernate adapter: https://www.nuget.org/packages/Antler.NHibernate/

EntityFramework adapter: https://www.nuget.org/packages/Antler.EntityFramework/


<hr>
Largely based on the great NoSQL framework https://github.com/Kostassoid/Anodyne.

