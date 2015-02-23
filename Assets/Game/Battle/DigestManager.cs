using System.Collections.Generic;
using Gem;

namespace Choanji.Battle
{
	public class DigestManager 
	{
		public bool empty { get { return mDigests.Empty(); } }
		public int count { get { return mDigests.Count; } }

		private readonly Queue<Digest> mDigests = new Queue<Digest>();

		public void Enq(Digest _digest)
		{
			mDigests.Enqueue(_digest);
		}

		public Digest Deq()
		{
			return mDigests.Dequeue();
		}
	}
}