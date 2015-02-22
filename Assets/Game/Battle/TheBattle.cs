using System;
using Gem;

namespace Choanji.Battle
{
	public static class TheBattle 
	{
		public static bool isRunning;
		public static State state;
		public static Battle battle;

		public static TriggerManager trigger;
		public static ActionManager action;

		public static Action<Setup> onSetup;
		public static Action onStart;
		public static Action<Result> onDone;

		public static void Setup(Setup _setup)
		{
			if (isRunning)
			{
				L.W("trying to done but not running.");
				return;
			}

			isRunning = true;

			TheChoanji.g.context = ContextType.BATTLE;

			state = new State(
				new Battler(_setup.battlerA),
				new Battler(_setup.battlerB));

			trigger = new TriggerManager();
			action = new ActionManager();

			battle = new Battle(_setup.mode, state) { onFinish = Done };

			onSetup.CheckAndCall(_setup);
		}

		public static void Start()
		{
			onStart.CheckAndCall();
			battle.StartTurn();
		}

		private static void Done(Result _result)
		{
			if (!isRunning)
			{
				L.W("trying to done but not running.");
				return;
			}

			onDone(_result);

			isRunning = false;
			state = null;
			battle = null;
		}
	}
}