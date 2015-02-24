#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Choanji.Battle
{
	public class ViewTest : MonoBehaviour
	{
		public HPBar hp;
		public APBar cost;
		public SCView sc;

		public MessageView msg;
		public List<string> txts;

		void Start()
		{
			hp.max = 50;
			cost.max = 50;
			cost.highlight = (AP)20;

			msg.Push(new MessageView.Message
			{
				txts = txts,
				manual = true,
			});
		}

		void Update()
		{
			var _val = Mathf.Abs(Mathf.Sin(Time.time));
			hp.Set(_val * 50, false);
			cost.Set(_val * 50, false);
			sc.sc = (SC) ((int) Time.time%SCHelper.COUNT);
		}
	}
}

#endif