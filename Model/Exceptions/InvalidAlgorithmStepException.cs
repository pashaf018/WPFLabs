using WPFLabs.Algorithms;
using System;

namespace WPFLabs.Exceptions
{
	[Serializable]
	public class InvalidAlgorithmStepException : Exception
	{
		public InvalidAlgorithmStepException(TypeOfStep step) : base("Неверный тип шага: " + step.ToString()) { }

		public InvalidAlgorithmStepException() { }

		public InvalidAlgorithmStepException(string message) : base(message) { }

		public InvalidAlgorithmStepException(string message, Exception? exception) : base(message, exception) { }
	}
}
