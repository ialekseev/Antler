Antler ![alt text](http://www.gravatar.com/avatar/99c436bbd301be46a6e6daabc0dc1aa4.png "SmartElk/Antler")
======

Pluggable framework for .NET to work with popular ORMs(NHibernate, EntityFramework, Linq2Db) using the <b>same syntax</b>. Antler framework is an abstraction over ORM that you like to use. 

Goals
--------------
+ To use common syntax to work with different ORMs, so we could easily substitute one ORM with another.
+ To have very easy "fluent" configuration.
+ To be fully pluggable. For example, it should be damn easy to choose which ORM, database or IoC container to use.
+ Applications that use framework should be easily testable(unit & integration tests).


Usage examples
--------------

Inserting Teams in database:
<pre>
UnitOfWork.Do(uow =>
                    {
                        uow.Repo&lt;Team&gt;().Insert(new Team() {Name = "Penguins", Description = "Hockey"});
                        uow.Repo&lt;Team&gt;().Insert(new Team() {Name = "Capitals", Description = "Hockey"});
                        uow.Repo&lt;Team&gt;().Insert(new Team() {Name = "Nets", Description = "Basketball"});
                    });
</pre>

Querying Teams from database:
<pre>
var found = UnitOfWork.Do(uow => uow.Repo&lt;Team&gt;().AsQueryable().Where(t => t.Description == "Hockey").
                                                                OrderBy(t => t.Name).ToArray()); 
</pre>

Configuration examples
-----------------------
For example, let's configure application to use (NHibernate + Sqlite database) and Castle Windsor container:
<pre>
var configurator = new AntlerConfigurator();
configurator.UseWindsorContainer().UseStorage(NHibernateStorage.Use.
                                              WithDatabaseConfiguration(SQLiteConfiguration.Standard.InMemory()).
                                              WithMappings(assemblyWithMappings));
</pre>

Let's configure application to use (EntityFramework Code First + SqlServer) and StructureMap container:
<pre>
var configurator = new AntlerConfigurator();
configurator.UseStructureMapContainer().UseStorage(EntityFrameworkStorage.Use.
                                              WithConnectionString(connectionString).
                                              WithMappings(assemblyWithMappings));
</pre>
More examples in wiki:
https://github.com/SmartElk/Antler/wiki/Configuration-examples

Installing NuGet packages
-------------------------

Core library: https://www.nuget.org/packages/Antler.Core/

### Adapters for IoC Containers

Windsor adapter: https://www.nuget.org/packages/Antler.Windsor/

StructureMap adapter: https://www.nuget.org/packages/Antler.StructureMap/

### Adapters for ORMs 

NHibernate adapter: https://www.nuget.org/packages/Antler.NHibernate/

EntityFramework Code First adapter: https://www.nuget.org/packages/Antler.EntityFramework/

Linq2Db adapter: https://www.nuget.org/packages/Antler.Linq2Db/

Build server(TeamCity)
-------------------------
http://smartelk.com:8111/guestLogin.html?guest=1

<hr>
Largely based on the great NoSQL framework https://github.com/Kostassoid/Anodyne.

