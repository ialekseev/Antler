Antler
======

Pluggable framework to work with different databases(SqlServer, Oracle, SQL CE, Sqlite etc.) and different ORMs(NHibernate, EntityFramework Code First) using the <b>same syntax</b>.

+ Supporting multiple storages at the same time.
+ Using common syntax to work with storages, so we can easily substitute one storage with another.
+ Have strong architectural base including UnitOfWork/DataSession/Repository etc. notions.
+ Be fully pluggable. For example, it should be damn easy to choose which storage or IoC container to use.
