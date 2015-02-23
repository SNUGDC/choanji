﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Choanji.Battle
{
	public partial class Scene : MonoBehaviour
	{
		private static Scene mG;
		public static Scene g
		{
			get { return (mG ?? (mG = FindObjectOfType<Scene>())); }
		}

		public GameObject root;
		public Canvas canvas;

		public FieldView field;
		public BattlerView battler;

		public HPBar hp;
		public APBar ap;

		public PartyView party;
		public SelectionView selection;

		public SubmitButton submit;

		public MessageView msg;
		public Poper poper;

		private float mDelay;

		void Start()
		{
			TheBattle.onSetup += Setup;
		}

		void OnDestroy()
		{
			TheBattle.onSetup -= Setup;
		}

		public void Setup(Setup _setup)
		{
			mDelay = 0;

			var _battlerA = TheBattle.state.battlerA;
			var _battlerB = TheBattle.state.battlerB;

			field.env = _setup.env;
			battler.SetBattler(_battlerB.data.key);

			hp.max = (int)_battlerA.hpMax;
			ap.max = (int)_battlerA.apMax;
			hp.Full();
			ap.Full();

			party.Setup(_battlerA.party);
			party.onSelect = _card => selection.Add(_card);
			party.onCancel = _card => selection.Remove(_card);
			party.onPointerOver = (_enter, _card) =>
			{
				if (_enter)
					ap.highlight = _battlerA.CalConsumption(_card.data.active.cost);
				else
					ap.highlight = 0;
			};

			selection.onCancel += party.Cancel;
		}

		void Update()
		{
			if (!TheBattle.isSetuped)
				return;
				
			mDelay -= Time.deltaTime;

			if (mDelay > 0)
				return;

			if (!TheBattle.digest.empty)
			{
				mDelay = Animate(TheBattle.digest.Deq());
			}
			else if (!TheBattle.isRunning)
			{
				if (mDelay < -3)
				{
					TheChoanji.g.context = ContextType.WORLD;
					TheBattle.Cleanup();
				}
			}
		}

		public void GatherCards(Action<List<Card>> _onDone)
		{
			submit.onClick = () =>
			{
				selection.Clear();
				_onDone(party.Submit());
				submit.onClick = null;
			};
		}
	}
}
