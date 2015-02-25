using System;
using System.Collections.Generic;
using Gem;
using UnityEngine;
using UnityEngine.UI;

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

		private bool mIsResultOpened;
		private Result mResult;

		public Popup winPopupPrf;
		public Popup losePopupPrf;

		private readonly Timer mTimer = new Timer();
		private float mDelay;

		void Start()
		{
			TheBattle.onSetup += Setup;
			TheBattle.onFinish += OnFinish;
		}

		void OnDestroy()
		{
			TheBattle.onSetup -= Setup;
			TheBattle.onFinish -= OnFinish;
		}

		public void Setup(Setup _setup)
		{
			mDelay = 0;
			mResult = null;
			mIsResultOpened = false;

			var _battlerA = TheBattle.state.battlerA;
			var _battlerB = TheBattle.state.battlerB;

			field.SetEnv(_setup.env, _setup.envIdx);
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
			var _dt = Time.deltaTime;
			mTimer.Update(_dt);

			if (!TheBattle.isSetuped)
				return;

			mDelay -= _dt;

			if (mDelay > 0)
				return;

			if (!TheBattle.digest.empty)
			{
				mDelay = Animate(TheBattle.digest.Deq());
			}
			else if (mResult != null && !mIsResultOpened)
			{
				mIsResultOpened = true;

				mTimer.Add(1, () => OpenResultPopup(() =>
				{
					TheChoanji.g.context = ContextType.WORLD;
					TheBattle.Cleanup();
				}));
			}
		}

		private void GatherCards(Action<List<Card>> _onDone)
		{
			submit.Show();
			submit.onClick = () =>
			{
				if (!TheBattle.digest.empty)
					return;
				submit.ChooseAndHide();
				selection.Clear();
				_onDone(party.Submit());
				submit.onClick = null;
			};
		}

		private void OnFinish(Result _result)
		{
			mResult = _result;
		}

		private void OpenResultPopup(Action _onClose)
		{
			SoundManager.StopMusic();

			Popup _popup;
			string _label;

			switch (mResult.type)
			{
				case ResultType.WIN_A:
					_popup = winPopupPrf.Instantiate();
					_label = new RichText("WIN").AddSize(120).AddColor(Color.blue);
					SoundManager.Play(SoundDB.g.battleWin, true);
					break;
				case ResultType.WIN_B:
					_popup = losePopupPrf.Instantiate();
					_label = new RichText("LOSE").AddSize(120).AddColor(Color.red);
					break;
				default:
					return;
			}

			_popup.transform.SetParent(canvas.transform, false);

			var _txt = _popup.transform.FindChild("Text").GetComponent<Text>();
			_txt.text = _label;

			_popup.onClose = _onClose;
			_popup.Open(null);
		}
	}
}
