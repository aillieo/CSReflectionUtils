using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AillieoUtils.CSReflectionUtils.Tests
{
    public class MethodInfoTests
    {
        [Test]
        public void TestMethodWithNoParameters()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.MethodWithNoParams));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public void MethodWithNoParams()", declaration);
        }

        [Test]
        public void TestMethodWithOneParameter()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.MethodWithOneParam));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public void MethodWithOneParam(int param1)", declaration);
        }

        [Test]
        public void TestMethodWithMultipleParameters()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.MethodWithMultiParams));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public void MethodWithMultiParams(int param1, string param2)", declaration);
        }

        [Test]
        public void TestStaticMethod()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.StaticMethod), BindingFlags.Static | BindingFlags.Public);
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public static int StaticMethod()", declaration);
        }

        [Test]
        public void TestPrivateMethod()
        {
            var methodInfo = typeof(ClassDerived).GetMethod("PrivateMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("private void PrivateMethod()", declaration);
        }

        [Test]
        public void TestMethodWithProtectedAccessModifier()
        {
            var methodInfo = typeof(ClassDerived).GetMethod("ProtectedMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("protected void ProtectedMethod()", declaration);
        }

        [Test]
        public void TestSealedMethod()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.SealedMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public sealed override int SealedMethod()", declaration);
        }

        [Test]
        public void TestOverriddenMethod()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.OverrideMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public override int OverrideMethod()", declaration);
        }

        [Test]
        public void TestVirtualMethod()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.VirtualMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public virtual int VirtualMethod()", declaration);
        }

        [Test]
        public void TestAsyncMethod()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.AsyncMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public static async Task<int> AsyncMethod()", declaration);
        }

        [Test]
        public void TestRefParameter()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.RefParameterMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public void RefParameterMethod(ref int i)", declaration);
        }

        [Test]
        public void TestOutParameter()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.OutParameterMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public void OutParameterMethod(out int i)", declaration);
        }

        [Test]
        public void TestInParameter()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.InParameterMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public void InParameterMethod(in int i)", declaration);
        }

        [Test]
        public void TestMethodWithGenericTypeParameter1()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.GenericMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public T1 GenericMethod<T1, T2>(T1 i, T2 i2)", declaration);
        }

        [Test]
        public void TestInternalMethod()
        {
            var methodInfo = typeof(ClassDerived).GetMethod(nameof(ClassDerived.InternalMethod), BindingFlags.NonPublic | BindingFlags.Instance);
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("internal void InternalMethod()", declaration);
        }

        [Test]
        public void TestAbstractMethod()
        {
            var methodInfo = typeof(ClassBase).GetMethod(nameof(ClassBase.AbstractMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public abstract int AbstractMethod()", declaration);
        }

        [Test]
        public void TestMethodWithGenericTypeParameter2()
        {
            var methodInfo = typeof(ClassDerivedGeneric<int>).GetMethod(nameof(ClassDerivedGeneric<int>.GenericMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public T1 GenericMethod<T1, T2>(T1 i, T2 i2)", declaration);
        }

        [Test]
        public void TestMethodWithGenericTypeParameter3()
        {
            var methodInfo = typeof(ClassDerivedGeneric<int>).GetMethod(nameof(ClassDerivedGeneric<int>.GenericTypeMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public int GenericTypeMethod(int i)", declaration);
        }

        [Test]
        public void TestMethodWithGenericTypeParameter4()
        {
            var methodInfo = typeof(OtherGenericMethods).GetMethod(nameof(OtherGenericMethods.GenericReturnTypeMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public static Dictionary<int, string> GenericReturnTypeMethod()", declaration);
        }

        [Test]
        public void TestMethodWithGenericTypeParameter5()
        {
            var methodInfo = typeof(OtherGenericMethods).GetMethod(nameof(OtherGenericMethods.GenericParameterTypeMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public static void GenericParameterTypeMethod(Dictionary<int, string> i)", declaration);
        }

        [Test]
        public void TestMethodWithGenericTypeParameter6()
        {
            var methodInfo = typeof(OtherGenericMethods).GetMethod(nameof(OtherGenericMethods.NestedGenericReturnTypeMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public static Dictionary<int, List<string>> NestedGenericReturnTypeMethod()", declaration);
        }

        [Test]
        public void TestMethodWithGenericTypeParameter7()
        {
            var methodInfo = typeof(OtherGenericMethods).GetMethod(nameof(OtherGenericMethods.NestedGenericParameterTypeMethod));
            var declaration = methodInfo.GetDeclaration();

            Assert.AreEqual("public static void NestedGenericParameterTypeMethod(Dictionary<int, List<string>> i1, List<Dictionary<int, string>> i2)", declaration);
        }
    }

    public class ClassDerived : ClassBase
    {
        public void MethodWithNoParams() { throw new NotImplementedException(); }

        public void MethodWithOneParam(int param1) { throw new NotImplementedException(); }

        public void MethodWithMultiParams(int param1, string param2) { throw new NotImplementedException(); }

        public static async Task<int> AsyncMethod() { throw new NotImplementedException(); }

        public static int StaticMethod() { throw new NotImplementedException(); }

        public int InstanceMethod() { throw new NotImplementedException(); }

        public override int AbstractMethod() { throw new NotImplementedException(); }

        public virtual int VirtualMethod() { throw new NotImplementedException(); }

        public override int OverrideMethod() { throw new NotImplementedException(); }

        public sealed override int SealedMethod() { throw new NotImplementedException(); }

        public int PublicMethod() { throw new NotImplementedException(); }

        private void PrivateMethod() { throw new NotImplementedException(); }

        protected void ProtectedMethod() { throw new NotImplementedException(); }

        internal void InternalMethod() { throw new NotImplementedException(); }

        protected internal int ProtectedInternalMethod() { throw new NotImplementedException(); }

        public void OutParameterMethod(out int i) { throw new NotImplementedException(); }

        public void RefParameterMethod(ref int i) { throw new NotImplementedException(); }

        public void InParameterMethod(in int i) { throw new NotImplementedException(); }

        public int ParamsParameterMethod(params int[] i) { throw new NotImplementedException(); }

        public int OptionalParameterMethod(int i = 1) { throw new NotImplementedException(); }

        public T1 GenericMethod<T1, T2>(T1 i, T2 i2) { throw new NotImplementedException(); }
    }

    public class ClassDerivedGeneric<T> : ClassBase
    {
        public override int AbstractMethod() { throw new NotImplementedException(); }

        public override int OverrideMethod() { throw new NotImplementedException(); }

        public T GenericTypeMethod(T i) { throw new NotImplementedException(); }

        public T1 GenericMethod<T1, T2>(T1 i, T2 i2) { throw new NotImplementedException(); }
    }

    public abstract class ClassBase
    {
        public abstract int AbstractMethod();

        public abstract int OverrideMethod();

        public virtual int SealedMethod() { throw new NotImplementedException(); }

        public void InheritedMethod() { throw new NotImplementedException(); }
    }

    public static class OtherGenericMethods
    {
        public static Dictionary<int, string> GenericReturnTypeMethod() { throw new NotImplementedException(); }

        public static Dictionary<int, List<string>> NestedGenericReturnTypeMethod() { throw new NotImplementedException(); }

        public static void GenericParameterTypeMethod(Dictionary<int, string> i) { throw new NotImplementedException(); }

        public static void NestedGenericParameterTypeMethod(Dictionary<int, List<string>> i1, List<Dictionary<int, string>> i2) { throw new NotImplementedException(); }
    }
}
