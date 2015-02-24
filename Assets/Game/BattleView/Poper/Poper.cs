using Gem;
using Gem.Sampler;
using UnityEngine;

namespace Choanji.Battle
{
	public class Poper : MonoBehaviour
	{
		private static readonly UniformRect DMG_SAMPLER = new UniformRect(
			new Vector2(40, 40), 
			new Vector2(80, 80));

		public DamagePop prfDmg;
		public TextPop prfText;

		public void PopDmg(Damage _dmg)
		{
			var _dmgPop = prfDmg.Instantiate();
			_dmgPop.transform.SetParent(transform);
			_dmgPop.transform.localPosition = DMG_SAMPLER.Sample();
			_dmgPop.transform.localScale = Vector3.one;
			_dmgPop.dmg = _dmg;
		}

		public void PopText(string _txt)
		{
			var _textPop =  prfText.Instantiate();
			_textPop.transform.SetParent(transform, false);
			_textPop.transform.Translate(-150f, 0f, 0f);
			_textPop.txt.text = _txt;
		}
	}
}