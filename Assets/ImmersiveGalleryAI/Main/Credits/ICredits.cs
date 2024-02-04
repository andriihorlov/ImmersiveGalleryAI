using System;

namespace ImmersiveGalleryAI.Main.Credits
{
    public interface ICredits
    {
        bool IsOwnCredits { get; }
        
        event Action<int> UpdateBalanceEvent;
        event Action UpgradeBalanceEvent;
        event Action NoCreditsLeftEvent;

        void SetCreditType(bool isOwn);
        int GetCreditsBalance();
        void SetCreditsBalance(int credits);
        void SpendCredit();
        void UpgradeBalance();
    }
}