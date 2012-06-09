using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.ComponentModel.Composition.Hosting;
using Ploeh.Samples.MenuModel;
using System.ComponentModel.Composition;
using Ploeh.Samples.Menu.Mef.ExportProvided;

namespace Ploeh.Samples.Menu.Mef
{
    public class MenuFacts
    {
        [Fact]
        public void EmptyContainerDoesNotResolveSauceBéarnaise()
        {
            var container = new CompositionContainer();
            Assert.Throws<ImportCardinalityMismatchException>(() =>
                container.GetExportedValue<SauceBéarnaise>());
        }

        [Fact]
        public void ContainerDoesNotResolveUnattributedType()
        {
            var container = new CompositionContainer(new TypeCatalog(typeof(SauceBéarnaise)));
            Assert.Throws<ImportCardinalityMismatchException>(() =>
                container.GetExportedValue<SauceBéarnaise>());
        }

        [Fact]
        public void ContainerResolvesSauceBéarnaise()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Concrete.SauceBéarnaise));
            var container = new CompositionContainer(catalog);
            SauceBéarnaise sauce =
                container.GetExportedValue<SauceBéarnaise>();

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerCanGetExportForSauceBéarnaise()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Concrete.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            Lazy<SauceBéarnaise> export =
                container.GetExport<SauceBéarnaise>();
            SauceBéarnaise sauce = export.Value;

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerDoesNotResolveIngredientToSauceBéarnaise()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Concrete.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            Assert.Throws<ImportCardinalityMismatchException>(() =>
                container.GetExportedValue<Ploeh.Samples.MenuModel.IIngredient>());
        }

        [Fact]
        public void ContainerResolvesIngredientToSauceBéarnaise()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            IIngredient ingredient = container.GetExportedValue<IIngredient>();

            Assert.IsAssignableFrom<SauceBéarnaise>(ingredient);
        }

        [Fact]
        public void ContainerResolvesBothAbstractionAndConcreteType()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            var sauce = container.GetExportedValue<SauceBéarnaise>();
            var ingredient = container.GetExportedValue<IIngredient>();

            Assert.NotNull(sauce);
            Assert.NotNull(ingredient);
        }

        [Fact]
        public void ResolveOliveOilFromDerivedClass()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Concrete.MefOliveOil));
            var container = new CompositionContainer(catalog);

            var oil = container.GetExportedValue<OliveOil>();

            Assert.NotNull(oil);
        }

        [Fact]
        public void ResolveIngredientFromDerivedOliveOil()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Concrete.MefOliveOil));
            var container = new CompositionContainer(catalog);

            var ingredient = container.GetExportedValue<IIngredient>();

            Assert.NotNull(ingredient);
        }

        [Fact]
        public void ResolveSharedOliveOil()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.Shared.OliveOilAdapter));
            var container = new CompositionContainer(catalog);

            var oil1 = container.GetExportedValue<OliveOil>();
            var oil2 = container.GetExportedValue<OliveOil>();

            Assert.Same(oil1, oil2);
        }

        [Fact]
        public void ResolveNonSharedOliveOil()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.NonShared.OliveOilAdapter));
            var container = new CompositionContainer(catalog);

            var oil1 = container.GetExportedValue<OliveOil>();
            var oil2 = container.GetExportedValue<OliveOil>();

            Assert.NotSame(oil1, oil2);
        }

        [Fact]
        public void ResolveMayonnaiseThroughAdapter()
        {
            var catalog = new TypeCatalog(
                typeof(Ploeh.Samples.Menu.Mef.Adapted.MayonnaiseAdapter),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.EggYolkAdapter),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.OliveOilAdapter));
            var container = new CompositionContainer(catalog);

            var m = container.GetExportedValue<Mayonnaise>();

            Assert.NotNull(m);
        }

        [Fact]
        public void ResolveSharedMayonnaise()
        {
            var catalog = new TypeCatalog(
                typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.Shared.MayonnaiseAdapter),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.Shared.EggYolkAdapter),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.Shared.OliveOilAdapter));
            var container = new CompositionContainer(catalog);

            var mayo1 = container.GetExportedValue<Mayonnaise>();
            var mayo2 = container.GetExportedValue<Mayonnaise>();

            Assert.Same(mayo1, mayo2);
        }

        [Fact]
        public void ResolveNonSharedMayonnaise()
        {
            var catalog = new TypeCatalog(
                typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.NonShared.MayonnaiseAdapter),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.NonShared.EggYolkAdapter),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.NonShared.OliveOilAdapter));
            var container = new CompositionContainer(catalog);

            var mayo1 = container.GetExportedValue<Mayonnaise>();
            var mayo2 = container.GetExportedValue<Mayonnaise>();

            Assert.NotSame(mayo1, mayo2);
            Assert.NotSame(mayo1.Oil, mayo2.Oil);
        }

        [Fact]
        public void ResolveNonSharedMayonnaiseWithSharedOil()
        {
            var catalog = new TypeCatalog(
                typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.NonShared.MayonnaiseAdapter),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.NonShared.EggYolkAdapter),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.Shared.OliveOilAdapter));
            var container = new CompositionContainer(catalog);

            var mayo1 = container.GetExportedValue<Mayonnaise>();
            var mayo2 = container.GetExportedValue<Mayonnaise>();

            Assert.Same(mayo1.Oil, mayo2.Oil);
        }

        [Fact]
        public void ResolveMayonnaiseThatRequiresFreshEggYolkWithDefaultEggYolk()
        {
            var catalog = new TypeCatalog(
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonSharedImport.Mayonnaise),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.EggYolkAdapter),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.OliveOilAdapter));
            var container = new CompositionContainer(catalog);

            var mayo1 = container.GetExportedValue<Mayonnaise>();
            var mayo2 = container.GetExportedValue<Mayonnaise>();

            Assert.NotSame(mayo1.EggYolk, mayo2.EggYolk);
        }

        [Fact]
        public void ResolveMayonnaiseThatRequiresFreshEggYolkWithNonSharedEggYolk()
        {
            var catalog = new TypeCatalog(
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonSharedImport.Mayonnaise),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.NonShared.EggYolkAdapter),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.OliveOilAdapter));
            var container = new CompositionContainer(catalog);

            var mayo1 = container.GetExportedValue<Mayonnaise>();
            var mayo2 = container.GetExportedValue<Mayonnaise>();

            Assert.NotSame(mayo1.EggYolk, mayo2.EggYolk);
        }

        [Fact]
        public void ResolveMayonnaiseThatRequiresFreshEggYolkWithSharedEggYolkThrows()
        {
            var catalog = new TypeCatalog(
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonSharedImport.Mayonnaise),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.Lifetime.Shared.EggYolkAdapter),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.OliveOilAdapter));
            var container = new CompositionContainer(catalog);

            Assert.Throws<ImportCardinalityMismatchException>(() =>
                container.GetExportedValue<Mayonnaise>());
        }

        [Fact]
        public void ResolveOliveOilThroughGenericAdapter()
        {
            var catalog = new TypeCatalog(typeof(MefAdapter<OliveOil>));
            var container = new CompositionContainer(catalog);

            var oil = container.GetExportedValue<OliveOil>();

            Assert.NotNull(oil);
        }

        [Fact]
        public void ResolveMayonnaiseThroughGenericAdapters()
        {
            var catalog = new TypeCatalog(
                typeof(MefAdapter<OliveOil>),
                typeof(MefAdapter<EggYolk>),
                typeof(MefAdapter<Mayonnaise, EggYolk, OliveOil>));
            var container = new CompositionContainer(catalog);

            var mayo = container.GetExportedValue<Mayonnaise>();

            Assert.NotNull(mayo);
        }

        [Fact]
        public void ResolveThrowsOnInvalidExport()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Invalid.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            Assert.Throws<CompositionContractMismatchException>(() =>
                container.GetExportedValue<ICourse>());
        }

        [Fact]
        public void ExistenceOfIngredientDoesNotInterfereWithCourse()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Course),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            var course = container.GetExportedValue<ICourse>();
            Assert.NotNull(course);
        }

        [Fact]
        public void ExportMultipleImplementationsOfTheSameType()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Steak));
            var container = new CompositionContainer(catalog);

            IEnumerable<IIngredient> ingredients =
                container.GetExportedValues<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void GetExportedValueThrowsOnMultipleExports()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Steak));
            var container = new CompositionContainer(catalog);

            Assert.Throws<ImportCardinalityMismatchException>(() =>
                container.GetExportedValue<IIngredient>());
        }

        [Fact]
        public void GetExportThrowsOnMultipleExports()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Steak));
            var container = new CompositionContainer(catalog);

            Assert.Throws<ImportCardinalityMismatchException>(() =>
                container.GetExport<IIngredient>());
        }

        [Fact]
        public void GetExportedValuesCorrectlyResolveSingleExport()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            var ingredients = container.GetExportedValues<IIngredient>();

            Assert.IsAssignableFrom<SauceBéarnaise>(ingredients.Single());
        }

        [Fact]
        public void ExportMultipleImplementationsOfTheSameTypeButResolveThemLazily()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Steak));
            var container = new CompositionContainer(catalog);

            IEnumerable<Lazy<IIngredient>> exports =
                container.GetExports<IIngredient>();

            var ingredients = from x in exports
                              select x.Value;

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterIngredientsIgnoresUnresolvableTypes()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.CordonBleu),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.ChiliConCarne));
            var container = new CompositionContainer(catalog);

            var ingredients = container.GetExportedValues<ICourse>();

            Assert.True(ingredients.OfType<CordonBleu>().Any());
            Assert.False(ingredients.OfType<ChiliConCarne>().Any());
        }

        [Fact]
        public void ResolveAllIngredients()
        {
            var representative = typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise);
            var ingredientTypes = (from t in representative.Assembly.GetExportedTypes()
                                   where t.Namespace == representative.Namespace
                                   && typeof(IIngredient).IsAssignableFrom(t)
                                   select t).ToList();
            var catalog = new TypeCatalog(ingredientTypes);
            var container = new CompositionContainer(catalog);

            var ingredients = container.GetExportedValues<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void ResolveAllIngredientsFromAssembly()
        {
            var assembly = typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Steak).Assembly;
            var catalog = new AssemblyCatalog(assembly);
            var container = new CompositionContainer(catalog);

            var ingredients = container.GetExportedValues<IIngredient>();

            Assert.NotEmpty(ingredients);
        }

        [Fact]
        public void ResolveAllIngredientsFromFolder()
        {
            var directory = Environment.CurrentDirectory;
            var catalog = new DirectoryCatalog(directory);
            var container = new CompositionContainer(catalog);

            var ingredients = container.GetExportedValues<IIngredient>();

            Assert.NotEmpty(ingredients);
        }

        [Fact]
        public void ResolveIngredientsFromAggregateCatalog()
        {
            var catalog1 = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise));
            var catalog2 = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Steak));
            var catalog = new AggregateCatalog(catalog1, catalog2);
            var container = new CompositionContainer(catalog);

            var ingredients = container.GetExportedValues<IIngredient>();

            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
            Assert.True(ingredients.OfType<Steak>().Any());
        }

        [Fact]
        public void ResolveSaucesByFilteredCatalog()
        {
            var representative = typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise);
            var assemblyCatalog = new AssemblyCatalog(representative.Assembly);
            var catalog = new FilteredCatalog(assemblyCatalog, def => def.ExportDefinitions.Any(x => x.ContractName.Contains("Sauce")));
            var container = new CompositionContainer(catalog);

            var sauces = container.GetExportedValues<IIngredient>();

            Assert.True(sauces.OfType<SauceBéarnaise>().Any());
            Assert.True(sauces.OfType<SauceHollandaise>().Any());
            Assert.True(sauces.OfType<SauceMousseline>().Any());

            Assert.False(sauces.OfType<Steak>().Any());
        }

        [Fact]
        public void ResolveSaucesBySauceCatalog()
        {
            var representative = typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise);
            var ingredientCatalog = new AssemblyCatalog(representative.Assembly);
            var catalog = new SauceCatalog(ingredientCatalog);
            var container = new CompositionContainer(catalog);

            var sauces = container.GetExportedValues<IIngredient>();

            Assert.True(sauces.OfType<SauceBéarnaise>().Any());
            Assert.True(sauces.OfType<SauceHollandaise>().Any());
            Assert.True(sauces.OfType<SauceMousseline>().Any());

            Assert.False(sauces.OfType<Steak>().Any());
        }

        [Fact]
        public void ResolveAllSaucesByName()
        {
            var representative = typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.SauceBéarnaise);
            var ingredientTypes = (from t in representative.Assembly.GetExportedTypes()
                                   where t.Namespace == representative.Namespace
                                   select t).ToList();
            var catalog = new TypeCatalog(ingredientTypes);
            var container = new CompositionContainer(catalog);

            var sauces = container.GetExportedValues<IIngredient>("sauce");

            Assert.True(sauces.OfType<SauceBéarnaise>().Any());
            Assert.True(sauces.OfType<SauceHollandaise>().Any());
            Assert.True(sauces.OfType<SauceMousseline>().Any());

            Assert.False(sauces.OfType<Steak>().Any());
        }

        [Fact]
        public void ResolveAllSaucesByMetadata()
        {
            var representative = typeof(Ploeh.Samples.Menu.Mef.Attributed.Metadata.Untyped.SauceBéarnaise);
            var ingredientTypes = (from t in representative.Assembly.GetExportedTypes()
                                   where t.Namespace == representative.Namespace
                                   && typeof(IIngredient).IsAssignableFrom(t)
                                   select t).ToList();
            var catalog = new TypeCatalog(ingredientTypes);
            var container = new CompositionContainer(catalog);

            var sauces = container.GetExports<IIngredient, IDictionary<string, object>>()
                .Where(x => x.Metadata.ContainsKey("category") && x.Metadata["category"].Equals("sauce"))
                .Select(x => x.Value)
                .ToList();

            Assert.True(sauces.OfType<SauceBéarnaise>().Any());
            Assert.True(sauces.OfType<SauceHollandaise>().Any());
            Assert.True(sauces.OfType<SauceMousseline>().Any());

            Assert.False(sauces.OfType<Steak>().Any());
        }

        [Fact]
        public void ResolveOliveOilFromExportProvider()
        {
            var provider = new OliveOilExportProvider();
            var container = new CompositionContainer(provider);

            var oil = container.GetExportedValue<OliveOil>();

            Assert.NotNull(oil);
        }

        [Fact]
        public void RegisterSauceBéarnaiseAsTransient()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonShared.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            var sauce1 = container.GetExportedValue<SauceBéarnaise>();
            var sauce2 = container.GetExportedValue<SauceBéarnaise>();

            Assert.NotSame(sauce1, sauce2);
        }

        [Fact]
        public void ExplicitlyRegisterSauceBéarnaiseAsSingleton()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Lifetime.Shared.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            var sauce1 = container.GetExportedValue<SauceBéarnaise>();
            var sauce2 = container.GetExportedValue<SauceBéarnaise>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void ImplicitlyRegisterSauceBéarnaiseAsSingleton()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Concrete.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            var sauce1 = container.GetExportedValue<SauceBéarnaise>();
            var sauce2 = container.GetExportedValue<SauceBéarnaise>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void ReleaseParsleyAsSingletonDoesNotDisposeParsley()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Lifetime.Shared.Parsley));
            var container = new CompositionContainer(catalog);

            var x = container.GetExport<IIngredient>();
            var ingredient = x.Value;

            container.ReleaseExport(x);

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.False(parsley.IsDisposed);
        }

        [Fact]
        public void ReleaseParsleyAsTransientDisposesParsley()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonShared.Parsley));
            var container = new CompositionContainer(catalog);

            var x = container.GetExport<IIngredient>();
            var ingredient = x.Value;

            container.ReleaseExport(x);

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.True(parsley.IsDisposed);
        }

        [Fact]
        public void DisposeContainerDisposesSingletonParsley()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Lifetime.Shared.Parsley));
            var container = new CompositionContainer(catalog);

            var ingredient = container.GetExportedValue<IIngredient>();

            container.Dispose();

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.True(parsley.IsDisposed);
        }

        [Fact]
        public void DisposeContainerDisposesTransientParsley()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonShared.Parsley));
            var container = new CompositionContainer(catalog);

            var ingredient = container.GetExportedValue<IIngredient>();

            container.Dispose();

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.True(parsley.IsDisposed);
        }

        [Fact]
        public void ResolveServicesWithSameSingletonDependency()
        {
            var representative = typeof(Ploeh.Samples.Menu.Mef.Attributed.Lifetime.Shared.Course);
            var types = (from t in representative.Assembly.GetExportedTypes()
                         where t.Namespace == representative.Namespace
                         select t).ToList();
            var catalog = new TypeCatalog(types);
            var container = new CompositionContainer(catalog);

            var c = container.GetExportedValue<Course>();

            Assert.Same(c.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c.Ingredients.OfType<Mayonnaise>().Single().Oil);
        }

        [Fact]
        public void ResolveServicesWithSameTransientDependency()
        {
            var representative = typeof(Ploeh.Samples.Menu.Mef.Attributed.Lifetime.NonShared.Course);
            var types = (from t in representative.Assembly.GetExportedTypes()
                         where t.Namespace == representative.Namespace
                         select t).ToList();
            var catalog = new TypeCatalog(types);
            var container = new CompositionContainer(catalog);

            var c = container.GetExportedValue<Course>();

            Assert.NotSame(c.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c.Ingredients.OfType<Mayonnaise>().Single().Oil);
        }

        [Fact]
        public void EmptyContainerResolvesAllToEmptyArray()
        {
            var catalog = new AggregateCatalog();
            var container = new CompositionContainer(catalog);

            var ingredients = container.GetExportedValues<IIngredient>();

            Assert.Empty(ingredients);
        }

        [Fact]
        public void RegisterMoreThanOneIngredientAndAttemtToResolveOnlyOne()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.SauceBéarnaise),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Steak));
            var container = new CompositionContainer(catalog);

            Assert.Throws<ImportCardinalityMismatchException>(() =>
            {
                var ingredient = container.GetExportedValue<IIngredient>();
            });
        }

        [Fact]
        public void RegisterNamedIngredients()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.SauceBéarnaise),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.Steak));
            var container = new CompositionContainer(catalog);

            var meat = container.GetExportedValue<IIngredient>("meat");
            var sauce = container.GetExportedValue<IIngredient>("sauce");

            Assert.IsAssignableFrom<Steak>(meat);
            Assert.IsAssignableFrom<SauceBéarnaise>(sauce);
        }

        [Fact]
        public void ContainerResolvesDefaultComponentEvenInThePresenceOfNamedComponent()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Steak),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            var ingredient = container.GetExportedValue<IIngredient>();
            Assert.IsAssignableFrom<Steak>(ingredient);
        }

        [Fact]
        public void ContainerResolvesNamedComponentEvenInThePresenceOfDefaultComponent()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Steak),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            var ingredient = container.GetExportedValue<IIngredient>("sauce");
            Assert.IsAssignableFrom<SauceBéarnaise>(ingredient);
        }

        [Fact]
        public void ContainerDoesNotResolveUnnamedComponentWhenOnlyNamedExists()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.SauceBéarnaise));
            var container = new CompositionContainer(catalog);

            Assert.Throws<ImportCardinalityMismatchException>(() =>
            {
                var ingredient = container.GetExportedValue<IIngredient>();
            });
        }

        [Fact]
        public void CreateThreeCourseMeal()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.ThreeCourseMeal),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.Rillettes),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.CordonBleu),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.MousseAuChocolat));
            var container = new CompositionContainer(catalog);

            var meal = container.GetExportedValue<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<ThreeCourseMeal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Entrée);
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.MainCourse);
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Dessert);
        }

        [Fact]
        public void CreateMultiCourseMealByExplicitlyPickingCourses()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Sequence.ExplicitPick.Meal),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Sequence.ExplicitPick.Rillettes),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Sequence.ExplicitPick.LobsterBisque),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Sequence.ExplicitPick.CordonBleu),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Sequence.ExplicitPick.MousseAuChocolat));
            var container = new CompositionContainer(catalog);

            var meal = container.GetExportedValue<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void CreateMultiCourseMealFromAllCourses()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Meal),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Rillettes),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.CordonBleu),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.MousseAuChocolat));
            var container = new CompositionContainer(catalog);

            var meal = container.GetExportedValue<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void ResolveCotolettaUsingConcreteContract()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Concrete.Breading),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Concrete.VealCutlet));
            var container = new CompositionContainer(catalog);

            var cotoletta = container.GetExportedValue<IIngredient>();

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void ResolveCotolettaUsingNamedContract()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.Breading),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Named.VealCutlet));
            var container = new CompositionContainer(catalog);

            var cotoletta = container.GetExportedValue<IIngredient>();

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void ResolveChiliConCarne()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.ChiliConCarne),
                typeof(Ploeh.Samples.Menu.Mef.Adapted.SpicynessAdapter));
            var container = new CompositionContainer(catalog);

            var course = container.GetExportedValue<ICourse>();

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Hot, chili.Spicyness);
        }

        [Fact]
        public void ResolveJunkFood()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Adapted.JunkFoodAdapter));
            var container = new CompositionContainer(catalog);

            var meal = container.GetExportedValue<IMeal>();

            var junk = Assert.IsAssignableFrom<JunkFood>(meal);
            Assert.Equal("chicken meal", junk.Name);
        }

        [Fact]
        public void ResolveBasicCaesarSalad()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.CaesarSalad));
            var container = new CompositionContainer(catalog);

            var course = container.GetExportedValue<ICourse>();

            Assert.IsAssignableFrom<CaesarSalad>(course);
        }

        [Fact]
        public void ResolveBasicCaesarSaladHasCorrectExtra()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.CaesarSalad));
            var container = new CompositionContainer(catalog);

            var course = container.GetExportedValue<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(course);
            Assert.IsAssignableFrom<NullIngredient>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWithExtraChicken()
        {
            var catalog = new TypeCatalog(typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.CaesarSalad),
                typeof(Ploeh.Samples.Menu.Mef.Attributed.Unmodified.Abstract.Chicken));
            var container = new CompositionContainer(catalog);

            var course = container.GetExportedValue<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(course);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }
    }
}
