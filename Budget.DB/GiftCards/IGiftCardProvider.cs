using BudgetApi.Models;

namespace Budget.DB.GiftCards;

public interface IGiftCardProvider
{
	GiftCard GetGiftCard(int giftCardId);
	IEnumerable<GiftCard> GetAllGiftCards(int groupId);
	bool AddUpdateGiftCard(int groupId, GiftCard inputGiftCard, int giftCardId = -1);
	int AddGiftCard(int groupId, GiftCard inputGiftCard);
	void UpdateGiftCard(GiftCard inputGiftCard);
    void DeleteGiftCardEntry(int giftCardId);
}

