using BudgetApi.Models;

namespace Budget.DB.GiftCards;

public interface IGiftCardProvider
{
	GiftCard GetGiftCard(int giftCardId);
	IEnumerable<GiftCard> GetAllGiftCards();
	bool AddUpdateGiftCard(GiftCard inputGiftCard, int giftCardId = -1);
	bool DeleteGiftCardEntry(int giftCardId);
}

