using System;
using Gem;

namespace Choanji.Battle
{
	public static class TheBattle 
	{
		public static bool isRunning;
		public static State state;
		public static Battle battle;
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
				new Battler(_setup.battlerA.stat, _setup.battlerA.party),
				new Battler(_setup.battlerB.stat, _setup.battlerB.party));

			battle = new Battle(_setup.mode, state);
		}

		public static void Start()
		{
			battle.SelectCards();
		}

		public static void Done(Result _result)
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