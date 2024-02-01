using System;

namespace ImmersiveGalleryAI.Main.Credits
{
    public interface ICredits
    {
        event Action<int> UpdateBalanceEvent;
        event Action UpgradeBalanceEvent;
        
        int GetCreditsBalance();
        void SetCreditsBalance(int credits);
        void SpendCredit();
        void UpgradeBalance();
    }
}