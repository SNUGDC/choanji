namespace Choanji
{
	public struct Damage
	{
		public ElementID ele;
		public int val;

		public static implicit operator int(Damage _this)
		{
			return _this.val;
		}
	}
}