using BudgetApi.Models;

namespace Budget.DB.GiftCards;

public interface IGiftCardProvider
{
	Task<GiftCard> GetGiftCard(int giftCardId);
	Task<IEnumerable<GiftCard>> GetAllGiftCards(int groupId);
	Task<bool> AddUpdateGiftCard(int groupId, GiftCard inputGiftCard, int giftCardId = -1);
	Task<int> AddGiftCard(int groupId, GiftCard inputGiftCard);
	Task UpdateGiftCard(GiftCard inputGiftCard);
    Task DeleteGiftCardEntry(int giftCardId);
}

