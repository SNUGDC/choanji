using System.Collections.Generic;

namespace Choanji.Battle
{
	public class DigestManager 
	{
		public int count;

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