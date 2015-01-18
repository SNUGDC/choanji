using System.Collections;
using System.Collections.Generic;

namespace Choanji
{
	public class DialogProvider : IEnumerable<string>
	{
		private readonly List<string> mTxts = new List<string>();

		public DialogProvider Add(string _txt)
		{
			mTxts.Add(_txt);
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