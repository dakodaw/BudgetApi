using BudgetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.DB.GiftCards;

public class GiftCardProvider: IGiftCardProvider
{
	BudgetEntities _db;

	public GiftCardProvider(BudgetEntities db)
	{
		_db = db;
    }

	public async Task<GiftCard> GetGiftCard(int giftCardId)
	{
		var foundGiftCard = await _db.GiftCards
			.Where(i => i.Id == giftCardId)
			.FirstOrDefaultAsync();

		if (foundGiftCard == default)
			return new GiftCard();

		return new GiftCard
		{
			AccessCode = foundGiftCard.AccessCode,
			InitialAmount = foundGiftCard.InitialAmount,
			CardNumber = foundGiftCard.CardNumber,
			Id = foundGiftCard.Id,
			Place = foundGiftCard.Place
		};
    }

	public async Task<IEnumerable<GiftCard>> GetAllGiftCards(int groupId)
	{
		var giftCardList = await _db.GiftCards.Where(x => x.BudgetingGroupId == groupId)
            .Select(gc => new GiftCard
		    {
			    AccessCode = gc.AccessCode,
			    InitialAmount = gc.InitialAmount,
			    CardNumber = gc.CardNumber,
			    Id = gc.Id,
			    Place = gc.Place
		    }).ToListAsync();

		return giftCardList;
	}

	public async Task<bool> AddUpdateGiftCard(int groupId, GiftCard inputGiftCardStuff, int giftCardId = -1)
	{
        if (inputGiftCardStuff == default)
            return false;

        if (giftCardId == -1)
        {
            try
            {
                var resultingGiftCardId = await AddGiftCard(groupId, inputGiftCardStuff);

                return true;
            }
            catch
            {
                return false;
            }
        }
        else
        {
            try
            {
                await UpdateGiftCard(inputGiftCardStuff);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public async Task<int> AddGiftCard(int groupId, GiftCard inputGiftCardStuff)
    {
        if (inputGiftCardStuff == default)
            throw new Exception("Unable to add a null Gift Card");

        var inputGiftCard = new GiftCardEntity
        {
            AccessCode = inputGiftCardStuff.AccessCode,
            InitialAmount = inputGiftCardStuff.InitialAmount,
            CardNumber = inputGiftCardStuff.CardNumber,
            Id = inputGiftCardStuff.Id,
            Place = inputGiftCardStuff.Place,
            BudgetingGroupId = groupId
        };

        try
        {
            await _db.GiftCards.AddAsync(inputGiftCard);
            await _db.SaveChangesAsync();

            return inputGiftCard.Id;
        }
        catch(Exception ex)
        {
            throw new Exception("Failed to Add Gift Card", ex);
        }
    }

    public async Task UpdateGiftCard(GiftCard inputGiftCard)
    {
        try
        {
            var existingGiftCard = await _db.GiftCards.FirstOrDefaultAsync(x => x.Id == inputGiftCard.Id);
            existingGiftCard.AccessCode = inputGiftCard.AccessCode;
            existingGiftCard.CardNumber = inputGiftCard.CardNumber;
            existingGiftCard.InitialAmount = inputGiftCard.InitialAmount;
            existingGiftCard.Place = inputGiftCard.Place;
            await _db.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            throw new Exception($"Failed to update gift card {inputGiftCard?.Id}", ex);
        }
    }

    public async Task DeleteGiftCardEntry(int giftCardId)
    {
        try
        {
            var toDelete = await _db.GiftCards.FindAsync(giftCardId);
            _db.GiftCards.Remove(toDelete);
            await _db.SaveChangesAsync();
        }
        catch(Exception e) 
        {
            throw new Exception("Failed to Delete Gift Card Entry", e);
        }
    }
}

