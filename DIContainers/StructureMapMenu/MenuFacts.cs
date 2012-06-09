using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using StructureMap;
using Ploeh.Samples.MenuModel;
using System.Xml;
using Ploeh.Samples.Menu.StructureMap.Properties;
using StructureMap.Pipeline;
using Xunit.Extensions;
using System.Threading;
using Ploeh.Samples.CacheModel;

namespace Ploeh.Samples.Menu.StructureMap
{
    public class MenuFacts : IDisposable
    {
        [Fact]
        public void ContainerResolvesSauceBéarnaise()
        {
            var container = new Container();
            SauceBéarnaise sauce = container.GetInstance<SauceBéarnaise>();

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerAutoWiresMayonnaise()
        {
            var container = new Container();
            var mayo = container.GetInstance<Mayonnaise>();

            Assert.NotNull(mayo);
        }

        [Fact]
        public void ContainerResolvesIngredientToSauceBéarnaise()
        {
            var container = new Container();
            container.Configure(r => r
                .For<IIngredient>()
                .Use<SauceBéarnaise>());
            IIngredient ingredient = container.GetInstance<IIngredient>();

            Assert.IsAssignableFrom<SauceBéarnaise>(ingredient);
        }

        [Fact]
        public void ContainerCanBeConfiguredThroughItsConstructor()
        {
            var container = new Container(r => r
                .For<IIngredient>()
                .Use<SauceBéarnaise>());
            IIngredient ingredient =
                container.GetInstance<IIngredient>();

            Assert.IsAssignableFrom<SauceBéarnaise>(ingredient);
        }

        [Fact]
        public void ContainerThrowsWhenResolvingUnmappedIngredient()
        {
            var container = new Container();

            Assert.Throws<StructureMapException>(() =>
                container.GetInstance<IIngredient>());
        }

        [Fact]
        public void ContainerResolvesBothAbstractionAndConcreteType()
        {
            var container = new Container();
            container.Configure(r =>
                r.For<IIngredient>().Use<SauceBéarnaise>());
            var sauce = container.GetInstance<SauceBéarnaise>();
            var ingredient = container.GetInstance<IIngredient>();

            Assert.NotNull(sauce);
            Assert.NotNull(ingredient);
        }

        [Fact]
        public void RegisterMultipleTypesInOneGo()
        {
            var container = new Container();
            container.Configure(r =>
            {
                r.For<IIngredient>()
                    .Use<SauceBéarnaise>();
                r.For<ICourse>()
                    .Use<Course>();
            });

            var course = container.GetInstance<ICourse>();

            var c = Assert.IsAssignableFrom<Course>(course);
            Assert.True(c.Ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterMultipleTypesOneAtATime()
        {
            var container = new Container();
            container.Configure(r => r
                .For<IIngredient>()
                .Use<SauceBéarnaise>());
            container.Configure(r => r
                .For<ICourse>()
                .Use<Course>());

            var course = container.GetInstance<ICourse>();

            var c = Assert.IsAssignableFrom<Course>(course);
            Assert.True(c.Ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterMultipleImplementationsOfTheSameType()
        {
            var container = new Container();
            container.Configure(registry =>
            {
                registry.For<IIngredient>().Use<Steak>();
                registry.For<IIngredient>().Use<SauceBéarnaise>();
            });

            IList<IIngredient> ingredients =
                container.GetAllInstances<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterIngredientsThrowsOnUnresolvableTypes()
        {
            var container = new Container();
            container.Configure(x =>
            {
                x.For<ICourse>().Use<CordonBleu>();
                x.For<ICourse>().Use<ChiliConCarne>();
            });

            Assert.Throws<StructureMapException>(() =>
                container.GetAllInstances<ICourse>());
        }

        [Fact]
        public void RegisterAllIngredients()
        {
            var container = new Container();
            container.Configure(r =>
                r.Scan(s =>
                {
                    s.AssemblyContainingType<Steak>();
                    s.AddAllTypesOf<IIngredient>();

                    s.ExcludeType<Breading>();
                }));

            var ingredients = container.GetAllInstances<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterAllSauces()
        {
            var container = new Container();
            container.Configure(r =>
                r.Scan(s =>
                {
                    s.AssemblyContainingType<Steak>();
                    s.AddAllTypesOf<IIngredient>();
                    s.Include(t => t.Name.StartsWith("Sauce"));
                }));

            var sauces = container.GetAllInstances<IIngredient>();

            Assert.True(sauces.OfType<SauceBéarnaise>().Any());
            Assert.True(sauces.OfType<SauceHollandaise>().Any());
            Assert.True(sauces.OfType<SauceMousseline>().Any());

            Assert.False(sauces.OfType<Steak>().Any());
        }

        [Fact]
        public void RegisterAllSaucesUsingCustomConvention()
        {
            var container = new Container();
            container.Configure(r =>
                r.Scan(s =>
                {
                    s.AssemblyContainingType<Steak>();
                    s.Convention<SauceConvention>();
                }));

            var sauces = container.GetAllInstances<IIngredient>();

            Assert.True(sauces.OfType<SauceBéarnaise>().Any());
            Assert.True(sauces.OfType<SauceHollandaise>().Any());
            Assert.True(sauces.OfType<SauceMousseline>().Any());

            Assert.False(sauces.OfType<Steak>().Any());
        }

        [Fact]
        public void RegisterBasedOnXmlConfiguration()
        {
            var configName =
                AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            var container = new Container();
            container.Configure(r =>
                r.AddConfigurationFromXmlFile(configName));

            var ingredient = container.GetInstance<IIngredient>();

            Assert.NotNull(ingredient);
        }

        [Fact]
        public void XmlConfigurationWinsWhenInstalledLast()
        {
            var configName = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            var container = new Container();
            container.Configure(r => r.For<IIngredient>().Use<SauceBéarnaise>());
            container.Configure(r =>
                r.AddConfigurationFromXmlFile(configName));

            var ingredient = container.GetInstance<IIngredient>();

            Assert.IsAssignableFrom<Steak>(ingredient);
        }

        [Fact]
        public void RegisterAllIngredientsUsingRegistry()
        {
            var container = new Container();
            container.Configure(r =>
                r.AddRegistry<MenuRegistry>());

            var ingredients = container.GetAllInstances<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterDefaultInstanceUsingRegistry()
        {
            var container = new Container();
            container.Configure(r =>
                r.AddRegistry<MenuRegistry>());

            var ingredient = container.GetInstance<ICourse>();
            Assert.NotNull(ingredient);
        }

        [Fact]
        public void CombineXmlConfigurationAndRegistry()
        {
            var configName =
                AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            var container = new Container();
            container.Configure(r =>
            {
                r.AddConfigurationFromXmlFile(configName);
                r.AddRegistry<MenuRegistry>();
            });

            var course = container.GetInstance<ICourse>();
            var ingredient = container.GetInstance<IIngredient>();

            Assert.NotNull(course);
            Assert.IsAssignableFrom<Steak>(ingredient);
        }

        [Fact]
        public void UseRegistryThroughConstructor()
        {
            var container = new Container(new MenuRegistry());

            var ingredients = container.GetAllInstances<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void ImplicitlyRegisterSauceBéarnaiseAsTransient()
        {
            var container = new Container();

            var sauce1 = container.GetInstance<SauceBéarnaise>();
            var sauce2 = container.GetInstance<SauceBéarnaise>();

            Assert.NotSame(sauce1, sauce2);
        }

        [Fact]
        public void RegisterSauceBéarnaiseAsSingleton()
        {
            var container = new Container();
            container.Configure(r =>
                r.For<SauceBéarnaise>().Singleton());

            var sauce1 = container.GetInstance<SauceBéarnaise>();
            var sauce2 = container.GetInstance<SauceBéarnaise>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void RegisterIngredientAsSingleton()
        {
            var container = new Container();
            container.Configure(r =>
                r.For<IIngredient>().Singleton().Use<SauceBéarnaise>());

            var ingredient1 = container.GetInstance<IIngredient>();
            var ingredient2 = container.GetInstance<IIngredient>();

            Assert.Same(ingredient1, ingredient2);
        }

        [Fact]
        public void RegisterIngredientAsSingletonDoesNotRegistersConcreteTypeAsSingleton()
        {
            var container = new Container();
            container.Configure(r =>
                r.For<IIngredient>().Singleton().Use<SauceBéarnaise>());

            var sauce1 = container.GetInstance<SauceBéarnaise>();
            var sauce2 = container.GetInstance<SauceBéarnaise>();

            Assert.NotSame(sauce1, sauce2);
        }

        [Fact]
        public void RegisterSauceBéarnaiseAsSingletonUsingLifecycle()
        {
            var container = new Container();
            container.Configure(r =>
                r.For<SauceBéarnaise>().LifecycleIs(new SingletonLifecycle()));

            var sauce1 = container.GetInstance<SauceBéarnaise>();
            var sauce2 = container.GetInstance<SauceBéarnaise>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void RegisterSauceBéarnaiseAsUnique()
        {
            var container = new Container();
            container.Configure(r => r
                .For<SauceBéarnaise>()
                .LifecycleIs(new UniquePerRequestLifecycle()));

            var sauce1 = container.GetInstance<SauceBéarnaise>();
            var sauce2 = container.GetInstance<SauceBéarnaise>();

            Assert.NotSame(sauce1, sauce2);
        }

        [Fact]
        public void RegisterIngredientExplicitlyAsPerRequest()
        {
            var container = new Container();
            container.Configure(r => r
                .For<IIngredient>()
                .LifecycleIs(null)
                .Use<SauceBéarnaise>());

            var ingredient1 = container.GetInstance<IIngredient>();
            var ingredient2 = container.GetInstance<IIngredient>();

            Assert.NotSame(ingredient1, ingredient2);
        }

        [Fact]
        public void RegisterIngredientImplicitlyAsPerRequest()
        {
            var container = new Container();
            container.Configure(r => r
                .For<IIngredient>()
                .Use<SauceBéarnaise>());

            var ingredient1 = container.GetInstance<IIngredient>();
            var ingredient2 = container.GetInstance<IIngredient>();

            Assert.NotSame(ingredient1, ingredient2);
        }

        [Fact]
        public void RegisterAllIngredientsAsSingletons()
        {
            var container = new Container();
            container.Configure(r =>
                r.Scan(s =>
                {
                    s.AssemblyContainingType<Steak>();
                    s.AddAllTypesOf<IIngredient>();
                    s.Convention<SingletonConvention>();

                    s.ExcludeType<Breading>();
                }));

            var ingredients1 = container.GetAllInstances<IIngredient>();
            var ingredients2 = container.GetAllInstances<IIngredient>();

            Assert.Same(
                ingredients1.OfType<SauceBéarnaise>().Single(),
                ingredients2.OfType<SauceBéarnaise>().Single());
        }

        [Fact]
        public void SingletonRegistrationConventionCorrectlyScopesConcreteType()
        {
            var container = new Container();
            container.Configure(r =>
                r.Scan(s =>
                {
                    s.AssemblyContainingType<Steak>();
                    s.AddAllTypesOf<IIngredient>();
                    s.Include(t => typeof(IIngredient).IsAssignableFrom(t));
                    s.Convention<SingletonConvention>();

                    s.ExcludeType<Breading>();
                }));

            var sauce1 = container.GetInstance<SauceBéarnaise>();
            var sauce2 = container.GetInstance<SauceBéarnaise>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void SingletonRegistrationConventionDoesNotImpactOtherRegistrations()
        {
            var container = new Container();
            container.Configure(r =>
            {
                r.For<ICourse>().Use<CordonBleu>();
                r.Scan(s =>
                {
                    s.AssemblyContainingType<Steak>();
                    s.AddAllTypesOf<IIngredient>();
                    s.Include(t => typeof(IIngredient).IsAssignableFrom(t));
                    s.Convention<SingletonConvention>();

                    s.ExcludeType<Breading>();
                });
            });

            var course1 = container.GetInstance<ICourse>();
            var course2 = container.GetInstance<ICourse>();

            Assert.NotSame(course1, course2);
        }

        [Fact]
        public void XmlConfiguredSingletonIngredientIsCorrectlyResolved()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Resources.SingletonIngredient);
            var xmlNode = xmlDoc.DocumentElement;

            var container = new Container();
            container.Configure(r =>
                r.AddConfigurationFromNode(xmlNode));

            var sauce1 = container.GetInstance<IIngredient>();
            var sauce2 = container.GetInstance<IIngredient>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void ResolveMultipleServicesWithSameSingletonDependency()
        {
            var container = new Container();
            container.Configure(x =>
                {
                    x.For<IIngredient>().Singleton().Use<Shrimp>();
                    x.For<OliveOil>().Singleton();
                    x.For<EggYolk>().Singleton();
                    x.For<Vinegar>().Singleton();
                    x.For<IIngredient>().Singleton().Use<Vinaigrette>();
                    x.For<IIngredient>().Singleton().Use<Mayonnaise>();
                    x.For<Course>().Singleton();
                });

            var c = container.GetInstance<Course>();

            Assert.Same(c.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c.Ingredients.OfType<Mayonnaise>().Single().Oil);
        }

        [Fact]
        public void ResolveServicesWithSameUniqueDependency()
        {
            var container = new Container();
            container.Configure(x =>
            {
                var unique = new UniquePerRequestLifecycle();
                x.For<IIngredient>().LifecycleIs(unique)
                    .Use<Shrimp>();
                x.For<OliveOil>().LifecycleIs(unique);
                x.For<EggYolk>().LifecycleIs(unique);
                x.For<Vinegar>().LifecycleIs(unique);
                x.For<IIngredient>().LifecycleIs(unique)
                    .Use<Vinaigrette>();
                x.For<IIngredient>().LifecycleIs(unique)
                    .Use<Mayonnaise>();
                x.For<Course>().LifecycleIs(unique);
            });

            var c1 = container.GetInstance<Course>();
            var c2 = container.GetInstance<Course>();

            Assert.NotSame(
                c1.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c1.Ingredients.OfType<Mayonnaise>().Single().Oil);
            Assert.NotSame(
                c2.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c2.Ingredients.OfType<Mayonnaise>().Single().Oil);
            Assert.NotSame(
                c1.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c2.Ingredients.OfType<Vinaigrette>().Single().Oil);
        }

        [Fact]
        public void ResolveServicesWithSamePerRequestDependency()
        {
            var container = new Container();
            container.Configure(x =>
            {
                x.For<IIngredient>().Use<Shrimp>();
                x.For<OliveOil>();
                x.For<EggYolk>();
                x.For<Vinegar>();
                x.For<IIngredient>().Use<Vinaigrette>();
                x.For<IIngredient>().Use<Mayonnaise>();
            });

            var c1 = container.GetInstance<Course>();
            var c2 = container.GetInstance<Course>();

            Assert.Same(
                c1.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c1.Ingredients.OfType<Mayonnaise>().Single().Oil);
            Assert.Same(
                c2.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c2.Ingredients.OfType<Mayonnaise>().Single().Oil);
            Assert.NotSame(
                c1.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c2.Ingredients.OfType<Vinaigrette>().Single().Oil);
        }

        [Fact]
        public void ResolveServicesWithSameExplicitPerRequestDependency()
        {
            var container = new Container();
            container.Configure(x =>
            {
                x.For<IIngredient>().LifecycleIs(null)
                    .Use<Shrimp>();
                x.For<OliveOil>().LifecycleIs(null);
                x.For<EggYolk>().LifecycleIs(null);
                x.For<Vinegar>().LifecycleIs(null);
                x.For<IIngredient>().LifecycleIs(null)
                    .Use<Vinaigrette>();
                x.For<IIngredient>().LifecycleIs(null)
                    .Use<Mayonnaise>();
                x.For<Course>().LifecycleIs(null);
            });

            var c1 = container.GetInstance<Course>();
            var c2 = container.GetInstance<Course>();

            Assert.Same(
                c1.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c1.Ingredients.OfType<Mayonnaise>().Single().Oil);
            Assert.Same(
                c2.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c2.Ingredients.OfType<Mayonnaise>().Single().Oil);
            Assert.NotSame(
                c1.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c2.Ingredients.OfType<Vinaigrette>().Single().Oil);
        }

        [Fact]
        public void DeclaringPerWebRequestLifestyle()
        {
            var container = new Container();
            container.Configure(r => r
                .For<IIngredient>()
                .HttpContextScoped()
                .Use<SauceBéarnaise>());
        }

        [Fact]
        public void UsingCachingIngredientWithLongTimeout()
        {
            var container = new Container();

            var lease = new SlidingLease(TimeSpan.FromMinutes(1));
            var cache = new CacheLifecycle(lease);
            container.Configure(r => r
                .For<IIngredient>()
                .LifecycleIs(cache)
                .Use<SauceBéarnaise>());

            var ingredient1 = container.GetInstance<IIngredient>();
            var ingredient2 = container.GetInstance<IIngredient>();

            Assert.Same(ingredient1, ingredient2);
        }

        [Fact]
        public void UsingCachingIngredientWithShortTimeout()
        {
            var lease = new SlidingLease(TimeSpan.FromTicks(1));
            var lifecycle = new CacheLifecycle(lease);

            var container = new Container();
            container.Configure(r => r
                .For<IIngredient>()
                .LifecycleIs(lifecycle)
                .Use<SauceBéarnaise>());

            var ingredient1 = container.GetInstance<IIngredient>();
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
            var ingredient2 = container.GetInstance<IIngredient>();

            Assert.NotSame(ingredient1, ingredient2);
        }

        [Fact]
        public void ReuseTheSameCacheForSeveralComponents()
        {
            var container = new Container();

            var lease = new SlidingLease(TimeSpan.FromMinutes(1));
            var cache = new CacheLifecycle(lease);
            container.Configure(r =>
            {
                r.For<IIngredient>().LifecycleIs(cache).Use<Steak>();
                r.For<ICourse>().LifecycleIs(cache).Use<Course>();
            });

            var course1 = container.GetInstance<ICourse>();
            var course2 = container.GetInstance<ICourse>();

            var ingredient1 = Assert.IsAssignableFrom<Course>(course1).Ingredients.Single();
            var ingredient2 = Assert.IsAssignableFrom<Course>(course2).Ingredients.Single();

            Assert.Same(ingredient1, ingredient2);
        }

        [Fact]
        public void UseDifferentCachesForDifferentComponents()
        {
            var container = new Container();

            container.Configure(r => r
                .For<IIngredient>()
                .LifecycleIs(
                    new CacheLifecycle(
                        new SlidingLease(
                            TimeSpan.FromHours(1))))
                .Use<Steak>());
            container.Configure(r => r
                .For<ICourse>()
                .LifecycleIs(
                    new CacheLifecycle(
                        new SlidingLease(
                            TimeSpan.FromMinutes(15))))
                .Use<Course>());

            var course = container.GetInstance<ICourse>();
            var ingredients = Assert.IsAssignableFrom<Course>(course).Ingredients;
            Assert.True(ingredients.OfType<Steak>().Any());
        }

        [Fact]
        public void EmptyContainerResolvesAllToEmptyList()
        {
            var container = new Container();

            var ingredients = container.GetAllInstances<IIngredient>();

            Assert.NotNull(ingredients);
        }

        [Fact]
        public void RegisterSteakAsDefaultIngredient()
        {
            var container = new Container();
            container.Configure(r =>
            {
                r.For<IIngredient>().Use<SauceBéarnaise>();
                r.For<IIngredient>().Use<Steak>();
            });

            var ingredient = container.GetInstance<IIngredient>();
            Assert.IsAssignableFrom<Steak>(ingredient);
        }

        [Fact]
        public void RegisterMultipleIngredientsPreservesAll()
        {
            var container = new Container();
            container.Configure(r => r.For<IIngredient>().Use<SauceBéarnaise>());
            container.Configure(r => r.For<IIngredient>().Use<Steak>());

            var ingredients = container.GetAllInstances<IIngredient>();
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
            Assert.True(ingredients.OfType<Steak>().Any());
        }

        [Fact]
        public void RegisterNamedIngredients()
        {
            var container = new Container();
            container.Configure(r =>
            {
                r.For<IIngredient>()
                    .Use<SauceBéarnaise>()
                    .Named("sauce");
                r.For<IIngredient>()
                    .Use<Steak>()
                    .Named("meat");
            });

            var meat = container.GetInstance<IIngredient>("meat");
            var sauce = container.GetInstance<IIngredient>("sauce");

            Assert.IsAssignableFrom<Steak>(meat);
            Assert.IsAssignableFrom<SauceBéarnaise>(sauce);
        }

        [Fact]
        public void CreateThreeCourseMealFromNamedInstances()
        {
            var container = new Container();
            container.Configure(r => r
                .For<ICourse>()
                .Use<Rillettes>()
                .Named("entrée"));
            container.Configure(r => r
                .For<ICourse>()
                .Use<CordonBleu>()
                .Named("mainCourse"));
            container.Configure(r => r
                .For<ICourse>()
                .Use<MousseAuChocolat>()
                .Named("dessert"));

            container.Configure(r => r
                .For<IMeal>()
                .Use<ThreeCourseMeal>()
                .Ctor<ICourse>("entrée").Is(i =>
                    i.TheInstanceNamed("entrée"))
                .Ctor<ICourse>("mainCourse").Is(i =>
                    i.TheInstanceNamed("mainCourse"))
                .Ctor<ICourse>("dessert").Is(i =>
                    i.TheInstanceNamed("dessert")));

            var meal = container.GetInstance<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<ThreeCourseMeal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Entrée);
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.MainCourse);
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Dessert);
        }

        [Fact]
        public void CreateThreeCourseMealFromReferencedInstances()
        {
            var container = new Container();
            container.Configure(r =>
            {
                var entrée =
                    r.For<ICourse>().Use<Rillettes>();
                var mainCourse =
                    r.For<ICourse>().Use<CordonBleu>();
                var dessert =
                    r.For<ICourse>().Use<MousseAuChocolat>();

                r.For<IMeal>()
                    .Use<ThreeCourseMeal>()
                    .Ctor<ICourse>("entrée").Is(entrée)
                    .Ctor<ICourse>("mainCourse").Is(mainCourse)
                    .Ctor<ICourse>("dessert").Is(dessert);
            });

            var meal = container.GetInstance<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<ThreeCourseMeal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Entrée);
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.MainCourse);
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Dessert);
        }

        [Fact]
        public void CreateMultiCourseMealByExplicitlyPickingNamedCourses()
        {
            var container = new Container();
            container.Configure(r => r.For<ICourse>().Use<Rillettes>().Named("entrée"));
            container.Configure(r => r.For<ICourse>().Use<LobsterBisque>().Named("entrée1"));
            container.Configure(r => r.For<ICourse>().Use<CordonBleu>().Named("mainCourse"));
            container.Configure(r => r.For<ICourse>().Use<MousseAuChocolat>().Named("dessert"));

            container.Configure(r => r
                .For<IMeal>()
                .Use<Meal>()
                .EnumerableOf<ICourse>().Contains(i =>
                {
                    i.TheInstanceNamed("entrée");
                    i.TheInstanceNamed("mainCourse");
                    i.TheInstanceNamed("dessert");
                }));

            var meal = container.GetInstance<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void CreateMultiCourseMealByExplicitlyPickingCourses()
        {
            var container = new Container();
            container.Configure(r =>
            {
                var entrée = r.For<ICourse>().Use<Rillettes>();
                var entrée1 = r.For<ICourse>().Use<LobsterBisque>();
                var mainCourse = r.For<ICourse>().Use<CordonBleu>();
                var dessert = r.For<ICourse>().Use<MousseAuChocolat>();

                r.For<IMeal>().Use<Meal>()
                    .EnumerableOf<ICourse>()
                    .Contains(entrée, mainCourse, dessert);
            });

            var meal = container.GetInstance<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void CreateMultiCourseMealFromAllCourses()
        {
            var container = new Container();
            container.Configure(r => r.For<ICourse>().Use<Rillettes>().Named("entrée"));
            container.Configure(r => r.For<ICourse>().Use<CordonBleu>().Named("mainCourse"));
            container.Configure(r => r.For<ICourse>().Use<MousseAuChocolat>().Named("dessert"));

            container.Configure(r => r.For<IMeal>().Use<Meal>());

            var meal = container.GetInstance<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void ExplicitlyConfigureCotolettaWithNamedCutlet()
        {
            var container = new Container();
            container.Configure(r => r
                .For<IIngredient>()
                .Use<VealCutlet>()
                .Named("cutlet"));
            container.Configure(r => r
                .For<IIngredient>()
                .Use<Breading>()
                .Ctor<IIngredient>()
                .Is(i => i.TheInstanceNamed("cutlet")));

            var cotoletta = container.GetInstance<IIngredient>();

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void ExplicitlyConfigureCotolettaWithCutlet()
        {
            var container = new Container();
            container.Configure(r =>
            {
                var cutlet = r.For<IIngredient>().Use<VealCutlet>();
                r.For<IIngredient>().Use<Breading>()
                    .Ctor<IIngredient>().Is(cutlet);
            });

            var cotoletta = container.GetInstance<IIngredient>();

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void EnrichCutletWithBreading()
        {
            var container = new Container();
            container.Configure(r => r
                .For<IIngredient>().Use<VealCutlet>()
                .EnrichWith(i => new Breading(i)));

            var cotoletta = container.GetInstance<IIngredient>();

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void ResolveChiliConCarneBasedOnConfiguredSpicynessThrows()
        {
            var container = new Container();
            container.Configure(r => r.For<Spiciness>().Use(Spiciness.Hot));
            container.Configure(r => r.For<ICourse>().Use<ChiliConCarne>());

            Assert.Throws<StructureMapException>(() =>
                container.GetInstance<ICourse>());
        }

        [Fact]
        public void ResolveChiliConCarne()
        {
            var container = new Container();
            container.Configure(r => r
                .For<ICourse>()
                .Use<ChiliConCarne>()
                .Ctor<Spiciness>()
                .Is(Spiciness.Hot));

            var course = container.GetInstance<ICourse>();

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Hot, chili.Spicyness);
        }

        [Fact]
        public void ResolveConcreteChiliConCarne()
        {
            var container = new Container();
            container.Configure(r => r
                .ForConcreteType<ChiliConCarne>()
                .Configure.Ctor<Spiciness>().Is(Spiciness.Medium));

            var chili = container.GetInstance<ChiliConCarne>();
            Assert.Equal(Spiciness.Medium, chili.Spicyness);
        }

        [Fact]
        public void ResolveChiliConCarneWithNamedSpicyness()
        {
            var container = new Container();
            container.Configure(r => r
                .For<Spiciness>()
                .Use(Spiciness.Hot)
                .Named("hot"));
            container.Configure(r => r
                .For<Spiciness>()
                .Use(Spiciness.Medium)
                .Named("medium"));
            container.Configure(r => r
                .ForConcreteType<ChiliConCarne>()
                .Configure.Ctor<Spiciness>().Is((IContext ctx) =>
                    ctx.GetInstance<Spiciness>("medium")));

            var chili = container.GetInstance<ChiliConCarne>();
            Assert.Equal(Spiciness.Medium, chili.Spicyness);
        }

        [Fact]
        public void ResolveStronglyTypedChiliConCarne()
        {
            var container = new Container();
            container.Configure(r => r.For<ICourse>().Use(() => new ChiliConCarne(Spiciness.Hot)));

            var course = container.GetInstance<ICourse>();

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Hot, chili.Spicyness);
        }

        [Fact]
        public void ResolveJunkFood()
        {
            var container = new Container();
            container.Configure(r => r
                .For<IMeal>()
                .Use(() =>
                    JunkFoodFactory.Create("chicken meal")));

            var meal = container.GetInstance<IMeal>();

            var junk = Assert.IsAssignableFrom<JunkFood>(meal);
            Assert.Equal("chicken meal", junk.Name);
        }

        [Fact(Skip = "Fails intermittently. Reported at http://groups.google.com/group/structuremap-users/browse_thread/thread/f29113aafb9b0608")]
        public void ResolveBasicCaesarSalad()
        {
            var container = new Container();
            container.Configure(r => r.For<ICourse>().Use<CaesarSalad>());

            var meal = container.GetInstance<ICourse>();

            Assert.IsAssignableFrom<CaesarSalad>(meal);
        }

        [Fact]
        public void ResolveCaesarSaladWithDefaultSetter()
        {
            var container = new Container();
            container.Configure(r => r.For<IIngredient>().Use<Chicken>());
            container.Configure(r => r.For<ICourse>().Use<CaesarSalad>().Setter<IIngredient>().IsTheDefault());

            var meal = container.GetInstance<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWithExplicitSetter()
        {
            var container = new Container();
            container.Configure(r =>
            {
                var chicken = r.For<IIngredient>().Use<Chicken>();
                r.For<ICourse>().Use<CaesarSalad>()
                    .Setter<IIngredient>().Is(chicken);
            });

            var meal = container.GetInstance<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWithExtraChicken()
        {
            var container = new Container();
            container.Configure(r =>
                r.For<IIngredient>().Use<Chicken>());
            container.Configure(r =>
                r.For<ICourse>().Use<CaesarSalad>());
            container.Configure(r =>
                r.FillAllPropertiesOfType<IIngredient>());

            var meal = container.GetInstance<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }

        [Fact]
        public void ResolveSauceBearnaiseUsingObjectFactory()
        {
            SauceBéarnaise sauce =
                ObjectFactory.GetInstance<SauceBéarnaise>();

            Assert.NotNull(sauce);
        }

        #region IDisposable Members

        public void Dispose()
        {
            ObjectFactory.ResetDefaults();
        }

        #endregion
    }
}
