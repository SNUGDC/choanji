using System.Collections;
using System.Collections.Generic;
using Gem;
using LitJson;

namespace Choanji
{
	public class DialogProvider : IEnumerable<string>
	{
		private readonly List<string> mTxts = new List<string>();

		public DialogProvider()
		{}

		public DialogProvider(JsonData _data)
		{
			Add(_data);
		}

		public DialogProvider(FullPath _path)
		{
			var _json = JsonHelper.DataWithRaw(_path);
			if (_json == null) return;
			Add(_json);
		}

		public DialogProvider Add(string _txt)
		{
			mTxts.Add(_txt);
			return this;
		}

		public DialogProvider Add(JsonData _json)
		{
			if (_json.IsString)
			{
				Add((string) _json);
			}
			else
			{
				foreach (var _txt in _json["txts"].GetListEnum())
					Add((string) _txt);
			}

			return this;
		}

		public DialogProvider Add(List<string> _txts)
		{
			mTxts.AddRange(_txts);
			return this;
		}

		public IEnumerator<string> GetEnumerator()
		{
			return mTxts.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

}