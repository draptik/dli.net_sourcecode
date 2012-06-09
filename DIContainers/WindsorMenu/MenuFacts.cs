using System;
using System.Linq;
using System.Threading;
using Castle.Core.Resource;
using Castle.Facilities.FactorySupport;
using Castle.MicroKernel;
using Castle.MicroKernel.Handlers;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;
using Ploeh.Samples.CacheModel;
using Ploeh.Samples.Menu.Windsor.Properties;
using Ploeh.Samples.MenuModel;
using Xunit;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace Ploeh.Samples.Menu.Windsor
{
    public class MenuFacts
    {
        [Fact]
        public void ContainerResolvesSauceBéarnaise()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<SauceBéarnaise>());
            SauceBéarnaise sauce = container.Resolve<SauceBéarnaise>();

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerResolvesIngredientToSauceBéarnaise()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<SauceBéarnaise>());
            IIngredient ingredient = container.Resolve<IIngredient>();

            Assert.IsAssignableFrom<SauceBéarnaise>(ingredient);
        }

        [Fact]
        public void ContainerThrowsWhenResolvingUnmappedIngredient()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<SauceBéarnaise>());

            Assert.Throws<ComponentNotFoundException>(() =>
                container.Resolve<IIngredient>());
        }

        [Fact]
        public void ContainerResolvesBothAbstractionAndConcreteType()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<SauceBéarnaise, IIngredient>());
            var sauce = container.Resolve<SauceBéarnaise>();
            var ingredient = container.Resolve<IIngredient>();

            Assert.NotNull(sauce);
            Assert.NotNull(ingredient);
        }

        [Fact]
        public void ContainerThrowsDescriptiveException()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IIngredient>().ImplementedBy<Mayonnaise>());
            container.Register(Component.For<OliveOil>());

            var e = Assert.Throws<HandlerException>(() =>
                container.Resolve<IIngredient>());
        }

        [Fact]
        public void RegisterMultipleTypes()
        {
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<SauceBéarnaise>());
            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<Course>());

            var course = container.Resolve<ICourse>();
            Assert.NotNull(course);
        }

        [Fact]
        public void RegisterMultipleImplementationsOfTheSameType()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<Steak>());
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<SauceBéarnaise>());

            IIngredient[] ingredients = container.ResolveAll<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterIngredientsIgnoresUnresolvableTypes()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<ICourse>().ImplementedBy<CordonBleu>());
            container.Register(Component.For<ICourse>().ImplementedBy<ChiliConCarne>());

            var ingredients = container.ResolveAll<ICourse>();

            Assert.True(ingredients.OfType<CordonBleu>().Any());
            Assert.False(ingredients.OfType<ChiliConCarne>().Any());
        }

        [Fact]
        public void RegisterAllIngredients()
        {
            var container = new WindsorContainer();
            container.Register(AllTypes
                .FromAssemblyContaining<Steak>()
                .BasedOn<IIngredient>());

            var ingredients = container.ResolveAll<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterAllSauces()
        {
            var container = new WindsorContainer();
            container.Register(AllTypes
                .FromAssemblyContaining<SauceBéarnaise>()
                .Where(t => t.Name.StartsWith("Sauce"))
                .WithService.AllInterfaces());

            var sauces = container.ResolveAll<IIngredient>();

            Assert.True(sauces.OfType<SauceBéarnaise>().Any());
            Assert.True(sauces.OfType<SauceHollandaise>().Any());
            Assert.True(sauces.OfType<SauceMousseline>().Any());

            Assert.False(sauces.OfType<Steak>().Any());
        }

        [Fact]
        public void RegisterBasedOnXmlConfiguration()
        {
            var container = new WindsorContainer();
            container.Install(Configuration.FromAppConfig());

            var ingredient = container.Resolve<IIngredient>();

            Assert.NotNull(ingredient);
        }

        [Fact]
        public void XmlConfigurationWinsWhenInstalledFirst()
        {
            var container = new WindsorContainer();
            container.Install(Configuration.FromAppConfig());
            container.Register(Component.For<IIngredient>().ImplementedBy<SauceBéarnaise>());

            var ingredient = container.Resolve<IIngredient>();

            Assert.IsAssignableFrom<Steak>(ingredient);
        }

        [Fact]
        public void RegisterAllIngredientsUsingInstaller()
        {
            var container = new WindsorContainer();
            container.Install(new IngredientInstaller());

            var ingredients = container.ResolveAll<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterAllIngredientsUsingInstallerFromXml()
        {
            var container = new WindsorContainer();
            container.Install(Configuration.FromXml(new StaticContentResource(Resources.IngredientInstaller)));

            var ingredients = container.ResolveAll<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterSauceBéarnaiseAsTransient()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<SauceBéarnaise>()
                .LifeStyle.Transient);

            var sauce1 = container.Resolve<SauceBéarnaise>();
            var sauce2 = container.Resolve<SauceBéarnaise>();

            Assert.NotSame(sauce1, sauce2);
        }

        [Fact]
        public void ExplicitlyRegisterSauceBéarnaiseAsSingleton()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<SauceBéarnaise>()
                .LifeStyle.Singleton);

            var sauce1 = container.Resolve<SauceBéarnaise>();
            var sauce2 = container.Resolve<SauceBéarnaise>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void ImplicitlyRegisterSauceBéarnaiseAsSingleton()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<SauceBéarnaise>());

            var sauce1 = container.Resolve<SauceBéarnaise>();
            var sauce2 = container.Resolve<SauceBéarnaise>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void XmlConfiguredTransientIngredientIsCorrectlyResolved()
        {
            var container = new WindsorContainer();
            container.Install(Configuration.FromXml(new StaticContentResource(Resources.TransientIngredient)));

            var sauce1 = container.Resolve<IIngredient>();
            var sauce2 = container.Resolve<IIngredient>();

            Assert.NotSame(sauce1, sauce2);
        }

        [Fact]
        public void ReleaseParsleyAsSingletonDoesNotDisposeParsley()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IIngredient>().ImplementedBy<Parsley>().LifeStyle.Singleton);

            var ingredient = container.Resolve<IIngredient>();

            container.Release(ingredient);

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.False(parsley.IsDisposed);
        }

        [Fact]
        public void ReleaseParsleyAsTransientDisposesParsley()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IIngredient>().ImplementedBy<Parsley>().LifeStyle.Transient);

            var ingredient = container.Resolve<IIngredient>();

            container.Release(ingredient);

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.True(parsley.IsDisposed);
        }

        [Fact]
        public void ResolveServicesWithSameSingletonDependency()
        {
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Register(Component.For<IIngredient>().ImplementedBy<Shrimp>());
            container.Register(Component.For<OliveOil>());
            container.Register(Component.For<EggYolk>());
            container.Register(Component.For<Vinegar>());
            container.Register(Component.For<IIngredient>().ImplementedBy<Vinaigrette>());
            container.Register(Component.For<IIngredient>().ImplementedBy<Mayonnaise>());
            container.Register(Component.For<Course>());

            var c = container.Resolve<Course>();

            Assert.Same(c.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c.Ingredients.OfType<Mayonnaise>().Single().Oil);
        }

        [Fact]
        public void ResolveServicesWithSameTransientDependency()
        {
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            container.Register(Component.For<IIngredient>().ImplementedBy<Shrimp>().LifeStyle.Transient);
            container.Register(Component.For<OliveOil>().LifeStyle.Transient);
            container.Register(Component.For<EggYolk>().LifeStyle.Transient);
            container.Register(Component.For<Vinegar>().LifeStyle.Transient);
            container.Register(Component.For<IIngredient>().ImplementedBy<Vinaigrette>().LifeStyle.Transient);
            container.Register(Component.For<IIngredient>().ImplementedBy<Mayonnaise>().LifeStyle.Transient);
            container.Register(Component.For<Course>().LifeStyle.Transient);

            var c = container.Resolve<Course>();

            Assert.NotSame(c.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c.Ingredients.OfType<Mayonnaise>().Single().Oil);
        }

        [Fact]
        public void ResolveIngredientFromPool()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<SauceBéarnaise>()
                .LifeStyle.Pooled);

            Func<IIngredient> resolve = () => container.Resolve<IIngredient>();
            var count = 5;
            var ingredients = Enumerable.Repeat(resolve, count).Select(f => f()).Distinct().ToList();
            Assert.Equal(count, ingredients.Count);

            ingredients.ForEach(container.Release);

            var recycled = container.Resolve<IIngredient>();
            Assert.Contains(recycled, ingredients);
        }

        [Fact]
        public void ResolveTooManyFromPoolIsStillPossible()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<SauceBéarnaise>()
                .LifeStyle.PooledWithSize(10, 50));

            Func<IIngredient> resolve = () => container.Resolve<IIngredient>();
            var count = 100;
            var ingredients = Enumerable.Repeat(resolve, count).Select(f => f()).Distinct().ToList();
            Assert.Equal(count, ingredients.Count);
        }

        [Fact]
        public void ReleasingAnIngredientFromAPool()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<SauceBéarnaise>()
                .LifeStyle.PooledWithSize(10, 50));

            var ingredient = container.Resolve<IIngredient>();

            container.Release(ingredient);
        }

        [Fact]
        public void DeclaringPerWebRequestLifestyle()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<SauceBéarnaise>()
                .LifeStyle.PerWebRequest);
        }

        [Fact]
        public void UsingCachingIngredientWithDefaultLease()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<SauceBéarnaise>()
                .LifeStyle.Custom<CacheLifestyleManager>());

            var ingredient1 = container.Resolve<IIngredient>();
            var ingredient2 = container.Resolve<IIngredient>();

            Assert.Same(ingredient1, ingredient2);
        }

        [Fact]
        public void UsingCachingIngredientWithCustomLease()
        {
            var container = new WindsorContainer();
            container.Register(Component
                .For<ILease>()
                .Instance(new SlidingLease(TimeSpan.FromTicks(1))));
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<SauceBéarnaise>()
                .LifeStyle.Custom<CacheLifestyleManager>());

            var ingredient1 = container.Resolve<IIngredient>();
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
            var ingredient2 = container.Resolve<IIngredient>();

            Assert.NotSame(ingredient1, ingredient2);
        }

        [Fact]
        public void EmptyContainerResolvesAllToEmptyArray()
        {
            var container = new WindsorContainer();

            var ingredients = container.ResolveAll<IIngredient>();

            Assert.NotNull(ingredients);
        }

        [Fact]
        public void RegisterSteakAsDefaultIngredient()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<Steak>());
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<SauceBéarnaise>());

            var ingredient = container.Resolve<IIngredient>();
            Assert.IsAssignableFrom<Steak>(ingredient);
        }

        [Fact]
        public void RegisterNamedIngredients()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<Steak>()
                .Named("meat"));
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<SauceBéarnaise>()
                .Named("sauce"));

            var meat = container.Resolve<IIngredient>("meat");
            var sauce = container.Resolve<IIngredient>("sauce");

            Assert.IsAssignableFrom<Steak>(meat);
            Assert.IsAssignableFrom<SauceBéarnaise>(sauce);
        }

        [Fact]
        public void CreateThreeCourseMeal()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<Rillettes>()
                .Named("entrée"));
            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<CordonBleu>()
                .Named("mainCourse"));
            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<MousseAuChocolat>()
                .Named("dessert"));

            container.Register(Component
                .For<IMeal>()
                .ImplementedBy<ThreeCourseMeal>()
                .ServiceOverrides(new
                    {
                        entrée = "entrée",
                        mainCourse = "mainCourse",
                        dessert = "dessert"
                    }));

            var meal = container.Resolve<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<ThreeCourseMeal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Entrée);
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.MainCourse);
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Dessert);
        }

        [Fact]
        public void CreateMultiCourseMealByExplicitlyPickingCourses()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<Rillettes>()
                .Named("entrée"));
            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<LobsterBisque>()
                .Named("entrée1"));
            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<CordonBleu>()
                .Named("mainCourse"));
            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<MousseAuChocolat>()
                .Named("dessert"));

            container.Register(Component
                .For<IMeal>()
                .ImplementedBy<Meal>()
                .ServiceOverrides(new
                    {
                        courses = new[] 
                            {
                                "entrée",
                                "mainCourse",
                                "dessert" 
                            }
                    }));

            var meal = container.Resolve<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void CreateMultiCourseMealFromAllCourses()
        {
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(
                new CollectionResolver(container.Kernel));

            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<Rillettes>()
                .Named("entrée"));
            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<CordonBleu>()
                .Named("mainCourse"));
            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<MousseAuChocolat>()
                .Named("dessert"));

            container.Register(Component
                .For<IMeal>()
                .ImplementedBy<Meal>());

            var meal = container.Resolve<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void ExplicitlyConfigureCotoletta()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<Breading>()
                .ServiceOverrides(new
                    {
                        ingredient = "cutlet"
                    }));
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<VealCutlet>()
                .Named("cutlet"));

            var cotoletta = container.Resolve<IIngredient>();

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void ResolveCotoletta()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<Breading>());
            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<VealCutlet>());

            var cotoletta = container.Resolve<IIngredient>();

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void ResolveChiliConCarne()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<ChiliConCarne>()
                .DependsOn(new
                    {
                        spicyness = Spiciness.Hot
                    }));

            var course = container.Resolve<ICourse>();

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Hot, chili.Spicyness);
        }

        [Fact]
        public void ResolveStronglyTypedChiliConCarne()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<ICourse>()
                .UsingFactoryMethod(() =>
                    new ChiliConCarne(Spiciness.Hot)));

            var course = container.Resolve<ICourse>();

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Hot, chili.Spicyness);
        }

        [Fact]
        public void ResolveJunkFood()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<IMeal>()
                .UsingFactoryMethod(() =>
                    JunkFoodFactory.Create("chicken meal")));

            var meal = container.Resolve<IMeal>();

            var junk = Assert.IsAssignableFrom<JunkFood>(meal);
            Assert.Equal("chicken meal", junk.Name);
        }

        [Fact]
        public void ResolveBasicCaesarSalad()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<CaesarSalad>());

            var course = container.Resolve<ICourse>();

            Assert.IsAssignableFrom<CaesarSalad>(course);
        }

        [Fact]
        public void ResolveBasicCaesarSaladDoesNotFillExtra()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<CaesarSalad>());

            var course = container.Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(course);
            Assert.IsAssignableFrom<NullIngredient>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWithExtraChicken()
        {
            var container = new WindsorContainer();

            container.Register(Component
                .For<IIngredient>()
                .ImplementedBy<Chicken>());
            container.Register(Component
                .For<ICourse>()
                .ImplementedBy<CaesarSalad>());

            var course = container.Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(course);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }
    }
}
