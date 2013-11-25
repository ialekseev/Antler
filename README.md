Antler
======

Pluggable framework to work with different databases(SQL CE, Sqlite, SqlExpress, SqlServer, Oracle etc.) and different ORMs(NHibernate, EntityFramework Code First) using the <b>same syntax</b>.

Usage examples
--------------

Inserting Teams in database:
<pre>
UnitOfWork.Do(uow =>
                    {
                        uow.Repo<Team>().Insert(new Team() {Name = "Super", BusinessGroup = "Great"});
                        uow.Repo<Team>().Insert(new Team() {Name = "Good", BusinessGroup = "Great"});
                        uow.Repo<Team>().Insert(new Team() {Name = "Bad", BusinessGroup = "BadBg"});
                    });
</pre>

Querying Teams from database:
<pre>
var found = UnitOfWork.Do(uow => uow.Repo<Team>().AsQueryable().Where(t => t.BusinessGroup == "Great").OrderBy(t => t.Name).ToArray()); 
</pre>

Configuration examples
-----------------------
For example, let's configure application to use (NHibernate + Sqlite database) and Castle Windsor container:
<pre>
var Configurator = new AntlerConfigurator();
Configurator.UseWindsorContainer().UseStorage(NHibernatePlusSqlite.Use);
</pre>

If we want to specify assembly with NHibernate mappings explicitly we can write:
<pre>
var Configurator = new AntlerConfigurator();
Configurator.UseWindsorContainer().UseStorage(NHibernatePlusSqlite.Use.WithMappings(Assembly.GetExecutingAssembly()));
</pre>

Let's configure application to use (EntityFramework Code First + Sql Compact database) and Castle Windsor container:
<pre>
var Configurator = new AntlerConfigurator();
Configurator.UseWindsorContainer().UseStorage(EntityFrameworkPlusSqlCe.Use.WithConnectionString("Data Source=DB.sdf")
                                                                      .WithMappings(Assembly.GetExecutingAssembly()));
</pre>


Largely based on the great NoSQL framework https://github.com/Kostassoid/Anodyne.

