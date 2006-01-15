using System;

using NUnit.Framework;
using NUnit.Core;
using NUnit.Util;
using NUnit.Tests.Assemblies;

namespace NUnit.Core.Tests
{
	/// <summary>
	/// Base class for tests of various kinds of runners
	/// </summary>
	public abstract class BasicRunnerTests
	{
		private static readonly string testsDll = "nonamespace-assembly.dll";
		private static readonly string mockDll = "mock-assembly.dll";
		private readonly string[] assemblies = new string[] { testsDll, mockDll };

		private TestRunner runner;

		[SetUp]
		public void SetUpRunner()
		{
			runner = CreateRunner( 123 );
		}

		protected abstract TestRunner CreateRunner( int runnerID );

        [Test]
        public void CheckRunnerID()
        {
            Assert.AreEqual(123, runner.ID);
        }

        [Test]
		public void LoadAssembly() 
		{
			Assert.IsTrue(runner.Load(testsDll), "Unable to load assembly" );
		}

		[Test]
		public void LoadAssemblyWithFixture()
		{
			Assert.IsTrue( runner.Load( mockDll, "NUnit.Tests.Assemblies.MockTestFixture" ) );
		}

		[Test]
		public void LoadAssemblyWithSuite()
		{
			runner.Load( mockDll, "NUnit.Tests.Assemblies.MockSuite" );
			Assert.IsNotNull(runner.Test, "Unable to build suite");
		}

		[Test]
		public void CountTestCases()
		{
			runner.Load( mockDll );
			Assert.AreEqual( MockAssembly.Tests, runner.Test.TestCount );
		}

		[Test]
		public void LoadMultipleAssemblies()
		{
			runner.Load( "TestSuite", assemblies );
			Assert.IsNotNull( runner.Test, "Unable to load assemblies" );
		}

		[Test]
		public void LoadMultipleAssembliesWithFixture()
		{
			runner.Load( "TestSuite", assemblies, "NUnit.Tests.Assemblies.MockTestFixture"  );
			Assert.IsNotNull(runner.Test, "Unable to build suite");
		}

		[Test]
		public void LoadMultipleAssembliesWithSuite()
		{
			runner.Load( "TestSuite", assemblies, "NUnit.Tests.Assemblies.MockSuite" );
			Assert.IsNotNull(runner.Test, "Unable to build suite");
		}

		[Test]
		public void CountTestCasesAcrossMultipleAssemblies()
		{
			runner.Load( "TestSuite", assemblies );
			Assert.AreEqual( NoNamespaceTestFixture.Tests + MockAssembly.Tests, 
				runner.Test.TestCount );			
		}

		[Test]
		public void RunAssembly()
		{
			runner.Load(mockDll);
			TestResult result = runner.Run( NullListener.NULL );
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.AreEqual( MockAssembly.Tests - MockAssembly.NotRun, summary.ResultCount );
		}

		[Test]
		public void RunAssemblyUsingBeginAndEndRun()
		{
			runner.Load(mockDll);
			runner.BeginRun( NullListener.NULL );
			TestResult[] results = runner.EndRun();
			Assert.IsNotNull( results );
			Assert.AreEqual( 1, results.Length );
			ResultSummarizer summary = new ResultSummarizer( results[0] );
			Assert.AreEqual( MockAssembly.Tests - MockAssembly.NotRun, summary.ResultCount );
		}

		[Test]
		public void RunMultipleAssemblies()
		{
			runner.Load( "TestSuite", assemblies );
			TestResult result = runner.Run( NullListener.NULL );
			ResultSummarizer summary = new ResultSummarizer(result);
			Assert.AreEqual( 
				NoNamespaceTestFixture.Tests + MockAssembly.Tests - MockAssembly.NotRun, 
				summary.ResultCount);
		}

		[Test]
		public void RunMultipleAssembliesUsingBeginAndEndRun()
		{
			runner.Load( "TestSuite", assemblies );
			runner.BeginRun( NullListener.NULL );
			TestResult[] results = runner.EndRun();
			Assert.IsNotNull( results );
			Assert.AreEqual( 1, results.Length );
			ResultSummarizer summary = new ResultSummarizer( results[0] );
			Assert.AreEqual( 
				NoNamespaceTestFixture.Tests + MockAssembly.Tests - MockAssembly.NotRun, 
				summary.ResultCount);
		}
	}
}