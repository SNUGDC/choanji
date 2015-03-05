using System;
using Gem;

namespace Choanji.Battle
{
	public static class TheBattle 
	{
		public static bool isRunning { get; private set; }
		public static bool isSetuped { get { return state != null; } }

		public static State state;
		public static TriggerManager trigger;
		public static DigestManager digest;
		public static Battle battle;

		public static Action<Setup> onSetup;
		public static Action onStart;
		public static Action<Result> onFinish;

		public static void Setup(Setup _setup)
		{
			if (isSetuped)
			{
				L.W("already setuped.");
				return;
			}

			isRunning = true;

			TheChoanji.g.context = ContextType.BATTLE;

			state = new State(
				new Battler(_setup.battlerA),
				new Battler(_setup.battlerB));

			trigger = new TriggerManager();
			digest = new DigestManager();

			battle = new Battle(_setup.mode, state) { onFinish = Finish };

			if (_setup.bgm)
				SoundManager.PlayImmediately(_setup.bgm, true);
			else 
				SoundManager.PlayImmediately(SoundDB.g.battleDefault, true);

			onSetup.CheckAndCall(_setup);
		}

		public static void Start()
		{
			battle.StartTurn();
			onStart.CheckAndCall();
		}

		public static void Update()
		{
			if (isRunning)
				battle.Update();
		}

		private static void Finish(Result _result)
		{
			if (!isRunning)
			{
				L.W("not running.");
				return;
			}

			isRunning = false;

			onFinish.CheckAndCall(_result);
		}

		public static void Cleanup()
		{
			if (isRunning)
			{
				L.E("cleanup while running.");
				return;
			}

			battle = null;
			trigger = null;
			digest = null;
			state = null;
		}
	}
}