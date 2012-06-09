using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Autofac;
using Ploeh.Samples.MenuModel;
using Autofac.Features.ResolveAnything;
using Autofac.Core.Registration;
using Autofac.Core;
using Autofac.Configuration;

namespace Ploeh.Samples.Menu.Autofac
{
    public class MenuFacts
    {
        [Fact]
        public void ContainerResolvesSauceBéarnaise()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SauceBéarnaise>();
            var container = builder.Build();
            SauceBéarnaise sauce = container.Resolve<SauceBéarnaise>();

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerAutomaticallyResolvesSauceBéarnaise()
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(
                new AnyConcreteTypeNotAlreadyRegisteredSource());
            var container = builder.Build();
            SauceBéarnaise sauce = container.Resolve<SauceBéarnaise>();

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerAutoWiresMayonnaise()
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            var mayo = builder.Build().Resolve<Mayonnaise>();

            Assert.NotNull(mayo);
        }

        [Fact]
        public void ContainerResolvesIngredientToSauceBéarnaise()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SauceBéarnaise>().As<IIngredient>();
            var container = builder.Build();
            IIngredient ingredient = container.Resolve<IIngredient>();

            Assert.IsAssignableFrom<SauceBéarnaise>(ingredient);
        }

        [Fact]
        public void ContainerThrowsWhenResolvingUnmappedIngredient()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();

            Assert.Throws<ComponentNotRegisteredException>(() =>
                container.Resolve<IIngredient>());
        }

        [Fact]
        public void ContainerResolvesBothAbstractionAndConcreteType()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SauceBéarnaise>()
                .As<SauceBéarnaise, IIngredient>();

            var container = builder.Build();
            var sauce = container.Resolve<SauceBéarnaise>();
            var ingredient = container.Resolve<IIngredient>();

            Assert.NotNull(sauce);
            Assert.NotNull(ingredient);
        }

        [Fact]
        public void ContainerResolvesBothAbstractionAndConcreteTypeUsingChainedAs()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SauceBéarnaise>()
                .As<SauceBéarnaise>().As<IIngredient>();

            var container = builder.Build();
            var sauce = container.Resolve<SauceBéarnaise>();
            var ingredient = container.Resolve<IIngredient>();

            Assert.NotNull(sauce);
            Assert.NotNull(ingredient);
        }

        [Fact]
        public void RegisterMultipleTypes()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SauceBéarnaise>().As<IIngredient>();
            builder.RegisterType<Course>().As<ICourse>();

            var course = builder.Build().Resolve<ICourse>();
            Assert.NotNull(course);
        }

        [Fact]
        public void RegisterMultipleImplementationsOfTheSameType()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SauceBéarnaise>().As<IIngredient>();
            builder.RegisterType<Steak>().As<IIngredient>();

            var container = builder.Build();
            var ingredients = container.Resolve<IEnumerable<IIngredient>>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void ResolvingArraysIsPossible()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SauceBéarnaise>().As<IIngredient>();
            builder.RegisterType<Steak>().As<IIngredient>();

            var container = builder.Build();
            var ingredients = container.Resolve<IIngredient[]>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void ResolveAllIngredientsThrowsOnUnresolvableTypes()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CordonBleu>().As<ICourse>();
            builder.RegisterType<ChiliConCarne>().As<ICourse>();

            Assert.Throws<DependencyResolutionException>(() =>
                builder.Build().Resolve<IEnumerable<ICourse>>());
        }

        [Fact]
        public void RegisterAllIngredients()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(Steak).Assembly)
                .As<IIngredient>();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            var ingredients = builder.Build().Resolve<IEnumerable<IIngredient>>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterAllSauces()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(Steak).Assembly)
                .Where(t => t.Name.StartsWith("Sauce"))
                .As<IIngredient>();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            var sauces = builder.Build().Resolve<IEnumerable<IIngredient>>();

            Assert.True(sauces.OfType<SauceBéarnaise>().Any());
            Assert.True(sauces.OfType<SauceHollandaise>().Any());
            Assert.True(sauces.OfType<SauceMousseline>().Any());

            Assert.False(sauces.OfType<Steak>().Any());
        }

        [Fact]
        public void RegisterBasedOnXmlConfiguration()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ConfigurationSettingsReader());

            var ingredient = builder.Build().Resolve<IIngredient>();

            Assert.NotNull(ingredient);
        }

        [Fact]
        public void XmlConfigurationWinsWhenRegisteredLast()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SauceBéarnaise>().As<IIngredient>();
            builder.RegisterModule(new ConfigurationSettingsReader());

            var ingredient = builder.Build().Resolve<IIngredient>();

            Assert.IsAssignableFrom<Steak>(ingredient);
        }

        [Fact]
        public void RegisterAllIngredientsUsingModule()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<IngredientModule>();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            var ingredients = builder.Build().Resolve<IEnumerable<IIngredient>>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterModuleUsingInstanceMethod()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new IngredientModule());
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            var ingredients = builder.Build().Resolve<IEnumerable<IIngredient>>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterModuleUsingXml()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ConfigurationSettingsReader("autofac3"));
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            var ingredients = builder.Build().Resolve<IEnumerable<IIngredient>>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterSauceBéarnaiseAsSingleton()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SauceBéarnaise>().SingleInstance();
            var container = builder.Build();

            var sauce1 = container.Resolve<SauceBéarnaise>();
            var sauce2 = container.Resolve<SauceBéarnaise>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void RegisterIngredientAsSingletonStatedLast()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterType<SauceBéarnaise>()
                .As<IIngredient>()
                .SingleInstance();
            var container = builder.Build();

            var ingredient1 = container.Resolve<IIngredient>();
            var ingredient2 = container.Resolve<IIngredient>();

            Assert.Same(ingredient1, ingredient2);
        }

        [Fact]
        public void RegisterIngredientAsSingletonStatedEarlier()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterType<SauceBéarnaise>()
                .SingleInstance()
                .As<IIngredient>();
            var container = builder.Build();

            var ingredient1 = container.Resolve<IIngredient>();
            var ingredient2 = container.Resolve<IIngredient>();

            Assert.Same(ingredient1, ingredient2);
        }

        [Fact]
        public void ExplicitlyRegisterSauceBéarnaiseAsTransient()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterType<SauceBéarnaise>()
                .InstancePerDependency();
            var container = builder.Build();

            var sauce1 = container.Resolve<SauceBéarnaise>();
            var sauce2 = container.Resolve<SauceBéarnaise>();

            Assert.NotSame(sauce1, sauce2);
        }

        [Fact]
        public void ImplicitlyRegisterSauceBéarnaiseAsTransient()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterType<SauceBéarnaise>();
            var container = builder.Build();

            var sauce1 = container.Resolve<SauceBéarnaise>();
            var sauce2 = container.Resolve<SauceBéarnaise>();

            Assert.NotSame(sauce1, sauce2);
        }

        [Fact]
        public void RegisterConventionAsSingleton()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(typeof(Steak).Assembly)
                .As<IIngredient>()
                .SingleInstance();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            var container = builder.Build();

            var ingredient1 = container.Resolve<IEnumerable<IIngredient>>().First();
            var ingredient2 = container.Resolve<IEnumerable<IIngredient>>().First();

            Assert.Same(ingredient1, ingredient2);
        }

        [Fact]
        public void XmlConfiguredSingletonIngredientIsCorrectlyResolved()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ConfigurationSettingsReader("autofac2"));
            var container = builder.Build();

            var sauce1 = container.Resolve<IIngredient>();
            var sauce2 = container.Resolve<IIngredient>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void ReleaseParsleyAsSingletonDoesNotDisposeParsley()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Parsley>().As<IIngredient>().SingleInstance();
            var container = builder.Build();

            Parsley parsley;
            using (var scope = container.BeginLifetimeScope())
            {
                var ingredient = scope.Resolve<IIngredient>();

                parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            }

            Assert.False(parsley.IsDisposed);
        }

        [Fact]
        public void ReleaseParsleyAsTransientDisposesParsley()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Parsley>().As<IIngredient>().InstancePerDependency();
            var container = builder.Build();

            Parsley parsley;
            using (var scope = container.BeginLifetimeScope())
            {
                var ingredient = scope.Resolve<IIngredient>();

                parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            }

            Assert.True(parsley.IsDisposed);
        }

        [Fact]
        public void DisposeContainerDisposesSingletonParsley()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Parsley>().SingleInstance();
            var container = builder.Build();

            var parsley = container.Resolve<Parsley>();
            container.Dispose();

            Assert.True(parsley.IsDisposed);
        }

        [Fact]
        public void ReleaseMeal()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Meal>().As<IMeal>();
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var meal = scope.Resolve<IMeal>();
                // Consume meal ...
            }
        }

        [Fact]
        public void ResolveServicesWithSameSingletonDependency()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Shrimp>().As<IIngredient>().SingleInstance();
            builder.RegisterType<OliveOil>().SingleInstance();
            builder.RegisterType<EggYolk>().SingleInstance();
            builder.RegisterType<Vinegar>().SingleInstance();
            builder.RegisterType<Vinaigrette>().As<IIngredient>().SingleInstance();
            builder.RegisterType<Mayonnaise>().As<IIngredient>().SingleInstance();
            builder.RegisterType<Course>();

            var c = builder.Build().Resolve<Course>();

            Assert.Same(c.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c.Ingredients.OfType<Mayonnaise>().Single().Oil);
        }

        [Fact]
        public void ResolveServicesWithSameTransientDependency()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Shrimp>().As<IIngredient>().InstancePerDependency();
            builder.RegisterType<OliveOil>().InstancePerDependency();
            builder.RegisterType<EggYolk>().InstancePerDependency();
            builder.RegisterType<Vinegar>().InstancePerDependency();
            builder.RegisterType<Vinaigrette>().As<IIngredient>().InstancePerDependency();
            builder.RegisterType<Mayonnaise>().As<IIngredient>().InstancePerDependency();
            builder.RegisterType<Course>();

            var c = builder.Build().Resolve<Course>();

            Assert.NotSame(c.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c.Ingredients.OfType<Mayonnaise>().Single().Oil);
        }

        [Fact]
        public void DeclaringInstancePerLifetimeScope()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SauceBéarnaise>()
                .As<IIngredient>()
                .InstancePerLifetimeScope();
            var container = builder.Build();

            IIngredient i1;
            IIngredient i2;
            using (var scope = container.BeginLifetimeScope())
            {
                i1 = scope.Resolve<IIngredient>();
            }
            using (var scope = container.BeginLifetimeScope())
            {
                i2 = scope.Resolve<IIngredient>();
            }

            Assert.NotSame(i1, i2);
        }

        [Fact]
        public void InstanceIsSharedWithinScope()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Steak>().InstancePerLifetimeScope();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                var steak1 = scope.Resolve<Steak>();
                var steak2 = scope.Resolve<Steak>();

                Assert.Same(steak1, steak2);
            }
        }

        [Fact]
        public void TyingInstancesToSpecificScopes()
        {
            var builder = new ContainerBuilder();

            var organic = new object();
            builder.RegisterType<Steak>().InstancePerMatchingLifetimeScope(organic);

            var container = builder.Build();
            var scope = container.BeginLifetimeScope(organic);

            var organicSteak1 = scope.Resolve<Steak>();
            var organicSteak2 = scope.Resolve<Steak>();

            Assert.Same(organicSteak1, organicSteak2);

            scope.Dispose();
        }

        [Fact]
        public void EmptyContainerResolvesAllToEmptySequence()
        {
            var builder = new ContainerBuilder();

            var ingredients = builder.Build().Resolve<IEnumerable<IIngredient>>();

            Assert.False(ingredients.Any());
        }

        [Fact]
        public void RegisterSteakAsDefaultIngredient()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SauceBéarnaise>().As<IIngredient>();
            builder.RegisterType<Steak>().As<IIngredient>();

            var ingredient = builder.Build().Resolve<IIngredient>();
            Assert.IsAssignableFrom<Steak>(ingredient);
        }

        [Fact]
        public void RegisterNamedIngredients()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Steak>()
                .Named<IIngredient>("meat");
            builder.RegisterType<SauceBéarnaise>()
                .Named<IIngredient>("sauce");
            var container = builder.Build();

            var meat = container.ResolveNamed<IIngredient>("meat");
            var sauce = container.ResolveNamed<IIngredient>("sauce");

            Assert.IsAssignableFrom<Steak>(meat);
            Assert.IsAssignableFrom<SauceBéarnaise>(sauce);
        }

        [Fact]
        public void RegisterOnlyNamedIngredientsHasNoDefaultRegistration()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Steak>().Named<IIngredient>("meat");
            builder.RegisterType<SauceBéarnaise>().Named<IIngredient>("sauce");

            Assert.Throws<ComponentNotRegisteredException>(() =>
                builder.Build().Resolve<IIngredient>());
        }

        [Fact]
        public void RegisterKeyedIngredients()
        {
            var builder = new ContainerBuilder();

            var meatKey = new object();
            builder.RegisterType<Steak>().Keyed<IIngredient>(meatKey);
            var sauceKey = new object();
            builder.RegisterType<SauceBéarnaise>().Keyed<IIngredient>(sauceKey);

            var container = builder.Build();

            var meat = container.ResolveKeyed<IIngredient>(meatKey);
            var sauce = container.ResolveKeyed<IIngredient>(sauceKey);

            Assert.IsAssignableFrom<Steak>(meat);
            Assert.IsAssignableFrom<SauceBéarnaise>(sauce);
        }

        [Fact]
        public void CreateThreeCourseMeal()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Rillettes>()
                .Named<ICourse>("entrée");
            builder.RegisterType<CordonBleu>()
                .Named<ICourse>("mainCourse");
            builder.RegisterType<MousseAuChocolat>()
                .Named<ICourse>("dessert");

            builder.RegisterType<ThreeCourseMeal>()
                .As<IMeal>()
                .WithParameter(
                    (p, c) => p.Name == "entrée",
                    (p, c) =>
                        c.ResolveNamed<ICourse>("entrée"))
                .WithParameter(
                    (p, c) => p.Name == "mainCourse",
                    (p, c) =>
                        c.ResolveNamed<ICourse>("mainCourse"))
                .WithParameter(
                    (p, c) => p.Name == "dessert",
                    (p, c) =>
                        c.ResolveNamed<ICourse>("dessert"));

            var meal = builder.Build().Resolve<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<ThreeCourseMeal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Entrée);
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.MainCourse);
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Dessert);
        }

        [Fact]
        public void CreateThreeCourseMealByImplicitParameterNameMatching()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Rillettes>().Named<ICourse>("entrée");
            builder.RegisterType<CordonBleu>().Named<ICourse>("mainCourse");
            builder.RegisterType<MousseAuChocolat>().Named<ICourse>("dessert");
            builder.RegisterType<ThreeCourseMeal>()
                .As<IMeal>()
                .WithParameter(
                    (p, c) => true,
                    (p, c) => c.ResolveNamed(p.Name, p.ParameterType));

            var meal = builder.Build().Resolve<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<ThreeCourseMeal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Entrée);
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.MainCourse);
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Dessert);
        }

        [Fact]
        public void CreateMultiCourseMealByExplicitlyPickingCourses()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Rillettes>().Named<ICourse>("entrée");
            builder.RegisterType<LobsterBisque>().Named<ICourse>("entrée1");
            builder.RegisterType<CordonBleu>().Named<ICourse>("mainCourse");
            builder.RegisterType<MousseAuChocolat>().Named<ICourse>("dessert");
            builder.RegisterType<Meal>()
                .As<IMeal>()
                .WithParameter(
                    (p, c) => true,
                    (p, c) => new[] 
                    { 
                        c.ResolveNamed<ICourse>("entrée"), 
                        c.ResolveNamed<ICourse>("mainCourse"),
                        c.ResolveNamed<ICourse>("dessert")
                    });

            var meal = builder.Build().Resolve<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void CreateMultiCourseMealFromAllCourses()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MousseAuChocolat>().As<ICourse>();
            builder.RegisterType<CordonBleu>().As<ICourse>();
            builder.RegisterType<Rillettes>().As<ICourse>();
            builder.RegisterType<Meal>().As<IMeal>();

            var meal = builder.Build().Resolve<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void ExplicitlyConfigureCotoletta()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<VealCutlet>().Named<IIngredient>("cutlet");
            builder.RegisterType<Breading>()
                .As<IIngredient>()
                .WithParameter(
                    (p, c) => p.ParameterType == typeof(IIngredient),
                    (p, c) => c.ResolveNamed<IIngredient>("cutlet"));

            var cotoletta = builder.Build().Resolve<IIngredient>();

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void RegisterCotolettaUsingDelegate()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<VealCutlet>()
                .As<IIngredient, VealCutlet>();
            builder.Register(c => new Breading(c.Resolve<VealCutlet>()))
                .As<IIngredient>();

            var cotoletta = builder.Build().Resolve<IIngredient>();

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void ImplicitlyConfigureChiliConCarneWithSpicyness()
        {
            var builder = new ContainerBuilder();
            builder.Register<Spiciness>(c => Spiciness.Medium);
            builder.RegisterType<ChiliConCarne>().As<ICourse>();

            var course = builder.Build().Resolve<ICourse>();

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Medium, chili.Spicyness);
        }

        [Fact]
        public void ResolveChiliConCarne()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ChiliConCarne>()
                .As<ICourse>()
                .WithParameter("spicyness", Spiciness.Hot);

            var course = builder.Build().Resolve<ICourse>();

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Hot, chili.Spicyness);
        }

        [Fact]
        public void ResolveStronglyTypedChiliConCarne()
        {
            var builder = new ContainerBuilder();
            builder.Register<ICourse>(c =>
                new ChiliConCarne(Spiciness.Hot));

            var course = builder.Build().Resolve<ICourse>();

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Hot, chili.Spicyness);
        }

        [Fact]
        public void ResolveJunkFood()
        {
            var builder = new ContainerBuilder();
            builder.Register(c =>
                JunkFoodFactory.Create("chicken meal"));

            var meal = builder.Build().Resolve<IMeal>();

            var junk = Assert.IsAssignableFrom<JunkFood>(meal);
            Assert.Equal("chicken meal", junk.Name);
        }

        [Fact]
        public void ResolveBasicCaesarSalad()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CaesarSalad>().As<ICourse>();

            var meal = builder.Build().Resolve<ICourse>();

            Assert.IsAssignableFrom<CaesarSalad>(meal);
        }

        [Fact]
        public void ResolveBasicCaesarSaladDoesNotFillExtra()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CaesarSalad>().As<ICourse>();

            var meal = builder.Build().Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<NullIngredient>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWhenIngredientIsRegisteredDoesNotFillExtra()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Chicken>().As<IIngredient>();
            builder.RegisterType<CaesarSalad>().As<ICourse>();

            var meal = builder.Build().Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<NullIngredient>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWithExtraChicken()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CaesarSalad>()
                .As<ICourse>()
                .PropertiesAutowired();
            builder.RegisterType<Chicken>().As<IIngredient>();

            var meal = builder.Build().Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }

        [Fact]
        public void AutoWiringDoesNothingWhenComponentIsNotAvailable()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CaesarSalad>()
                .As<ICourse>()
                .PropertiesAutowired();

            var meal = builder.Build().Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<NullIngredient>(salad.Extra);
        }

        [Fact]
        public void RegisterCaesarSaladWithExtraProperty()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Chicken>().As<IIngredient>();
            builder.RegisterType<CaesarSalad>()
                .As<ICourse>()
                .WithProperty(new ResolvedParameter(
                    (p, c) => p.Member.Name == "set_Extra",
                    (p, c) => c.Resolve<IIngredient>()));

            var meal = builder.Build().Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWithExplicitSetter()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Chicken>().As<IIngredient>();
            builder.RegisterType<CaesarSalad>()
                .As<ICourse>()
                .OnActivating(e =>
                    e.Instance.Extra = e.Context.Resolve<IIngredient>());

            var meal = builder.Build().Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }
    }
}
