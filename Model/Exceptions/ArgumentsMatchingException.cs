using System;

namespace WPFLabs.Exceptions
{
	[Serializable]
	public class ArgumentsMatchingException : Exception
	{
		public ArgumentsMatchingException(Object obj) : base("Были переданы 2 одиннаковых объекта: " + obj.ToString()) { }

		public ArgumentsMatchingException() : base() { }

		public ArgumentsMatchingException(string message) : base(message) { }

		public ArgumentsMatchingException(string message, Exception exception) : base(message, exception) { }
	}
}
