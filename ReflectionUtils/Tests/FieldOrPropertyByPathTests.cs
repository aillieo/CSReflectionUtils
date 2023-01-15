using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AillieoUtils.CSReflectionUtils.Tests
{
    public class FieldOrPropertyByPathTests
    {
        [Test]
        public static void SetField1()
        {
            ClassA a = new ClassA();
            a.number = 2;
            ReflectionUtils.SetFieldOrPropertyValueByPath(a, "number", 3);
            Assert.AreEqual(a.number, 3);
        }

        [Test]
        public static void SetField2()
        {
            ClassA a = new ClassA();
            a.instanceB = new ClassB();
            a.instanceB.text = "hello";
            ReflectionUtils.SetFieldOrPropertyValueByPath(a, "instanceB.text", "nice");
            Assert.AreEqual(a.instanceB.text, "nice");
        }

        [Test]
        public static void SetProperty1()
        {
            ClassA a = new ClassA();
            a.numberProp = 3;
            ReflectionUtils.SetFieldOrPropertyValueByPath(a, "numberProp", 5);
            Assert.AreEqual(a.numberProp, 5);
        }

        [Test]
        public static void SetProperty2()
        {
            ClassA a = new ClassA();
            a.instanceBProp = new ClassB();
            a.instanceBProp.instanceAProp = new ClassA();
            ReflectionUtils.SetFieldOrPropertyValueByPath(a, "instanceBProp.instanceAProp.number", 10);
            Assert.AreEqual(a.instanceBProp.instanceAProp.number, 10);
        }

        [Test]
        public static void SetMix1()
        {
            ClassA a = new ClassA();
            a.instanceBProp = new ClassB();
            a.instanceBProp.instanceA = new ClassA();
            ReflectionUtils.SetFieldOrPropertyValueByPath(a, "instanceBProp.instanceA.number", 12);
            Assert.AreEqual(a.instanceBProp.instanceA.number, 12);
        }

        [Test]
        public static void SetMix2()
        {
            ClassA a = new ClassA();
            a.instanceB = new ClassB();
            a.instanceB.instanceAProp = new ClassA();
            ReflectionUtils.SetFieldOrPropertyValueByPath(a, "instanceB.instanceAProp.number", 8);
            Assert.AreEqual(a.instanceB.instanceAProp.number, 8);
        }

        [Test]
        public static void SetPropertyWithArray1()
        {
            ClassC c = new ClassC();
            c.arrayOfA = new ClassA[] { new ClassA() };
            ReflectionUtils.SetFieldOrPropertyValueByPath(c, "arrayOfA[0].number", 8);
            Assert.AreEqual(c.arrayOfA[0].number, 8);
        }

        [Test]
        public static void SetPropertyWithArray2()
        {
            ClassC c = new ClassC();
            c.numbers = new int[] { 0 };
            ReflectionUtils.SetFieldOrPropertyValueByPath(c, "numbers[0]", 123);
            Assert.AreEqual(c.numbers[0], 123);
        }

        [Test]
        public static void GetField1()
        {
            ClassA a = new ClassA();
            a.number = 2;
            ReflectionUtils.GetFieldOrPropertyValueByPath(a, "number", out object value);
            Assert.AreEqual(a.number, (int)value);
        }

        [Test]
        public static void GetField2()
        {
            ClassA a = new ClassA();
            a.instanceB = new ClassB();
            a.instanceB.text = "hello";
            ReflectionUtils.GetFieldOrPropertyValueByPath(a, "instanceB.text", out object value);
            Assert.AreEqual(a.instanceB.text, (string)value);
        }

        [Test]
        public static void GetProperty1()
        {
            ClassA a = new ClassA();
            a.numberProp = 3;
            ReflectionUtils.GetFieldOrPropertyValueByPath(a, "numberProp", out object value);
            Assert.AreEqual(a.numberProp, (int)value);
        }

        [Test]
        public static void GetProperty2()
        {
            ClassA a = new ClassA();
            a.instanceBProp = new ClassB();
            a.instanceBProp.instanceAProp = new ClassA();
            a.instanceBProp.instanceAProp.number = 9;
            ReflectionUtils.GetFieldOrPropertyValueByPath(a, "instanceBProp.instanceAProp.number", out object value);
            Assert.AreEqual(a.instanceBProp.instanceAProp.number, (int)value);
        }

        [Test]
        public static void GetMix1()
        {
            ClassA a = new ClassA();
            a.instanceBProp = new ClassB();
            a.instanceBProp.instanceA = new ClassA();
            a.instanceBProp.instanceA.number = 16;
            ReflectionUtils.GetFieldOrPropertyValueByPath(a, "instanceBProp.instanceA.number", out object value);
            Assert.AreEqual(a.instanceBProp.instanceA.number, (int)value);
        }

        [Test]
        public static void GetMix2()
        {
            ClassA a = new ClassA();
            a.instanceB = new ClassB();
            a.instanceB.instanceAProp = new ClassA();
            a.instanceB.instanceAProp.number = 20;
            ReflectionUtils.GetFieldOrPropertyValueByPath(a, "instanceB.instanceAProp.number", out object value);
            Assert.AreEqual(a.instanceB.instanceAProp.number, (int)value);
        }

        [Test]
        public static void GetPropertyWithArray1()
        {
            ClassC c = new ClassC();
            c.arrayOfA = new ClassA[] { new ClassA() };
            c.arrayOfA[0].number = 6;
            ReflectionUtils.GetFieldOrPropertyValueByPath(c, "arrayOfA[0].number", out object value);
            Assert.AreEqual(c.arrayOfA[0].number, (int)value);
        }

        [Test]
        public static void GetPropertyWithArray2()
        {
            ClassC c = new ClassC();
            c.numbers = new int[] { 123 };
            ReflectionUtils.GetFieldOrPropertyValueByPath(c, "numbers[0]", out object value);
            Assert.AreEqual(c.numbers[0], (int)value);
        }

        /////////////////////////////////////////////////////////////////////////////////////////

        public class ClassA
        {
            public int number;
            public ClassB instanceB;

            public int numberProp { get; set; }

            public ClassB instanceBProp { get; set; }
        }

        public class ClassB
        {
            public string text;
            public ClassA instanceA;

            public string textProp { get; set; }

            public ClassA instanceAProp { get; set; }
        }

        public class ClassC
        {
            public ClassA[] arrayOfA;
            public int[] numbers;
        }
    }
}
