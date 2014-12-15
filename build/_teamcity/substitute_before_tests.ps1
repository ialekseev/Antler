$configDirectory = Split-Path $script:MyInvocation.MyCommand.Path

function substituteConnectionString($appConfigPath){
  $appConfigPathFull = ($configDirectory + $appConfigPath)
  [xml]$appConfig = Get-Content $appConfigPathFull   
  $appConfig.SelectSingleNode("//configuration/appSettings/add[@key='ConnectionString']").value = "Data Source=.\\SQLEXPRESS;Initial Catalog=AntlerTest;Integrated Security=True"
  $appConfig.Save($appConfigPathFull)
}

substituteConnectionString "\..\..\src\specs\Antler-NHibernate-SqlServer-Specs\bin\Release\Antler.NHibernate.SqlServer.Specs.dll.config"
substituteConnectionString "\..\..\src\specs\Antler-EntityFramework-SqlServer-Specs\bin\Release\Antler.EntityFramework.SqlServer.Specs.dll.config"
substituteConnectionString "\..\..\src\specs\Antler-Linq2Db-SqlServer-Specs\bin\Release\Antler.Linq2Db.SqlServer.Specs.dll.config"