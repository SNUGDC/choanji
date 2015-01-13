using Gem;
namespace Choanji 
{
	public interface ICharacterInputDelegate
	{
		void ProcessInputYes();

		void ProcessInput(Direction _dir);

	}
}