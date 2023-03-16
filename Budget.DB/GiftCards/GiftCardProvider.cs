using BudgetApi.Models;

namespace Budget.DB.GiftCards;

public class GiftCardProvider
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

        var inputGiftCard = new GiftCardEntity
        {
            AccessCode = inputGiftCardStuff.AccessCode,
            InitialAmount = inputGiftCardStuff.InitialAmount,
            CardNumber = inputGiftCardStuff.CardNumber,
            Id = inputGiftCardStuff.Id,
            Place = inputGiftCardStuff.Place
        };

        if (giftCardId == -1)
        {
            bool success = false;
            _db.GiftCards.Add(inputGiftCard);
            _db.SaveChanges();
            try
            {
                var checkCard = _db.GiftCards.Where(i => i.CardNumber == inputGiftCard.CardNumber).FirstOrDefault();
                success = true;
            }
            catch
            {

            }
            return success;
        }
        else
        {
            bool success = false;

            try
            {
                _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault().AccessCode = inputGiftCard.AccessCode;
                _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault().CardNumber = inputGiftCard.CardNumber;
                _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault().InitialAmount = inputGiftCard.InitialAmount;
                _db.GiftCards.Where(i => i.Id == giftCardId).FirstOrDefault().Place = inputGiftCard.Place;
                _db.SaveChanges();
                success = true;
            }
            catch
            {

            }
            return success;
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

