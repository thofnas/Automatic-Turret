using System;
using System.Globalization;
using Events;
using Managers;
using Turret;
using UnityEngine;

namespace UserInterface.StateMachine.States
{
    public class UIPlayState : UIState
    {
        public UIPlayState(UIStateMachine context, UIStateFactory uiStateFactory)
            : base(context, uiStateFactory) { }

        public override void EnterState()
        {
            GameEvents.OnWaveStarted.AddListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.AddListener(GameEvents_Wave_OnWaveEnded);
            GameEvents.OnSubWaveStarted.AddListener(GameEvents_Wave_OnSubWaveStarted);
            GameEvents.OnSubWaveEnded.AddListener(GameEvents_Wave_OnSubWaveEnded);
            GameEvents.OnCollectedGearAmountChanged.AddListener(GameEvents_Item_OnCollectedGearAmountChanged);
            GameEvents.OnWaveLost.AddListener(GameEvents_Wave_OnLost);
            GameEvents.OnWaveWon.AddListener(GameEvents_Wave_OnWon);
        }

        public override void ExitState()
        {
            GameEvents.OnWaveStarted.RemoveListener(GameEvents_Wave_OnWaveStarted);
            GameEvents.OnWaveEnded.RemoveListener(GameEvents_Wave_OnWaveEnded);
            GameEvents.OnSubWaveStarted.RemoveListener(GameEvents_Wave_OnSubWaveStarted);
            GameEvents.OnSubWaveEnded.RemoveListener(GameEvents_Wave_OnSubWaveEnded);
            GameEvents.OnCollectedGearAmountChanged.RemoveListener(GameEvents_Item_OnCollectedGearAmountChanged);
            GameEvents.OnWaveLost.RemoveListener(GameEvents_Wave_OnLost);
            GameEvents.OnWaveWon.RemoveListener(GameEvents_Wave_OnWon);
        }

        public override void UpdateState() => CheckSwitchStates();

        public override void CheckSwitchStates() { }

        public override void EnableElement() => Ctx.PlayScreenUITransform.gameObject.SetActive(true);
        
        public override void DisableElement() => Ctx.PlayScreenUITransform.gameObject.SetActive(false);

        private void UpdateGameUIText()
        {

            Ctx.CurrentSubWaveCount.text
                = $"{GameManager.Instance.WaveStateMachine.CurrentSubWaveID + 1} / {GameManager.Instance.WaveStateMachine.CurrentSubWaveIDMax + 1}";

            Ctx.CollectedGearsAmount.text = GameManager.Instance.CollectedGearAmount.ToString();
            
            Ctx.HealthText.text = UpgradeManager.Instance.GetTurretUpgradedStat(Stat.AmountOfHealth).ToString(CultureInfo.CurrentCulture);
        }

        private void GameEvents_Wave_OnWaveStarted() => UpdateGameUIText();
        
        private void GameEvents_Wave_OnWaveEnded()
        {
            UpdateGameUIText();
            SwitchState(Factory.UILobby());
        }

        private void GameEvents_Wave_OnSubWaveStarted() => UpdateGameUIText();

        private void GameEvents_Wave_OnSubWaveEnded() => UpdateGameUIText();
        
        private void GameEvents_Item_OnCollectedGearAmountChanged() =>
            Ctx.CollectedGearsAmount.text = GameManager.Instance.CollectedGearAmount.ToString();

        private void GameEvents_Wave_OnLost() => SwitchState(Factory.UIResults());

        private void GameEvents_Wave_OnWon() => SwitchState(Factory.UIResults());
    }
}
