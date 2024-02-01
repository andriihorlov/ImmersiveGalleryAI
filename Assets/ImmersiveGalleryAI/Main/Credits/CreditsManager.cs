using System;

namespace ImmersiveGalleryAI.Main.Credits
{
    public class CreditsManager : ICredits
    {
        public event Action<int> UpdateBalanceEvent;
        public event Action UpgradeBalanceEvent;

        private int _creditsBalance;
        
        public int GetCreditsBalance() => _creditsBalance;

        public void SetCreditsBalance(int credits)
        {
            _creditsBalance = credits;
        }

        public void SpendCredit()
        {
            _creditsBalance--;
            UpdateBalanceEvent?.Invoke(_creditsBalance);
        }

        public void UpgradeBalance()
        {
            UpgradeBalanceEvent?.Invoke();
        }
    }
}