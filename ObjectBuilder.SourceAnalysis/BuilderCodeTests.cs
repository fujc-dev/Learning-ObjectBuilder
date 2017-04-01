using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectBuilder.SourceAnalysis
{
    class BuilderCodeTests
    {

        public static void CanCreateInstances()
        {
            Builder builder = new Builder();
            Locator locator = CreateLocator();

            ConstructorPolicy policy = new ConstructorPolicy();
            policy.AddParameter(new ValueParameter<int>(12));
            ITypeMappingPolicy _ = new TypeMappingPolicy(typeof(BuilderCodeTests), null);
            

            builder.Policies.Set<ICreationPolicy>(policy, typeof(SimpleObject), null);  //
            //builder.Policies.Set<ITypeMappingPolicy>(_, typeof(SimpleObject), null);
            builder.Policies.Set<ITypeMappingPolicy>(new TypeMappingPolicy(typeof(SimpleObject), null), typeof(SimpleObject), null);

            SimpleObject __ = new SimpleObject(100);
            SimpleObject m1 = builder.BuildUp<SimpleObject>(locator, null, null);
            SimpleObject m5 = builder.BuildUp<SimpleObject>(locator, null, null);
            //SimpleObject m2 = builder.BuildUp<SimpleObject>(locator, null, __);
            //SimpleObject m3 = builder.BuildUp<SimpleObject>(locator, null, "123");

            //Console.WriteLine(m2.IntParam);
            //Assert.IsNotNull(m1);
            //Assert.IsNotNull(m2);
            //Assert.AreEqual(12, m1.IntParam);
            //Assert.AreEqual(12, m2.IntParam);
            //Assert.IsTrue(m1 != m2);
        }

        public static void CanCreateSingleton()
        {
            Builder builder = new Builder();
            Locator locator = CreateLocator();

            ConstructorPolicy policy = new ConstructorPolicy();
            policy.AddParameter(new ValueParameter<int>(12));
            builder.Policies.Set<ICreationPolicy>(policy, typeof(SimpleObject), null);
            builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof(SimpleObject), null);

            SimpleObject m1 = builder.BuildUp<SimpleObject>(locator, null, null);
            SimpleObject m2 = builder.BuildUp<SimpleObject>(locator, null, null);

            //Assert.AreSame(m1, m2);
        }

        public static void CreateComplexObject()
        {
            Builder builder = new Builder();
            Locator locator = CreateLocator();

            ConstructorPolicy policy = new ConstructorPolicy();
            policy.AddParameter(new ValueParameter<int>(12));
            builder.Policies.Set<ICreationPolicy>(policy, typeof(SimpleObject), null);

            ConstructorPolicy policy2 = new ConstructorPolicy();
            policy2.AddParameter(new CreationParameter(typeof(SimpleObject)));
            builder.Policies.Set<ICreationPolicy>(policy2, typeof(ComplexObject), null);

            builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof(SimpleObject), null);
            builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof(ComplexObject), null);

            ComplexObject cm = builder.BuildUp<ComplexObject>(locator, null, null);
            SimpleObject m = builder.BuildUp<SimpleObject>(locator, null, null);

            //Assert.AreSame(m, cm.SimpleObject);
            //Assert.IsNotNull(cm);
            //Assert.IsNotNull(cm.SimpleObject);
            //Assert.AreEqual(12, cm.SimpleObject.IntParam);
        }

        public static void CanCreateNamedInstance()
        {
            Builder builder = new Builder();
            Locator locator = CreateLocator();

            ConstructorPolicy policy1 = new ConstructorPolicy();
            policy1.AddParameter(new ValueParameter<int>(12));
            builder.Policies.Set<ICreationPolicy>(policy1, typeof(SimpleObject), "Object1");

            ConstructorPolicy policy2 = new ConstructorPolicy();
            policy2.AddParameter(new ValueParameter<int>(32));
            builder.Policies.Set<ICreationPolicy>(policy2, typeof(SimpleObject), "Object2");

            SimpleObject m1 = builder.BuildUp<SimpleObject>(locator, "Object1", null);
            SimpleObject m2 = builder.BuildUp<SimpleObject>(locator, "Object2", null);

            //Assert.IsNotNull(m1);
            //Assert.IsNotNull(m2);
            //Assert.AreEqual(12, m1.IntParam);
            //Assert.AreEqual(32, m2.IntParam);
            //Assert.IsTrue(m1 != m2);
        }

        public static void RefParamsCanAskForSpecificallyNamedObjects()
        {
            Builder builder = new Builder();
            Locator locator = CreateLocator();

            ConstructorPolicy policy1 = new ConstructorPolicy();
            policy1.AddParameter(new ValueParameter<int>(12));
            builder.Policies.Set<ICreationPolicy>(policy1, typeof(SimpleObject), "Object1");
            builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof(SimpleObject), "Object1");

            ConstructorPolicy policy2 = new ConstructorPolicy();
            policy2.AddParameter(new ValueParameter<int>(32));
            builder.Policies.Set<ICreationPolicy>(policy2, typeof(SimpleObject), "Object2");
            builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof(SimpleObject), "Object2");

            ConstructorPolicy policy3 = new ConstructorPolicy();
            policy3.AddParameter(new CreationParameter(typeof(SimpleObject), "Object2"));
            builder.Policies.Set<ICreationPolicy>(policy3, typeof(ComplexObject), null);
            builder.Policies.Set<ISingletonPolicy>(new SingletonPolicy(true), typeof(ComplexObject), null);

            ComplexObject cm = builder.BuildUp<ComplexObject>(locator, null, null);
            SimpleObject sm = builder.BuildUp<SimpleObject>(locator, "Object2", null);

            //Assert.IsNotNull(cm);
            //Assert.IsNotNull(cm.SimpleObject);
            //Assert.AreEqual(32, cm.SimpleObject.IntParam);
            //Assert.AreSame(sm, cm.SimpleObject);
        }

        public static void CanInjectValuesIntoProperties()
        {
            Builder builder = new Builder();
            Locator locator = CreateLocator();

            PropertySetterPolicy policy = new PropertySetterPolicy();
            policy.Properties.Add("StringProperty", new PropertySetterInfo("StringProperty", new ValueParameter<string>("Bar is here")));
            builder.Policies.Set<IPropertySetterPolicy>(policy, typeof(SimpleObject), null);

            SimpleObject sm = builder.BuildUp<SimpleObject>(locator, null, null);

            //Assert.IsNotNull(sm);
            //Assert.AreEqual("Bar is here", sm.StringProperty);
        }

        public static void CanInjectMultiplePropertiesIncludingCreatedObjects()
        {
            Builder builder = new Builder();
            Locator locator = CreateLocator();

            ConstructorPolicy policy = new ConstructorPolicy();
            policy.AddParameter(new ValueParameter<int>(15));
            builder.Policies.Set<ICreationPolicy>(policy, typeof(SimpleObject), null);

            PropertySetterPolicy policy1 = new PropertySetterPolicy();
            policy1.Properties.Add("StringProperty", new PropertySetterInfo("StringProperty", new ValueParameter<string>("Bar is here")));
            policy1.Properties.Add("SimpleObject", new PropertySetterInfo("SimpleObject", new CreationParameter(typeof(SimpleObject))));
            builder.Policies.Set<IPropertySetterPolicy>(policy1, typeof(ComplexObject), null);

            ComplexObject co = builder.BuildUp<ComplexObject>(locator, null, null);

            //Assert.IsNotNull(co);
            //Assert.IsNotNull(co.SimpleObject);
            //Assert.AreEqual("Bar is here", co.StringProperty);
            //Assert.AreEqual(15, co.SimpleObject.IntParam);
        }

        public static void CanCreateConcreteObjectByAskingForInterface()
        {
            Builder builder = new Builder();
            builder.Policies.Set<ITypeMappingPolicy>(new TypeMappingPolicy(typeof(SimpleObject), null), typeof(ISimpleObject), null);
            Locator locator = CreateLocator();

            ISimpleObject sm1 = builder.BuildUp<ISimpleObject>(locator, null, null);
            ISimpleObject sm2 = builder.BuildUp<ISimpleObject>(locator, null, null);
            //Assert.IsNotNull(sm);
            //Assert.IsTrue(sm is SimpleObject);
        }

        public static void CanCreateNamedConcreteObjectByAskingForNamedInterface()
        {
            Builder builder = new Builder();
            ConstructorPolicy policy = new ConstructorPolicy(new ValueParameter<int>(12));
            builder.Policies.Set<ICreationPolicy>(policy, typeof(SimpleObject), "Foo");
            builder.Policies.Set<ITypeMappingPolicy>(new TypeMappingPolicy(typeof(SimpleObject), null), typeof(ISimpleObject), null);
            builder.Policies.Set<ITypeMappingPolicy>(new TypeMappingPolicy(typeof(SimpleObject), "Foo"), typeof(ISimpleObject), "sm2");
            Locator locator = CreateLocator();

            ISimpleObject sm1 = builder.BuildUp<ISimpleObject>(locator, null, null);
            ISimpleObject sm2 = builder.BuildUp<ISimpleObject>(locator, "sm2", null);
            ISimpleObject sm3 = builder.BuildUp<ISimpleObject>(locator, "sm2", null);
            //Assert.IsNotNull(sm1);
            //Assert.IsNotNull(sm2);
            //Assert.IsTrue(sm1 is SimpleObject);
            //Assert.IsTrue(sm2 is SimpleObject);
            //Assert.AreEqual(0, ((SimpleObject)sm1).IntParam);
            //Assert.AreEqual(12, ((SimpleObject)sm2).IntParam);
        }

        public static void CanAddStrategiesToBuilder()
        {
            Builder builder = new Builder();
            MockStrategy strategy = new MockStrategy();
            Locator locator = CreateLocator();

            builder.Strategies.Add(strategy, BuilderStage.PostInitialization);

            builder.BuildUp(locator, typeof(object), null, null);

            //Assert.IsTrue(strategy.WasCalled);
        }

        public static void CanCreateGenericType()
        {
            Builder builder = new Builder();
            Locator locator = CreateLocator();

            GenericObject<int> result = builder.BuildUp<GenericObject<int>>(locator, null, null);

            //Assert.IsNotNull(result);
        }

        private static Locator CreateLocator()
        {
            Locator locator = new Locator();
            LifetimeContainer lifetime = new LifetimeContainer();
            locator.Add(typeof(ILifetimeContainer), lifetime);
            return locator;
        }

        private class GenericObject<TValue>
        {
            public TValue TheValue;

            public GenericObject(TValue theValue)
            {
                TheValue = theValue;
            }
        }

        public interface ISimpleObject
        {
        }

        public class SimpleObject : ISimpleObject
        {
            public int IntParam;
            private string stringProperty;

            public string StringProperty
            {
                get { return stringProperty; }
                set { stringProperty = value; }
            }

            public SimpleObject(int foo)
            {
                IntParam = foo;
            }
        }

        public class ComplexObject
        {
            private SimpleObject simpleObject;
            private string stringProperty;

            public SimpleObject SimpleObject
            {
                get { return simpleObject; }
                set { simpleObject = value; }
            }

            public string StringProperty
            {
                get { return stringProperty; }
                set { stringProperty = value; }
            }

            public ComplexObject(SimpleObject monk)
            {
                SimpleObject = monk;
            }
        }

        public class MockStrategy : BuilderStrategy
        {
            public bool WasCalled = false;

            public override object BuildUp(IBuilderContext context, Type t, object existing, string id)
            {
                WasCalled = true;
                return null;
            }
        }
        static void Main(string[] args)
        {
            CanCreateInstances();
        }
    }
}
