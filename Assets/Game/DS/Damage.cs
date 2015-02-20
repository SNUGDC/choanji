namespace Choanji
{
	public struct Damage
	{
		public ElementID ele;
		public HP val;

		public Damage(ElementID _ele, HP _val)
		{
			ele = _ele;
			val = _val;
		}

		public static implicit operator HP(Damage _this)
		{
			return _this.val;
		}
	}
}