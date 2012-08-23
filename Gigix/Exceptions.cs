using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gigix
{
	[Serializable]
	public class InvalidCharactersSequenceException : Exception
	{
		public InvalidCharactersSequenceException() { }
		public InvalidCharactersSequenceException(string message) : base(message) { }
		public InvalidCharactersSequenceException(string message, Exception inner) : base(message, inner) { }
		protected InvalidCharactersSequenceException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
