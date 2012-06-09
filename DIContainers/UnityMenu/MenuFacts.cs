using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Ploeh.Samples.CacheModel;
using Ploeh.Samples.MenuModel;
using Xunit;

namespace Ploeh.Samples.Menu.Unity
{
    public class MenuFacts
    {
        [Fact]
        public void ContainerResolvesSauceBéarnaise()
        {
            var container = new UnityContainer();
            SauceBéarnaise sauce = container.Resolve<SauceBéarnaise>();

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerAutoWiresMayonnaise()
        {
            var container = new UnityContainer();
            var mayo = container.Resolve<Mayonnaise>();

            Assert.NotNull(mayo);
        }

        [Fact]
        public void ContainerResolvesIngredientToSauceBéarnaise()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, SauceBéarnaise>();
            IIngredient ingredient = container.Resolve<IIngredient>();

            Assert.IsAssignableFrom<SauceBéarnaise>(ingredient);
        }

        [Fact]
        public void ContainerThrowsWhenResolvingUnmappedIngredient()
        {
            var container = new UnityContainer();
            container.RegisterType<SauceBéarnaise>();

            Assert.Throws<ResolutionFailedException>(() =>
                container.Resolve<IIngredient>());
        }

        [Fact]
        public void ContainerResolvesBothAbstractionAndConcreteType()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, SauceBéarnaise>();
            var sauce = container.Resolve<SauceBéarnaise>();
            var ingredient = container.Resolve<IIngredient>();

            Assert.NotNull(sauce);
            Assert.NotNull(ingredient);
        }

        [Fact]
        public void RegisterMultipleTypes()
        {
            var container = new UnityContainer();
            container.RegisterType<IEnumerable<IIngredient>, IIngredient[]>();
            container.RegisterType<IIngredient, SauceBéarnaise>();
            container.RegisterType<ICourse, Course>();

            var course = container.Resolve<ICourse>();
            Assert.NotNull(course);
        }

        [Fact]
        public void RegisterMultipleImplementationsOfTheSameType()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Steak>("meat");
            container.RegisterType<IIngredient, SauceBéarnaise>("sauce");

            var ingredients = container.ResolveAll<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void ResolveAllThrowsOnUnresolvableTypes()
        {
            var container = new UnityContainer();
            container.RegisterType<ICourse, CordonBleu>("valid");
            container.RegisterType<ICourse, ChiliConCarne>("invalid");

            Assert.Throws<ResolutionFailedException>(() =>
                container.ResolveAll<ICourse>().ToList());
        }

        [Fact]
        public void RegisterAllIngredients()
        {
            var container = new UnityContainer();

            foreach (var t in typeof(Steak).Assembly.GetExportedTypes())
            {
                // This filter clutters the sample code and is removed in the book
                if (typeof(IIngredient) == t || typeof(Breading) == t)
                {
                    continue;
                }
                if (typeof(IIngredient).IsAssignableFrom(t))
                {
                    container.RegisterType(typeof(IIngredient), t, t.FullName);
                }
            }

            var ingredients = container.ResolveAll<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterBasedOnXmlConfiguration()
        {
            /* Note: To use the LoadConfiguration extension method, we must both add a reference to
             * Microsoft.Practices.Unity.Configuration and add a using directive for
             * Microsoft.Practices.Unity.Configuration. */

            var container = new UnityContainer();
            container.LoadConfiguration();

            var ingredient = container.Resolve<IIngredient>();

            Assert.NotNull(ingredient);
        }

        [Fact]
        public void XmlConfigurationWinsWhenLoadedLast()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, SauceBéarnaise>();
            container.LoadConfiguration();

            var ingredient = container.Resolve<IIngredient>();

            Assert.IsAssignableFrom<Steak>(ingredient);
        }

        [Fact]
        public void ImperativeRegistrationWinsWhenDefinedAfterConfigurationLoad()
        {
            var container = new UnityContainer();
            container.LoadConfiguration();
            container.RegisterType<IIngredient, SauceBéarnaise>();

            var ingredient = container.Resolve<IIngredient>();

            Assert.IsAssignableFrom<SauceBéarnaise>(ingredient);
        }

        [Fact]
        public void IngredientsCanBeRegisteredUsingExtension()
        {
            var container = new UnityContainer();
            container.AddNewExtension<IngredientExtension>();

            var ingredients = container.ResolveAll<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void ExtensionCanBeAddByInstance()
        {
            var container = new UnityContainer();
            container.AddExtension(new IngredientExtension());

            var ingredients = container.ResolveAll<IIngredient>();

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterSauceBéarnaiseAsSingleton()
        {
            var container = new UnityContainer();
            container.RegisterType<SauceBéarnaise>(
                new ContainerControlledLifetimeManager());

            var sauce1 = container.Resolve<SauceBéarnaise>();
            var sauce2 = container.Resolve<SauceBéarnaise>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void RegisterIngredientAsSingleton()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, SauceBéarnaise>(
                new ContainerControlledLifetimeManager());

            var i1 = container.Resolve<IIngredient>();
            var i2 = container.Resolve<IIngredient>();

            Assert.Same(i1, i2);
        }

        [Fact]
        public void RegisterIngredientAsSingletonAlsoConfiguresLifetimeForConcreteType()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, SauceBéarnaise>(new ContainerControlledLifetimeManager());

            var sauce1 = container.Resolve<SauceBéarnaise>();
            var sauce2 = container.Resolve<SauceBéarnaise>();

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void ExplicitlyRegisterSauceBéarnaiseAsTransient()
        {
            var container = new UnityContainer();
            container.RegisterType<SauceBéarnaise>(
                new TransientLifetimeManager());

            var sauce1 = container.Resolve<SauceBéarnaise>();
            var sauce2 = container.Resolve<SauceBéarnaise>();

            Assert.NotSame(sauce1, sauce2);
        }

        [Fact]
        public void ImplicitlyRegisterSauceBéarnaiseAsTransient()
        {
            var container = new UnityContainer();
            container.RegisterType<SauceBéarnaise>();

            var sauce1 = container.Resolve<SauceBéarnaise>();
            var sauce2 = container.Resolve<SauceBéarnaise>();

            Assert.NotSame(sauce1, sauce2);
        }

        [Fact]
        public void XmlConfiguredSingletonIngredientIsCorrectlyResolved()
        {
            var container = new UnityContainer();
            container.LoadConfiguration("container2");

            var i1 = container.Resolve<IIngredient>();
            var i2 = container.Resolve<IIngredient>();

            Assert.Same(i1, i2);
        }

        [Fact]
        public void TeardownParsleyAsSingletonDoesNotDisposeParsley()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Parsley>(new ContainerControlledLifetimeManager());

            var ingredient = container.Resolve<IIngredient>();

            container.Teardown(ingredient);

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.False(parsley.IsDisposed);
        }

        [Fact]
        public void TeardownParsleyAsTransientDoesNotDisposeParsley()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Parsley>(new TransientLifetimeManager());

            var ingredient = container.Resolve<IIngredient>();

            container.Teardown(ingredient);

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.False(parsley.IsDisposed); // <-- Boo hiss!!
        }

        [Fact]
        public void TeardownParsleyAsPerResolveDoesNotDisposeParsley()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Parsley>(new PerResolveLifetimeManager());

            var ingredient = container.Resolve<IIngredient>();

            container.Teardown(ingredient);

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.False(parsley.IsDisposed); // <-- Boo hiss!!
        }

        [Fact]
        public void DisposeChildContainerDoesNotDisposeSingletonParsley()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Parsley>(new ContainerControlledLifetimeManager());

            IIngredient ingredient = null;
            using (var child = container.CreateChildContainer())
            {
                ingredient = child.Resolve<IIngredient>();
            }

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.False(parsley.IsDisposed);
        }

        [Fact]
        public void DisposeChildContainerDoesNotDisposeTransientParsley()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Parsley>(new TransientLifetimeManager());

            IIngredient ingredient = null;
            using (var child = container.CreateChildContainer())
            {
                ingredient = child.Resolve<IIngredient>();
            }

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.False(parsley.IsDisposed); // <-- Boo hiss!!
        }

        [Fact]
        public void DisposeChildContainerDoesNotDisposePerResolveParsley()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Parsley>(new PerResolveLifetimeManager());

            IIngredient ingredient = null;
            using (var child = container.CreateChildContainer())
            {
                ingredient = child.Resolve<IIngredient>();
            }

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.False(parsley.IsDisposed); // <-- Boo hiss!!
        }

        [Fact]
        public void DisposeChildContainerDisposesHierachicalParsley()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Parsley>(new HierarchicalLifetimeManager());

            IIngredient ingredient = null;
            using (var child = container.CreateChildContainer())
            {
                ingredient = child.Resolve<IIngredient>();
            }

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.True(parsley.IsDisposed);
        }

        [Fact]
        public void ReleaseMeal()
        {
            var container = new UnityContainer();
            container.RegisterType<IEnumerable<ICourse>, ICourse[]>();
            container.RegisterType<IMeal, Meal>();

            using (var child = container.CreateChildContainer())
            {
                var meal = child.Resolve<IMeal>();
                // Consume meal ...
            }
        }

        [Fact]
        public void ResolveMultipleServicesWithSameSingletonDependency()
        {
            var container = new UnityContainer();
            container.RegisterType<IEnumerable<IIngredient>, IIngredient[]>();
            container.RegisterType<IIngredient, Shrimp>("shrimp", new ContainerControlledLifetimeManager());
            container.RegisterType<OliveOil>(new ContainerControlledLifetimeManager());
            container.RegisterType<EggYolk>(new ContainerControlledLifetimeManager());
            container.RegisterType<Vinegar>(new ContainerControlledLifetimeManager());
            container.RegisterType<IIngredient, Vinaigrette>("vinaigrette", new ContainerControlledLifetimeManager());
            container.RegisterType<IIngredient, Mayonnaise>("mayonnaise", new ContainerControlledLifetimeManager());
            container.RegisterType<Course>(new ContainerControlledLifetimeManager());

            var c = container.Resolve<Course>();

            Assert.Same(c.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c.Ingredients.OfType<Mayonnaise>().Single().Oil);
        }

        [Fact]
        public void ResolveServicesWithSameTransientDependency()
        {
            var container = new UnityContainer();
            container.RegisterType<IEnumerable<IIngredient>, IIngredient[]>();
            container.RegisterType<IIngredient, Shrimp>("shrimp", new TransientLifetimeManager());
            container.RegisterType<OliveOil>(new TransientLifetimeManager());
            container.RegisterType<EggYolk>(new TransientLifetimeManager());
            container.RegisterType<Vinegar>(new TransientLifetimeManager());
            container.RegisterType<IIngredient, Vinaigrette>("vinaigrette", new TransientLifetimeManager());
            container.RegisterType<IIngredient, Mayonnaise>("mayonnaise", new TransientLifetimeManager());
            container.RegisterType<Course>(new TransientLifetimeManager());

            var c1 = container.Resolve<Course>();
            var c2 = container.Resolve<Course>();

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

        [Fact(Skip = "Bug in Unity: http://unity.codeplex.com/workitem/8777")]
        public void ResolveServicesWithSamePerResolveDependency()
        {
            var container = new UnityContainer();
            container.RegisterType<IEnumerable<IIngredient>, IIngredient[]>();
            container.RegisterType<IIngredient, Shrimp>("shrimp", new PerResolveLifetimeManager());
            container.RegisterType<OliveOil>(new PerResolveLifetimeManager());
            container.RegisterType<EggYolk>(new PerResolveLifetimeManager());
            container.RegisterType<Vinegar>(new PerResolveLifetimeManager());
            container.RegisterType<IIngredient, Vinaigrette>("vinaigrette", new PerResolveLifetimeManager());
            container.RegisterType<IIngredient, Mayonnaise>("mayonnaise", new PerResolveLifetimeManager());
            container.RegisterType<Course>(new PerResolveLifetimeManager());

            var c1 = container.Resolve<Course>();
            var c2 = container.Resolve<Course>();

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
        public void DeclaringHierarchicalLifetime()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, SauceBéarnaise>(
                new HierarchicalLifetimeManager());

            IIngredient i1;
            IIngredient i2;
            using (var child = container.CreateChildContainer())
            {
                i1 = child.Resolve<IIngredient>();
            }
            using (var child = container.CreateChildContainer())
            {
                i2 = child.Resolve<IIngredient>();
            }

            Assert.NotSame(i1, i2);
        }

        [Fact]
        public void HierarchicalInstanceIsSharedWithinChildContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<Steak>(new HierarchicalLifetimeManager());

            using (var child = container.CreateChildContainer())
            {
                var steak1 = child.Resolve<Steak>();
                var steak2 = child.Resolve<Steak>();

                Assert.Same(steak1, steak2);
            }
        }

        [Fact]
        public void UsingCachingIngredientWithLongTimeout()
        {
            var container = new UnityContainer();

            var lease = new SlidingLease(TimeSpan.FromMinutes(1));
            var cache = new CacheLifetimeManager(lease);
            container.RegisterType<IIngredient, SauceBéarnaise>(cache);

            var ingredient1 = container.Resolve<IIngredient>();
            var ingredient2 = container.Resolve<IIngredient>();

            Assert.Same(ingredient1, ingredient2);
        }

        [Fact]
        public void UsingCachingIngredientWithShortTimeout()
        {
            var container = new UnityContainer();

            var lease = new SlidingLease(TimeSpan.FromTicks(1));
            var cache = new CacheLifetimeManager(lease);
            container.RegisterType<IIngredient, SauceBéarnaise>(cache);

            var ingredient1 = container.Resolve<IIngredient>();
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
            var ingredient2 = container.Resolve<IIngredient>();

            Assert.NotSame(ingredient1, ingredient2);
        }

        [Fact]
        public void TeardownParsleyAsCachedDoesNotDisposeParsley()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Parsley>(new CacheLifetimeManager(new SlidingLease(TimeSpan.FromMinutes(1))));

            var ingredient = container.Resolve<IIngredient>();

            container.Teardown(ingredient);

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.False(parsley.IsDisposed); // <-- Boo hiss!!
        }

        [Fact]
        public void TeardownParsleyAsCachedDisposesParsleyWhenCacheLifetimeStrategyExtensionIsAdded()
        {
            var container = new UnityContainer();
            container.AddNewExtension<CacheLifetimeStrategyExtension>();

            var lease = new SlidingLease(TimeSpan.FromTicks(1));
            var cache = new CacheLifetimeManager(lease);
            container.RegisterType<IIngredient, Parsley>(cache);

            var ingredient = container.Resolve<IIngredient>();

            Thread.Sleep(TimeSpan.FromMilliseconds(1));

            container.Teardown(ingredient);

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.True(parsley.IsDisposed);
        }

        [Fact]
        public void DisposeContainerDisposesCachedParsley()
        {
            var container = new UnityContainer();

            var lease = new SlidingLease(TimeSpan.FromMinutes(1));
            var cache = new CacheLifetimeManager(lease);
            container.RegisterType<IIngredient, Parsley>(cache);

            var ingredient = container.Resolve<IIngredient>();

            container.Dispose();

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.True(parsley.IsDisposed);
        }

        [Fact]
        public void DisposeChildContainerDoesNotDisposeCachedParsley()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Parsley>(new CacheLifetimeManager(new SlidingLease(TimeSpan.FromMinutes(1))));

            IIngredient ingredient;
            using (var child = container.CreateChildContainer())
            {
                ingredient = child.Resolve<IIngredient>();
            }

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.False(parsley.IsDisposed); // <-- Boo hiss!!
        }

        [Fact]
        public void DisposeChildContainerDisposesCachedParsleyWhenCacheLifetimeStrategyExtensionIsAdded()
        {
            var container = new UnityContainer();
            container.AddNewExtension<CacheLifetimeStrategyExtension>();
            container.RegisterType<IIngredient, Parsley>(new CacheLifetimeManager(new SlidingLease(TimeSpan.FromMinutes(1))));

            IIngredient ingredient = null;
            using (var child = container.CreateChildContainer())
            {
                ingredient = child.Resolve<IIngredient>();
            }

            var parsley = Assert.IsAssignableFrom<Parsley>(ingredient);
            Assert.True(parsley.IsDisposed);
        }

        [Fact]
        public void EmptyContainerResolvesAllToEmptySequence()
        {
            var container = new UnityContainer();

            var ingredients = container.ResolveAll<IIngredient>();

            Assert.Empty(ingredients);
        }

        [Fact]
        public void RegisterSteakAsDefaultIngredient()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Steak>();
            container.RegisterType<IIngredient, SauceBéarnaise>("sauce");

            var ingredient = container.Resolve<IIngredient>();
            Assert.IsAssignableFrom<Steak>(ingredient);
        }

        [Fact]
        public void RegisterTwoDefaultIngredientsOverwritesTheFirst()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Steak>();
            container.RegisterType<IIngredient, SauceBéarnaise>();

            var ingredient = container.Resolve<IIngredient>();
            Assert.IsAssignableFrom<SauceBéarnaise>(ingredient);
        }

        [Fact]
        public void RegisterTwoDefaultIngredientsRemovesTheFirst()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Steak>();
            container.RegisterType<IIngredient, SauceBéarnaise>();

            IEnumerable<IIngredient> ingredients =
                container.ResolveAll<IIngredient>();

            Assert.False(ingredients.OfType<Steak>().Any());
        }

        [Fact]
        public void ResolveAllResolvesNamedComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Steak>();
            container.RegisterType<IIngredient, SauceBéarnaise>("sauce");
            container.RegisterType<IIngredient, Chicken>("meat");

            var ingredients = container.ResolveAll<IIngredient>();

            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
            Assert.True(ingredients.OfType<Chicken>().Any());
        }

        [Fact]
        public void ResolveAllDoesNotResultDefaultComponent()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Steak>();
            container.RegisterType<IIngredient, SauceBéarnaise>("sauce");
            container.RegisterType<IIngredient, Chicken>("meat");

            var ingredients = container.ResolveAll<IIngredient>();

            Assert.False(ingredients.OfType<Steak>().Any());
        }

        [Fact]
        public void RegisterNamedIngredients()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Steak>("meat");
            container.RegisterType<IIngredient, SauceBéarnaise>("sauce");

            var meat = container.Resolve<IIngredient>("meat");
            var sauce = container.Resolve<IIngredient>("sauce");

            Assert.IsAssignableFrom<Steak>(meat);
            Assert.IsAssignableFrom<SauceBéarnaise>(sauce);
        }

        [Fact]
        public void CreateThreeCourseMeal()
        {
            var container = new UnityContainer();
            container.RegisterType<ICourse, Rillettes>("entrée");
            container.RegisterType<ICourse, CordonBleu>("mainCourse");
            container.RegisterType<ICourse, MousseAuChocolat>("dessert");

            container.RegisterType<IMeal, ThreeCourseMeal>(
                new InjectionConstructor(
                    new ResolvedParameter<ICourse>("entrée"),
                    new ResolvedParameter<ICourse>("mainCourse"),
                    new ResolvedParameter<ICourse>("dessert")));

            var meal = container.Resolve<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<ThreeCourseMeal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Entrée);
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.MainCourse);
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Dessert);
        }

        [Fact]
        public void CreateMultiCourseMealByExplicitlyPickingCourses()
        {
            var container = new UnityContainer();
            container.RegisterType<ICourse, Rillettes>("entrée");
            container.RegisterType<ICourse, LobsterBisque>("entrée1");
            container.RegisterType<ICourse, CordonBleu>("mainCourse");
            container.RegisterType<ICourse, MousseAuChocolat>("dessert");

            container.RegisterType<IMeal, Meal>(
                new InjectionConstructor(
                    new ResolvedArrayParameter<ICourse>(
                        new ResolvedParameter<ICourse>("entrée"),
                        new ResolvedParameter<ICourse>("mainCourse"),
                        new ResolvedParameter<ICourse>("dessert"))));

            var meal = container.Resolve<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void CreateMultiCourseMealFromAllCourses()
        {
            var container = new UnityContainer();
            container.RegisterType<ICourse, Rillettes>("entrée");
            container.RegisterType<ICourse, CordonBleu>("mainCourse");
            container.RegisterType<ICourse, MousseAuChocolat>("dessert");
            container.RegisterType<IEnumerable<ICourse>, ICourse[]>();
            container.RegisterType<IMeal, Meal>();

            var meal = container.Resolve<IMeal>();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void ResolveMealWhenEnumerableIsNotMappedThrows()
        {
            var container = new UnityContainer();
            container.RegisterType<ICourse, Rillettes>("entrée");
            container.RegisterType<ICourse, CordonBleu>("mainCourse");
            container.RegisterType<ICourse, MousseAuChocolat>("dessert");
            container.RegisterType<IMeal, Meal>();

            Assert.Throws<ResolutionFailedException>(() =>
                container.Resolve<IMeal>());
        }

        [Fact]
        public void ResolveCotolettaFromNamedComponent()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, VealCutlet>("cutlet");
            container.RegisterType<IIngredient, Breading>(
                new InjectionConstructor(
                    new ResolvedParameter<IIngredient>("cutlet")));

            var cotoletta = container.Resolve<IIngredient>();

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void ResolveCotolettaFromImplicitConcreteType()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Breading>(
                new InjectionConstructor(
                    new ResolvedParameter<VealCutlet>()));

            var cotoletta = container.Resolve<IIngredient>();

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void ResolveCotolettaFromConfiguredConcreteType()
        {
            var container = new UnityContainer();
            container.RegisterType<VealCutlet>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<IIngredient, Breading>(
                new InjectionConstructor(
                    new ResolvedParameter<VealCutlet>()));

            var cotoletta1 = container.Resolve<IIngredient>();
            var cotoletta2 = container.Resolve<IIngredient>();

            var breading1 = Assert.IsAssignableFrom<Breading>(cotoletta1);
            Assert.IsAssignableFrom<VealCutlet>(breading1.Ingredient);

            var breading2 = Assert.IsAssignableFrom<Breading>(cotoletta2);
            Assert.IsAssignableFrom<VealCutlet>(breading1.Ingredient);

            Assert.Equal(breading1.Ingredient, breading2.Ingredient);
        }

        [Fact]
        public void ImplicitlyConfigureSpicynessForChiliConCarne()
        {
            var container = new UnityContainer();
            container.RegisterInstance(Spiciness.Medium);
            container.RegisterType<ICourse, ChiliConCarne>();

            var course = container.Resolve<ICourse>();

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Medium, chili.Spicyness);
        }

        [Fact]
        public void ExplicitlyConfigureSpicynessForChiliConCarne()
        {
            var container = new UnityContainer();
            container.RegisterType<ICourse, ChiliConCarne>(
                new InjectionConstructor(Spiciness.Hot));

            var course = container.Resolve<ICourse>();

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Hot, chili.Spicyness);
        }

        [Fact]
        public void ConfigureSpicynessForChiliConCarneUsingCodeBlock()
        {
            var container = new UnityContainer();
            container.RegisterType<ICourse, ChiliConCarne>(
                new InjectionFactory(
                    c => new ChiliConCarne(Spiciness.Hot)));

            var course = container.Resolve<ICourse>();

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Hot, chili.Spicyness);
        }

        [Fact]
        public void ResolveJunkFood()
        {
            var container = new UnityContainer();
            container.RegisterType<IMeal, JunkFood>(
                new InjectionFactory(
                    c => JunkFoodFactory.Create("chicken meal")));

            var meal = container.Resolve<IMeal>();

            var junk = Assert.IsAssignableFrom<JunkFood>(meal);
            Assert.Equal("chicken meal", junk.Name);
        }

        [Fact]
        public void ResolveBasicCaesarSalad()
        {
            var container = new UnityContainer();
            container.RegisterType<ICourse, CaesarSalad>();

            var meal = container.Resolve<ICourse>();

            Assert.IsAssignableFrom<CaesarSalad>(meal);
        }

        [Fact]
        public void ResolveBasicCaesarSaladDoesNotFillExtra()
        {
            var container = new UnityContainer();
            container.RegisterType<ICourse, CaesarSalad>();

            var meal = container.Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<NullIngredient>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWhenIngredientIsRegisteredDoesNotFillExtra()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Chicken>();
            container.RegisterType<ICourse, CaesarSalad>();

            var meal = container.Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<NullIngredient>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWithExtraChicken()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Chicken>();
            container.RegisterType<ICourse, CaesarSalad>(
                new InjectionProperty("Extra"));

            var meal = container.Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWithSpecificallySelectedExtra()
        {
            var container = new UnityContainer();
            container.RegisterType<IIngredient, Chicken>("chicken");
            container.RegisterType<IIngredient, Steak>("steak");
            container.RegisterType<ICourse, CaesarSalad>(
                new InjectionProperty("Extra",
                    new ResolvedParameter<IIngredient>("chicken")));

            var meal = container.Resolve<ICourse>();

            var salad = Assert.IsAssignableFrom<CaesarSalad>(meal);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }

        [Fact]
        public void InjectionPropertyThrowsWhenComponentIsNotAvailable()
        {
            var container = new UnityContainer();
            container.RegisterType<ICourse, CaesarSalad>(new InjectionProperty("Extra"));

            Assert.Throws<ResolutionFailedException>(() =>
                container.Resolve<ICourse>());
        }
    }
}
