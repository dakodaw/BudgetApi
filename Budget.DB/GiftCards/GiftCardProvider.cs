using BudgetApi.Models;

namespace Budget.DB.GiftCards;

public class GiftCardProvider: IGiftCardProvider
{
	BudgetEntities _db;

	public GiftCardProvider(BudgetEntities db)
	{
		_db = db;
    }

	public GiftCard GetGiftCard(int giftCardId)
	{
		var foundGiftCard = _db.GiftCards
			.Where(i => i.Id == giftCardId)
			.FirstOrDefault();

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

	public IEnumerable<GiftCard> GetAllGiftCards()
	{
		var giftCardList = _db.GiftCards.Select(gc => new GiftCard
		{
			AccessCode = gc.AccessCode,
			InitialAmount = gc.InitialAmount,
			CardNumber = gc.CardNumber,
			Id = gc.Id,
			Place = gc.Place
		});

		return giftCardList;
	}

	public bool AddUpdateGiftCard(GiftCard inputGiftCardStuff, int giftCardId = -1)
	{
        if (inputGiftCardStuff == default)
            return false;

        if (giftCardId == -1)
        {
            try
            {
                var resultingGiftCardId = AddGiftCard(inputGiftCardStuff);

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
                UpdateGiftCard(inputGiftCardStuff);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public int AddGiftCard(GiftCard inputGiftCardStuff)
    {
        if (inputGiftCardStuff == default)
            throw new Exception("Unable to add a null Gift Card");

        var inputGiftCard = new GiftCardEntity
        {
            AccessCode = inputGiftCardStuff.AccessCode,
            InitialAmount = inputGiftCardStuff.InitialAmount,
            CardNumber = inputGiftCardStuff.CardNumber,
            Id = inputGiftCardStuff.Id,
            Place = inputGiftCardStuff.Place
        };

        try
        {
            _db.GiftCards.Add(inputGiftCard);
            _db.SaveChanges();

            return inputGiftCard.Id;
        }
        catch(Exception ex)
        {
            throw new Exception("Failed to Add Gift Card", ex);
        }
    }

    public void UpdateGiftCard(GiftCard inputGiftCard)
    {
        try
        {
            var existingGiftCard = _db.GiftCards.FirstOrDefault(x => x.Id == inputGiftCard.Id);
            existingGiftCard.AccessCode = inputGiftCard.AccessCode;
            existingGiftCard.CardNumber = inputGiftCard.CardNumber;
            existingGiftCard.InitialAmount = inputGiftCard.InitialAmount;
            existingGiftCard.Place = inputGiftCard.Place;
            _db.SaveChanges();
        }
        catch(Exception ex)
        {
            throw new Exception($"Failed to update gift card {inputGiftCard?.Id}", ex);
        }
    }

    public bool DeleteGiftCardEntry(int giftCardId)
    {
        try
        {
            var toDelete = _db.GiftCards.Find(giftCardId);
            _db.GiftCards.Remove(toDelete);
            _db.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}

