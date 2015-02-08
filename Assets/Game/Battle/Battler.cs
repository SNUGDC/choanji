namespace Choanji
{
	public class Battler
	{
		public Battler(Deck _deck, int _hpMax, int _manaMax)
		{
			hpMax = hp = _hpMax;
			mana = manaMax = _manaMax;
			deck = _deck;
		}

		public readonly Deck deck;

		public readonly int hpMax;
		public int hp { get; private set; }

		public readonly int manaMax;
		public int mana { get; private set; }
	}
}