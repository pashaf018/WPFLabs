using System;

namespace WPFLabs.Exceptions
{
	[Serializable]
	public class DrawingSizeTooBig : Exception
	{
		public DrawingSizeTooBig(int size) : base("Размеры объекта сликшмо большие: " + size) { }

		public DrawingSizeTooBig() { }

		public DrawingSizeTooBig(string message) : base(message) { }

		public DrawingSizeTooBig(string message, Exception? exception) : base(message, exception) { }
	}
}
