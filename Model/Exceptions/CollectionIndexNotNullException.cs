using System;

namespace WPFLabs.Exceptions
{
	[Serializable]
	public class CollectionIndexNotNullException : Exception
	{
		public CollectionIndexNotNullException(Object obj) : base("В коллекции уже есть такой объект: " + obj.ToString()) { }

		public CollectionIndexNotNullException() { }

		public CollectionIndexNotNullException(string? message) : base(message) { }

		public CollectionIndexNotNullException(string? message, Exception? innerException) : base(message, innerException) { }
	}
}
