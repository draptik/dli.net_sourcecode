svcutil ..\..\..\ComplexCommerce\CommerceService\bin\Ploeh.Samples.CommerceService.dll
svcutil ploehproductMgtSrvc.wsdl *.xsd /n:"urn:ploeh:productMgtSrvc,Ploeh.Samples.ProductManagement.WcfAgent.WcfClient" /noConfig /o:IProductManagementService.cs /tcv:Version35
del *.wsdl
del *.xsd