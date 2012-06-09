using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Spring.Context.Support;
using Ploeh.Samples.MenuModel;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Xml;
using System.IO;
using Spring.Core.IO;
using System.Xml.Linq;
using System.Xml;
using Spring.Objects.Factory.Support;
using Spring.Objects.Factory.Config;

namespace Ploeh.Samples.Menu.SpringNet
{
    public class MenuFacts
    {
        [Fact]
        public void ContainerResolvesSauceBéarnaise()
        {
            var context = new XmlApplicationContext("sauce.xml");
            SauceBéarnaise sauce = (SauceBéarnaise)context.GetObject("Sauce");

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerResolvesSauceBéarnaiseThroughType()
        {
            var context = new XmlApplicationContext("sauce.xml");
            SauceBéarnaise sauce = context.GetObjectsOfType(typeof(SauceBéarnaise)).Values.OfType<SauceBéarnaise>().Single();

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerResolvesSauceBéarnaiseThroughExtensionMethod()
        {
            var context = new XmlApplicationContext("sauce.xml");
            SauceBéarnaise sauce = context.Resolve<SauceBéarnaise>();

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerResolvesIngredientThroughExtensionMethod()
        {
            var context = new XmlApplicationContext("sauce.xml");
            IIngredient ingredient = context.Resolve<IIngredient>();

            Assert.IsAssignableFrom<SauceBéarnaise>(ingredient);
        }

        [Fact]
        public void ContainerLoadsXmlFromFileMoniker()
        {
            var context = new XmlApplicationContext("file://sauce.xml");
            SauceBéarnaise sauce =
                (SauceBéarnaise)context.GetObject("Sauce");

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerLoadsXmlFromConfig()
        {
            var context = new XmlApplicationContext("config://spring/objects");
            SauceBéarnaise sauce =
                (SauceBéarnaise)context.GetObject("Sauce");

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerLoadsXmlFromEmbeddedResource()
        {
            var context = new XmlApplicationContext("assembly://Ploeh.Samples.Menu.SpringNet/Ploeh.Samples.Menu.SpringNet/sauce.xml");
            SauceBéarnaise sauce =
                (SauceBéarnaise)context.GetObject("Sauce");

            Assert.NotNull(sauce);
        }

        [Fact(Skip = "This is not really a unit test, but rather an integration test. To work, the sauce.xml file must be available via anonymous HTTP at the specified address.")]
        public void ContainerLoadsXmlFromHttp()
        {
            var context = new XmlApplicationContext("http://localhost/sauce.xml");
            SauceBéarnaise sauce =
                (SauceBéarnaise)context.GetObject("Sauce");

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerLoadsXmlFromRawStream()
        {
            var xml = "<objects xmlns=\"http://www.springframework.net\"><object id=\"Sauce\" type=\"SauceBéarnaise\" /></objects>";
            using (var stream = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream))
                {
                    sw.Write(xml);
                    sw.Flush();
                    stream.Position = 0;
                    var resource = new InputStreamResource(stream, "");
                    var context = new XmlObjectFactory(resource);

                    var sauce = context.GetObject("Sauce");
                    Assert.IsAssignableFrom<SauceBéarnaise>(sauce);
                }
            }
        }

        [Fact]
        public void ConfigureContainerWithSauceUsingLinqToXml()
        {
            XNamespace xmlns = @"http://www.springframework.net";
            var xml =
                new XElement(xmlns + "objects",
                    new XElement(xmlns + "object", new XAttribute("id", "Sauce"), new XAttribute("type", "SauceBéarnaise")));
            using (var stream = new MemoryStream())
            {
                using (var xw = XmlWriter.Create(stream))
                {
                    xml.Save(xw);
                    xw.Flush();
                    stream.Position = 0;
                }
                var resource = new InputStreamResource(stream, "");
                var context = new XmlObjectFactory(resource);

                var sauce = context.GetObject("Sauce");
                Assert.IsAssignableFrom<SauceBéarnaise>(sauce);
            }
        }

        [Fact]
        public void ConfigureContainerUsingApi()
        {
            var context = new GenericApplicationContext();
            context.RegisterObjectDefinition("Sauce",
                new RootObjectDefinition(typeof(SauceBéarnaise)));
            var sauce = context.GetObject("Sauce");
            Assert.IsAssignableFrom<SauceBéarnaise>(sauce);
        }

        [Fact]
        public void LoadMultipleConfigurationsToCreateCourse()
        {
            var context = new XmlApplicationContext(
                "config://spring/objects",
                "meat.xml",
                "file://course.xml");
            var course = (Course)context.GetObject("Course");

            Assert.True(course.Ingredients.OfType<SauceBéarnaise>().Any());
            Assert.True(course.Ingredients.OfType<Steak>().Any());
        }

        [Fact]
        public void ImportMultipleConfigurationsToCreateCourse()
        {
            var context = new XmlApplicationContext("ImportMultipleConfigurationsToCreateCourse.xml");

            var course = (Course)context.GetObject("Course");

            Assert.True(course.Ingredients.OfType<SauceBéarnaise>().Any());
            Assert.True(course.Ingredients.OfType<Steak>().Any());
        }

        [Fact]
        public void ContainerResolvesSauceBéarnaiseFromTypeAlias()
        {
            var context = new XmlApplicationContext("SauceWithTypeAlias.xml");
            SauceBéarnaise sauce =
                (SauceBéarnaise)context.GetObject("Sauce");

            Assert.NotNull(sauce);
        }

        [Fact]
        public void ContainerDoesNotResolveMayonnaiseWhenWiringIsNotDefined()
        {
            Assert.Throws<ObjectCreationException>(() =>
                new XmlApplicationContext("ContainerDoesNotResolveMayonnaiseWhenWiringIsNotDefined.xml"));
        }

        [Fact]
        public void ContainerResolvesExplicitlyWiredMayonnaise()
        {
            var context = new XmlApplicationContext("ContainerResolvesExplicitlyWiredMayonnaise.xml");
            var mayo = context.GetObject("Mayonnaise");

            Assert.IsAssignableFrom<Mayonnaise>(mayo);
        }

        [Fact]
        public void ContainerResolvesAutoWiredMayonnaise()
        {
            var context = new XmlApplicationContext("ContainerResolvesAutoWiredMayonnaise.xml");
            var mayo = context.GetObject("Mayonnaise");

            Assert.IsAssignableFrom<Mayonnaise>(mayo);
        }

        [Fact]
        public void ContainerResolveMayonnaiseWithAutoWiringIsEnabledByDefault()
        {
            var context = new XmlApplicationContext("ContainerResolveMayonnaiseWithAutoWiringIsEnabledByDefault.xml");
            var mayo = context.GetObject("Mayonnaise");

            Assert.IsAssignableFrom<Mayonnaise>(mayo);
        }

        [Fact]
        public void ContainerCannontAutoWireMayonnaiseWhenMultipleOliveOilsAreConfigured()
        {
            Assert.Throws<UnsatisfiedDependencyException>(() =>
                new XmlApplicationContext("ContainerCannontAutoWireMayonnaiseWhenMultipleOliveOilsAreConfigured.xml"));
        }

        [Fact]
        public void ContainerCanComposeMayonnaiseWhenAutoWiringIsOverriden()
        {
            var context = new XmlApplicationContext("ContainerCanComposeMayonnaiseWhenAutoWiringIsOverriden.xml");
            var mayo = context.GetObject("Mayonnaise");

            Assert.IsAssignableFrom<Mayonnaise>(mayo);
        }

        [Fact]
        public void ContainerResolvesMayonnaiseConfiguredByApi()
        {
            var context = new GenericApplicationContext();
            context.RegisterObjectDefinition("EggYolk",
                new RootObjectDefinition(typeof(EggYolk)));
            context.RegisterObjectDefinition("OliveOil",
                new RootObjectDefinition(typeof(OliveOil)));
            context.RegisterObjectDefinition("Mayonnaise",
                new RootObjectDefinition(typeof(Mayonnaise),
                    AutoWiringMode.AutoDetect));
            var mayo = context.GetObject("Mayonnaise");

            Assert.IsAssignableFrom<Mayonnaise>(mayo);
        }

        [Fact]
        public void RegisterMultipleTypes()
        {
            var context = new XmlApplicationContext("RegisterMultipleTypes.xml");
            var course = (ICourse)context.GetObject("Course");

            Assert.NotNull(course);
        }

        [Fact]
        public void RegisterMultipleImplementationsOfTheSameType()
        {
            var context = new XmlApplicationContext("RegisterMultipleImplementationsOfTheSameType.xml");
            var ingredients = (IEnumerable<IIngredient>)context.GetObject("Ingredients");

            Assert.True(ingredients.OfType<Steak>().Any());
            Assert.True(ingredients.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void RegisterSauceBéarnaiseAsTransient()
        {
            var context = new XmlApplicationContext("RegisterSauceBéarnaiseAsTransient.xml");
            var sauce1 = (SauceBéarnaise)context.GetObject("Sauce");
            var sauce2 = (SauceBéarnaise)context.GetObject("Sauce");

            Assert.NotSame(sauce1, sauce2);
        }

        [Fact]
        public void ExplicitlyRegisterSauceBéarnaiseAsSingleton()
        {
            var context = new XmlApplicationContext("ExplicitlyRegisterSauceBéarnaiseAsSingleton.xml");
            var sauce1 = (SauceBéarnaise)context.GetObject("Sauce");
            var sauce2 = (SauceBéarnaise)context.GetObject("Sauce");

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void ImplicitlyRegisterSauceBéarnaiseAsSingleton()
        {
            var context = new XmlApplicationContext("ImplicitlyRegisterSauceBéarnaiseAsSingleton.xml");
            var sauce1 = (SauceBéarnaise)context.GetObject("Sauce");
            var sauce2 = (SauceBéarnaise)context.GetObject("Sauce");

            Assert.Same(sauce1, sauce2);
        }

        [Fact]
        public void ResolveServicesWithSameSingletonDependency()
        {
            var context = new XmlApplicationContext("ResolveServicesWithSameSingletonDependency.xml");
            var c = (Course)context.GetObject("Course");

            Assert.Same(c.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c.Ingredients.OfType<Mayonnaise>().Single().Oil);
        }

        [Fact]
        public void ResolveServicesWithSameTransientDependency()
        {
            var context = new XmlApplicationContext("ResolveServicesWithSameTransientDependency.xml");
            var c = (Course)context.GetObject("Course");

            Assert.NotSame(c.Ingredients.OfType<Vinaigrette>().Single().Oil,
                c.Ingredients.OfType<Mayonnaise>().Single().Oil);
        }

        [Fact]
        public void ConfiguringTwoObjectsWithTheSameNameThrows()
        {
            Assert.Throws<ObjectDefinitionStoreException>(() =>
                new XmlApplicationContext("ConfiguringTwoObjectsWithTheSameNameThrows.xml"));
        }

        [Fact]
        public void RegisterNamedIngredients()
        {
            var context = new XmlApplicationContext("RegisterNamedIngredients.xml");
            var meat = context.GetObject("Meat");
            var sauce = context.GetObject("Sauce");

            Assert.IsAssignableFrom<Steak>(meat);
            Assert.IsAssignableFrom<SauceBéarnaise>(sauce);
        }

        [Fact]
        public void RegisterUnnamedIngredientsAndResolveByConcreteType()
        {
            var context = new XmlApplicationContext("RegisterUnnamedIngredientsAndResolveByConcreteType.xml");
            var meat = context.GetObjectsOfType(typeof(Steak))
                .Values
                .OfType<Steak>()
                .FirstOrDefault();
            var sauce = context.GetObjectsOfType(typeof(SauceBéarnaise)).Values.OfType<SauceBéarnaise>().FirstOrDefault();

            Assert.NotNull(meat);
            Assert.NotNull(sauce);
        }

        [Fact]
        public void RegisterUnnamedIngredientsAndResolveByInterface()
        {
            var context = new XmlApplicationContext("RegisterUnnamedIngredientsAndResolveByInterface.xml");
            var ingredients = context.GetObjectsOfType(typeof(IIngredient));

            Assert.True(ingredients.Values.OfType<Steak>().Any());
            Assert.True(ingredients.Values.OfType<SauceBéarnaise>().Any());
        }

        [Fact]
        public void CreateThreeCourseMeal()
        {
            var context = new XmlApplicationContext("CreateThreeCourseMeal.xml");
            var meal = context.GetObject("Meal");

            var threeCourseMeal = Assert.IsAssignableFrom<ThreeCourseMeal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Entrée);
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.MainCourse);
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Dessert);
        }

        [Fact]
        public void CreateMultiCourseMealFromArrayImportingMeal()
        {
            var context = new XmlApplicationContext("CreateMultiCourseMealFromArrayImportingMeal.xml");
            var meal = context.GetObjectsOfType(typeof(IMeal)).Values.OfType<IMeal>().Single();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void CreateMultiCourseMealFromAllCourses()
        {
            var context = new XmlApplicationContext("CreateMultiCourseMealFromAllCourses.xml");
            var meal = context.GetObjectsOfType(typeof(IMeal)).Values.OfType<IMeal>().Single();

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void CreateMultiCourseMealByExplicitlyPickingCourses()
        {
            var context = new XmlApplicationContext("CreateMultiCourseMealByExplicitlyPickingCourses.xml");
            var meal = context.GetObject("Meal");

            var threeCourseMeal = Assert.IsAssignableFrom<Meal>(meal);
            Assert.IsAssignableFrom<Rillettes>(threeCourseMeal.Courses.First());
            Assert.IsAssignableFrom<CordonBleu>(threeCourseMeal.Courses.ElementAt(1));
            Assert.IsAssignableFrom<MousseAuChocolat>(threeCourseMeal.Courses.ElementAt(2));
        }

        [Fact]
        public void ExplicitlyConfigureCotoletta()
        {
            var context = new XmlApplicationContext("ExplicitlyConfigureCotoletta.xml");
            var cotoletta = context.GetObject("Breading");

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void InlineConfigureCotoletta()
        {
            var context = new XmlApplicationContext("InlineConfigureCotoletta.xml");
            var cotoletta = context.GetObject("Breading");

            var breading = Assert.IsAssignableFrom<Breading>(cotoletta);
            Assert.IsAssignableFrom<VealCutlet>(breading.Ingredient);
        }

        [Fact]
        public void InterceptTwoTargetsWithTheSameInterceptor()
        {
            var context = new XmlApplicationContext("InterceptTwoTargetsWithTheSameInterceptor.xml");

            context.Resolve<IFoo>().DoFoo();
            context.Resolve<IBar>().DoBar();

            var spy = context.Resolve<SpyInterceptor>();
            Assert.True(spy.Invocations.Select(i => i.Method.Name).Contains("DoFoo"));
            Assert.True(spy.Invocations.Select(i => i.Method.Name).Contains("DoBar"));
        }

        [Fact]
        public void ResolveChiliConCarne()
        {
            var context = new XmlApplicationContext("ResolveChiliConCarne.xml");
            var course = context.GetObject("Course");

            var chili = Assert.IsAssignableFrom<ChiliConCarne>(course);
            Assert.Equal(Spiciness.Hot, chili.Spicyness);
        }

        [Fact]
        public void ResolveJunkFood()
        {
            var context = new XmlApplicationContext("ResolveJunkFood.xml");
            var meal = context.GetObject("Meal");

            var junk = Assert.IsAssignableFrom<JunkFood>(meal);
            Assert.Equal("chicken meal", junk.Name);
        }

        [Fact]
        public void ResolveUnnamedJunkFood()
        {
            var context = new XmlApplicationContext("ResolveUnnamedJunkFood.xml");
            var meal = context.GetObjectsOfType(typeof(JunkFood)).Values.OfType<JunkFood>().FirstOrDefault();

            Assert.NotNull(meal);
        }

        [Fact]
        public void ResolveBasicCaesarSaladDoesNotFillExtra()
        {
            var context = new XmlApplicationContext("ResolveBasicCaesarSaladDoesNotFillExtra.xml");
            var course = context.GetObject("Course");

            var salad = Assert.IsAssignableFrom<CaesarSalad>(course);
            Assert.IsAssignableFrom<NullIngredient>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWithExtraChicken()
        {
            var context = new XmlApplicationContext("ResolveCaesarSaladWithExtraChicken.xml");
            var course = context.GetObject("Course");

            var salad = Assert.IsAssignableFrom<CaesarSalad>(course);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }

        [Fact]
        public void ResolveCaesarSaladWithInlinedExtraChicken()
        {
            var context = new XmlApplicationContext("ResolveCaesarSaladWithInlinedExtraChicken.xml");
            var course = context.GetObject("Course");

            var salad = Assert.IsAssignableFrom<CaesarSalad>(course);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }

        [Fact]
        public void ResolveAutowiredCaesarSaladWhenNoIngredientIsAvailable()
        {
            var context = new XmlApplicationContext("ResolveAutowiredCaesarSaladWhenNoIngredientIsAvailable.xml");
            var course = context.GetObject("Course");

            var salad = Assert.IsAssignableFrom<CaesarSalad>(course);
            Assert.IsAssignableFrom<NullIngredient>(salad.Extra);
        }

        [Fact]
        public void ResolveAutowiredCaesarSaladWhenIngredientIsAvailable()
        {
            var context = new XmlApplicationContext("ResolveAutowiredCaesarSaladWhenIngredientIsAvailable.xml");
            var course = context.GetObject("Course");

            var salad = Assert.IsAssignableFrom<CaesarSalad>(course);
            Assert.IsAssignableFrom<Chicken>(salad.Extra);
        }
    }
}
