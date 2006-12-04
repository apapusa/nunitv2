using System;
using System.Collections;
using System.Text;

namespace NUnit.Core.Filters
{
	/// <summary>
	/// SimpleName filter selects tests based on their name
	/// </summary>
    [Serializable]
    public class SimpleNameFilter : RecursiveTestFilter
    {
        private ArrayList names = new ArrayList();

		/// <summary>
		/// Construct an empty SimpleNameFilter
		/// </summary>
        public SimpleNameFilter() { }

        /// <summary>
        /// Construct a SimpleNameFilter for a single name
        /// </summary>
        /// <param name="name">The name the filter will recognize</param>
		public SimpleNameFilter( string name )
        {
            this.names.Add( name );
        }

		/// <summary>
		/// Add a name to a SimpleNameFilter
		/// </summary>
		/// <param name="name">The name to be added</param>
		public void Add( string name )
		{
			names.Add( name );
		}

		/// <summary>
		/// Check whether the filter matches a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if it matches, otherwise false</returns>
		public override bool Match( ITest test )
		{
			foreach( string name in names )
				if ( test.TestName.FullName == name )
					return true;

			return false;
		}
	}
}