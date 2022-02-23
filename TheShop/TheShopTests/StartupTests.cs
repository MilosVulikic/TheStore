using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheShop;

namespace TheShopTests
{
	[TestClass]
	public class StartupTests
	{
		
		public StartupTests()
		{
			Startup.Instance.MapInterfaceToConcreteType(typeof(IBBB),typeof(BBB));
			Startup.Instance.MapInterfaceToConcreteType(typeof(ICCC), typeof(CCC));
			Startup.Instance.MapInterfaceToConcreteType(typeof(IDDD), typeof(DDD));
						
			
			// For testing specific constructor (comment to use first defined constructor)
			Startup.Instance.MapTypeToConstructor(typeof(A), typeof(A).GetConstructor(new Type[] { typeof(B), typeof(C) }));
			Startup.Instance.MapTypeToConstructor(typeof(B), typeof(B).GetConstructor(new Type[] { typeof(C), typeof(D) }));			
			Startup.Instance.MapTypeToConstructor(typeof(AAA), typeof(AAA).GetConstructor(new Type[] { typeof(BBB), typeof(CCC) }));
			Startup.Instance.MapTypeToConstructor(typeof(BBB), typeof(BBB).GetConstructor(new Type[] { typeof(CCC), typeof(DDD) }));
		}

		// Tests for concrete classes
		[TestMethod]
		public void InstantiatiorCalled_LowerLevelConcreteClassDatatypeC_InstanceOfDatatypeCReturned()
		{
			var c = Startup.Instance.Instantiator<C>();
			Assert.IsNotNull(c);
			Assert.IsInstanceOfType(c,typeof(C));
		}

		[TestMethod]
		public void InstantiatiorCalled_MidlevelConcreteClassDatatypeB_InstanceOfDatatypeBReturned()
		{
			var b = Startup.Instance.Instantiator<B>();
			Assert.IsNotNull(b);			
			Assert.IsInstanceOfType(b, typeof(B));
			Assert.IsNotNull(b._c);
			Assert.IsInstanceOfType(b._c, typeof(C));
			Assert.IsNotNull(b._d);
			Assert.IsInstanceOfType(b._d, typeof(D));
		}

		[TestMethod]
		public void InstantiatiorCalled_ToplevelConcreteClassDatatypeA_InstanceOfDatatypeAReturned()
		{			
			var a = Startup.Instance.Instantiator<A>();
			Assert.IsNotNull(a);
			Assert.IsInstanceOfType(a, typeof(A));
			Assert.IsNotNull(a._b);
			Assert.IsInstanceOfType(a._b, typeof(B));
			Assert.IsNotNull(a._c);
			Assert.IsInstanceOfType(a._c, typeof(C));
			Assert.IsNotNull(a._b._c);
			Assert.IsInstanceOfType(a._b._c, typeof(C));
			Assert.IsNotNull(a._b._d);
			Assert.IsInstanceOfType(a._b._d, typeof(D));
		}

		
		// Tests for classes with interfaces default constructors
		[TestMethod]
		public void InstantiatiorCalled_MidlevelClassWithInterfacesDatatypeBBB_InstanceOfDatatypeBBBReturned()
		{
			var b = Startup.Instance.Instantiator<BBB>();
			Assert.IsNotNull(b);
			Assert.IsInstanceOfType(b, typeof(BBB));
			Assert.IsNotNull(b.CCC);
			Assert.IsInstanceOfType(b.CCC, typeof(CCC));
			Assert.IsNotNull(b.DDD);
			Assert.IsInstanceOfType(b.DDD, typeof(DDD));
		}

		[TestMethod]
		public void InstantiatiorCalled_ToplevelClassWithInterfacesDatatypeAAA_InstanceOfDatatypeAAAReturned()
		{
			var a = Startup.Instance.Instantiator<AAA>();
			Assert.IsNotNull(a);
			Assert.IsInstanceOfType(a, typeof(AAA));
			Assert.IsNotNull(a._bbb);
			Assert.IsInstanceOfType(a._bbb, typeof(BBB));
			Assert.IsNotNull(a._ccc);
			Assert.IsInstanceOfType(a._ccc, typeof(CCC));
			Assert.IsNotNull(a._bbb.CCC);
			Assert.IsInstanceOfType(a._bbb.CCC, typeof(CCC));
			Assert.IsNotNull(a._bbb.DDD);
			Assert.IsInstanceOfType(a._bbb.DDD, typeof(DDD));
		}

	}

	#region ConcreteClasses
	// Top level class
	public class A
	{
		public B _b = null;
		public C _c = null;

		public A()
		{

		}

		public A(B b)
		{
			_b = b;
		}
		public A(B b, C c)
		{
			_b = b;
			_c = c;
		}

		public void DummyMethodA()
		{
			Console.WriteLine("called dummy method");
		}
	}

	// Midlevel class
	public class B
	{
		public C _c = null;
		public D _d = null;

		public B(C c, D d)
		{
			_c = c;
			_d = d;
		}
		public void DummyMethodB()
		{
			Console.WriteLine("called dummy method");
		}
	}

	// Lower level class
	public class C
	{
		public void DummyMethodC()
		{
			Console.WriteLine("called dummy method");
		}
	}

	// Bottom level class
	public class D
	{
		public void DummyMethodD()
		{
			Console.WriteLine("called dummy method");
		}
	}
	#endregion


	#region ClassesWithInterfaces
	// Top level class
	public interface IBBB
	{
		void DummyMethodBBB();
		ICCC CCC { get; }
		IDDD DDD { get; }
	}

	public interface ICCC
	{
		void DummyMethodCCC();
	}

	public interface IDDD
	{
		void DummyMethodDDD();
	}


	public class AAA
	{
		public IBBB _bbb = null;
		public ICCC _ccc = null;

		public AAA(IBBB bbb, ICCC ccc)
		{
			_bbb = bbb;
			_ccc = ccc;
		}

		public void DummyMethodAAA()
		{
			Console.WriteLine("called dummy method");
		}
	}



	// Midlevel class
	public class BBB : IBBB
	{
		public ICCC _ccc = null;
		public IDDD _ddd = null;

		public BBB(ICCC ccc, IDDD ddd)
		{
			_ccc = ccc;
			_ddd = ddd;
		}

		public ICCC CCC => _ccc;

		public IDDD DDD => _ddd;

		public void DummyMethodBBB()
		{		
			Console.WriteLine("called dummy method");
		}
		
	}

	// Lower level class
	public class CCC : ICCC
	{
		public void DummyMethodCCC()
		{
			Console.WriteLine("called dummy method");
		}
	}

	// Bottom level class
	public class DDD : IDDD
	{
		public void DummyMethodDDD()
		{
			Console.WriteLine("called dummy method");
		}
	}
	#endregion
}
