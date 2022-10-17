using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AillieoUtils.CSReflectionUtils.Tests
{
    public class MethodInfoTests
    {
        [Test]
        public static void SignatureTests()
        {
            var ms = typeof(ClassDerived<int>).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            foreach (var m in ms)
            {
                UnityEngine.Debug.LogError(m.GetSignature());
            }
        }
    }

    public class ClassDerived<T> : ClassBase
    {
        public static async Task<int> AsyncMethod() { throw new NotImplementedException(); }

        public static int StaticMethod() { throw new NotImplementedException(); }

        public int InstanceMethod() { throw new NotImplementedException(); }

        public override int AbstractMethod() { throw new NotImplementedException(); }

        public virtual int VirtualMethod() { throw new NotImplementedException(); }

        public override int OverrideMethod() { throw new NotImplementedException(); }

        public int PublicMethod() { throw new NotImplementedException(); }

        private int PrivateMethod() { throw new NotImplementedException(); }

        protected int ProtectedMethod() { throw new NotImplementedException(); }

        internal int InternalMethod() { throw new NotImplementedException(); }

        protected internal int ProtectedInternalMethod() { throw new NotImplementedException(); }

        public int OutParameterMethod(out int i) { throw new NotImplementedException(); }

        public int RefParameterMethod(ref int i) { throw new NotImplementedException(); }

        public int InParameterMethod(in int i) { throw new NotImplementedException(); }

        public int ParamsParameterMethod(params int[] i) { throw new NotImplementedException(); }

        public int OptionalParameterMethod(int i = 1) { throw new NotImplementedException(); }

        public T GenericTypeMethod(T i) { throw new NotImplementedException(); }

        public T1 GenericMethod<T1, T2>(T1 i, T2 i2) { throw new NotImplementedException(); }
    }

    public abstract class ClassBase
    {
        public abstract int AbstractMethod();

        public abstract int OverrideMethod();

        public void InheritedMethod() { throw new NotImplementedException(); }
    }
}
