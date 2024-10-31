using System;

namespace ImmersiveGalleryAI.Main.Credits
{
    public class CreditsManager : ICredits
    {
        public event Action<int> UpdateBalanceEvent;
        public event Action RequestUpgradeBalanceEvent;
        public event Action NoCreditsLeftEvent;

        private int _creditsBalance;
        public bool IsOwnCredits { get; private set; }

        public void SetCreditType(bool isOwn)
        {
            IsOwnCredits = isOwn;
        }

        public int GetCreditsBalance() => _creditsBalance;

        public void SetCreditsBalance(int credits)
        {
            _creditsBalance = credits;
            UpdateBalanceEvent?.Invoke(_creditsBalance);
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

        public void RequestUpgradeBalance()
        {
            RequestUpgradeBalanceEvent?.Invoke();
        }
    }
}