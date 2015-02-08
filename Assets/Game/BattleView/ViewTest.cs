#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Choanji.Battle
{
	public class ViewTest : MonoBehaviour
	{
		public HPBar hp;
		public CostBar cost;
		public SCView sc;

		public MessageView msg;
		public List<string> txts;

		void Start()
		{
			hp.max = 50;
			cost.max = 50;
			cost.highlight = 20;

			msg.Push(new MessageView.Message
			{
				txts = txts,
				autoclear = false,
			});
		}

		void Update()
		{
			var _val = Mathf.Abs(Mathf.Sin(Time.time));
			hp.Set(_val * 50);
			cost.Set(_val * 50);
			sc.sc = (SC) ((int) Time.time%SCHelper.COUNT);
		}
	}
}

#endif