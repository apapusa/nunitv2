#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright � 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright � 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright � 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright � 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Core
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Reflection;

	/// <summary>
	///		Test Class.
	/// </summary>
	public abstract class Test : LongLivingMarshalByRefObject, ITest, IComparable
	{
		#region Fields
		/// <summary>
		/// TestName that identifies this test
		/// </summary>
		private TestName testName;

		/// <summary>
		/// Indicates whether the test should be executed
		/// </summary>
		private RunState runState;

		/// <summary>
		/// The reason for not running the test
		/// </summary>
		private string ignoreReason;
		
		/// <summary>
		/// Description for this test 
		/// </summary>
		private string description;
		
		/// <summary>
		/// Test suite containing this test, or null
		/// </summary>
		private TestSuite parent;
		
		/// <summary>
		/// List of categories applying to this test
		/// </summary>
		private IList categories;

		/// <summary>
		/// A dictionary of properties, used to add information
		/// to tests without requiring the class to change.
		/// </summary>
		private IDictionary properties;

		#endregion

		#region Construction

		protected Test( string name )
		{
			this.testName = new TestName();
			this.testName.FullName = name;
			this.testName.Name = name;
			this.testName.TestID = new TestID();

            this.runState = RunState.Runnable;
		}

		protected Test( string pathName, string name ) 
		{ 
			this.testName = new TestName();
			this.testName.FullName = pathName == null || pathName == string.Empty 
				? name : pathName + "." + name;
			this.testName.Name = name;
			this.testName.TestID = new TestID();

            this.runState = RunState.Runnable;
		}
	
		internal void SetRunnerID( int runnerID, bool recursive )
		{
			this.testName.RunnerID = runnerID;

			if ( recursive && this.Tests != null )
				foreach( Test child in this.Tests )
					child.SetRunnerID( runnerID, true );
		}

		#endregion

		#region Properties
		public TestName TestName
		{
			get { return testName; }
		}

		/// <summary>
		/// Whether or not the test should be run
		/// </summary>
        public RunState RunState
        {
            get { return runState; }
            set { runState = value; }
        }

		/// <summary>
		/// Reason for not running the test, if applicable
		/// </summary>
		public string IgnoreReason
		{
			get { return ignoreReason; }
			set { ignoreReason = value; }
		}

		ITest ITest.Parent 
		{
			get { return parent; }
		}

		public TestSuite Parent
		{
			get { return parent; }
			set { parent = value; }
		}

		public IList Categories 
		{
			get { return categories; }
			set { categories = value; }
		}

		public String Description
		{
			get { return description; }
			set { description = value; }
		}

		public IDictionary Properties
		{
			get 
			{
				if ( properties == null )
					properties = new ListDictionary();

				return properties; 
			}
			set
			{
				properties = value;
			}
		}
		#endregion

		#region Virtual Methods and Properties
		public virtual int TestCount 
		{ 
			get { return 1; } 
		}

		public virtual int CountTestCases()
		{
			return CountTestCases( TestFilter.Empty );
		}

		public virtual bool Filter(TestFilter filter)
		{
			return filter.Pass( this );
		}

		public virtual TestResult Run( EventListener listener )
		{
			return Run( listener, TestFilter.Empty );
		}
		#endregion

		#region Abstract Methods and Properties
		public abstract int CountTestCases(TestFilter filter);
		
		public abstract bool IsSuite { get; }
		public abstract bool IsFixture{ get; }

		public abstract IList Tests { get; }

		public abstract TestResult Run(EventListener listener, TestFilter filter);
		#endregion

		#region IComparable Members
		public int CompareTo(object obj)
		{
			Test other = obj as Test;
			
			if ( other == null )
				return -1;

			return this.TestName.FullName.CompareTo( other.TestName.FullName );
		}
		#endregion
	}
}
