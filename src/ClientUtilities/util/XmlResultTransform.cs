using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace NUnit.Util
{
	/// <summary>
	/// Summary description for XmlResultTransform.
	/// </summary>
	public class XmlResultTransform
	{
		private XslTransform xslTransform = new XslTransform();

		public XmlResultTransform() { }

		public XmlResultTransform( string stylesheet )
		{
			Load( stylesheet );
		}

		public XmlResultTransform( XmlReader reader )
		{
			Load( reader );
		}

		public void Load( string stylesheet )
		{
			xslTransform.Load( stylesheet );				
		}

		public void Load( XmlReader reader )
		{
			xslTransform.Load( reader );
		}

		public void Transform( string inputFile, string outputFile )
		{
			Transform( new StreamReader( inputFile ), new StreamWriter( outputFile ) );
		}

		public void Transform( TextReader reader, TextWriter writer )
		{
			Transform( new XPathDocument( reader ), writer );
		}

		public void Transform( IXPathNavigable xpnav, TextWriter writer )
		{
			xslTransform.Transform( xpnav, null, writer );
		}
	}
}