using System;

namespace ImmersiveGalleryAI.Main.Credits
{
    public class CreditsManager : ICredits
    {
        public event Action<int> UpdateBalanceEvent;
        public event Action UpgradeBalanceEvent;
        public event Action NoCreditsLeftEvent;

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

            if (_creditsBalance == 0)
            {
                NoCreditsLeftEvent?.Invoke();
            }
        }

        public void UpgradeBalance()
        {
            UpgradeBalanceEvent?.Invoke();
        }
    }
}