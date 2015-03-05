using System;
using Gem;

namespace Choanji
{
	public sealed class TriggerInspect : Trigger
	{
		private readonly LocalCoor mCoor;
		private readonly IInspectee mInspectee;

		private class Inspectee : IInspectee
		{
			private readonly WeakReference mTrigger;

			public Inspectee(WeakReference _trigger)
			{
				mTrigger = _trigger;
			}

			protected override void DoStart(InspectRequest _data)
			{
				if (mTrigger.IsAlive)
				{
					var _trigger = (Trigger) mTrigger.Target;
					if (_trigger.isEnabled)
						_trigger.Invoke(null);
				}
				Done(null);
			}
		}

		public TriggerInspect(LocalCoor _coor)
			: base(TriggerType.INSPECT)
		{
			mCoor = _coor;
			mInspectee = new Inspectee(new WeakReference(this));
		}

		private TileState GetTile()
		{
			TileState _tile;
			mCoor.map.dynamic.grid.TryGet(mCoor.val, out _tile);

			if (_tile == null)
			{
				L.W("tile not exists.");
				return null;
			}

			return _tile;
		}

		protected override void DoEnable()
		{
			base.DoEnable();
			var _tile = GetTile();
			if (_tile == null) return;
			if (_tile.inspectee != null)
				L.W("tile already has inspectee.");
			_tile.inspectee = mInspectee;
		}

		protected override void DoDisable()
		{
			base.DoDisable();
			var _tile = GetTile();
			if (_tile == null) return;
			if (_tile.inspectee == mInspectee)
				_tile.inspectee = null;
			else
				L.W("inspectee changed");
		}
	}
}