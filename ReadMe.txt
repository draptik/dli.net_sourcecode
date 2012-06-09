Most of the solutions in this folder contains unit tests written with xUnit.net.

REQUIRED COMPONENTS

All solutions are built using Visual Studio 2010 on .NET 4.

All web applications (except CampaignManagement.sln) also require ASP.NET MVC 3.

The unit tests use open source libraries that are needed to be able to compile and run the tests. They are all included as NuGet packages, but if you are interested they can be found at the following addresses:
xUnit.net: http://xunit.codeplex.com/
AutoFixture: http://autofixture.codeplex.com/
Moq: http://code.google.com/p/moq

Some of the solutions themselves use various open source DI Containers to compile and run:
Castle Windsor: http://castleproject.org/
StructureMap: http://structuremap.github.com/structuremap/
Spring.NET: http://www.springframework.net/
Autofac: http://code.google.com/p/autofac/
Unity: http://unity.codeplex.com/

DATABASE SETUP

The applications also require that their databases are set up correctly. Each application requiring a database have associated T-SQL scripts, although SimpleCommerce shares the script with MarysECommerce.

In the case of ComplexCommerce, you must add the database tables to a database that has been prepared in advance for ASP.NET services (you can, e.g. use aspnet_regsql.exe). The file DBSchema.sql contains further instructions.

IIS SETUP

The CommerceService solutions must be hosted in IIS for the WpfProductManagementClient solution to work.
- Add an application to Default Web Site and call it "CommerceService". Point it to the root of the CommerceService folder.
- Set up permissions to the folder so that the proper hosting process can read the files.
- Grant the IIS user permissions to the database used by CommerceService. Graning the database roles db_datareader and db_datawriter works fine.


(last updated 2011.04.11 while finalizing the manuscript)