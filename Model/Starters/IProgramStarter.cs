using WPFLabs.Drawings;
using WPFLabs.Model.Starters;

namespace WPFLabs.Starters
{
	public interface IProgramStarter
	{
		public void StartAlgorithm(int interval);

		public ShowStepReturnable ShowStep();

		public void StopAlgorithm();

		public void ShowNextStep();

		public void ShowPreviousStep();

		public void ChangeSpeed(int interval);

		public void SaveData(string filename);

		public DrawingGraph LoadData(string filename,int pictureWidth,int pictureHeight);
	}
}
