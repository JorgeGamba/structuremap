using System;
using NUnit.Framework;
using StructureMap.Pipeline;
using StructureMap.Testing.GenericWidgets;
using StructureMap.Testing.Widget;

namespace StructureMap.Testing.Pipeline
{
    [TestFixture]
    public class MissingInstanceTester
    {
        [Test]
        public void configure_and_use_missing_instance()
        {
            // If a user should happen to ask for a Rule by name
            // that does not exist, StructureMap will use an Instance
            // that builds a "ColorRule" object using the 
            // IContext.RequestedName property
            var container = new Container(x =>
            {
                x.ForRequestedType<Rule>().MissingNamedInstanceIs
                    .ConstructedBy(context => new ColorRule(context.RequestedName));
            });

            container.GetInstance<Rule>("red").ShouldBeOfType<ColorRule>().Color.ShouldEqual("red");
            container.GetInstance<Rule>("green").ShouldBeOfType<ColorRule>().Color.ShouldEqual("green");
            container.GetInstance<Rule>("blue").ShouldBeOfType<ColorRule>().Color.ShouldEqual("blue");
        }

        [Test]
        public void configure_and_use_missing_instance_for_open_generic_types()
        {
            // If a user should happen to ask for a IThing<,>
            // (open generic type) that does not exist,
            // StructureMap will use an Instance that builds a
            // "ColorThing<object, object>" object using the 
            // IContext.RequestedName property
            var container = new Container(x =>
            {
                x.For(typeof(IThing<,>)).MissingNamedInstanceIs
                    .ConstructedBy(context => new ColorThing<object, object>(context.RequestedName));
            });

            container.GetInstance<IThing<object, object>>("red").ShouldBeOfType<ColorThing<object, object>>().Color.ShouldEqual("red");
            container.GetInstance<IThing<object, object>>("green").ShouldBeOfType<ColorThing<object, object>>().Color.ShouldEqual("green");
            container.GetInstance<IThing<object, object>>("blue").ShouldBeOfType<ColorThing<object, object>>().Color.ShouldEqual("blue");
        }
        
        [Test]
        public void returns_missing_instance_if_it_exists_and_the_requested_instance_is_not_found()
        {
            var factory = new InstanceFactory(typeof (IWidget));
            var missing = new ObjectInstance(new AWidget());


            factory.MissingInstance = missing;

            factory.FindInstance("anything").ShouldBeTheSameAs(missing);
            factory.FindInstance(Guid.NewGuid().ToString()).ShouldBeTheSameAs(missing);
        }
    }
}