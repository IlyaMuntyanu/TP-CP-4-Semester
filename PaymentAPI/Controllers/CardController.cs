using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Data;
using PaymentAPI.Models;
using PaymentAPI.RequestBodies;

namespace PaymentAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CardController : ControllerBase
{
    private readonly ApiDbContext _db;

    public CardController(ApiDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult> OpenCard()
    {
        var random = new Random();
        var cardNumber = random.NextInt64(9000000000000000, 9999999999999999);
        var cvc = random.Next(100, 999);

        var currentDate = DateTime.Now;
        var cardStatus = await _db.CardStatus.FirstAsync(cs => cs.Name == "Открыта");

        var card = new Card
        {
            CardNumber = cardNumber,
            Cvc = cvc,
            ValidThroughYear = currentDate.Year + 6,
            ValidThroughMonth = currentDate.Month,
            CardStatus = cardStatus
        };

        await _db.Cards.AddAsync(card);
        await _db.SaveChangesAsync();

        return Created("/Card", card);
    }

    [HttpPost]
    [Route("Verify")]
    public async Task<ActionResult> VerifyCard(CardBody body)
    {
        var card = await _db.Cards.FirstOrDefaultAsync(c => c.CardNumber == body.CardNumber);

        if (card == null ||
            card.ValidThroughYear != body.ValidThroughYear ||
            card.ValidThroughMonth != body.ValidThroughMonth ||
            card.Cvc != body.Cvc)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpPost]
    [Route("Replenish")]
    public async Task<ActionResult> ReplenishCard(ReplenishBody body)
    {
        var card = await _db.Cards.FirstOrDefaultAsync(c => c.CardNumber == body.CardNumber);

        if (card == null)
        {
            return NotFound();
        }

        card.Balance += body.Sum;

        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Route("Transfer")]
    public async Task<ActionResult> Transfer(TransferBody body)
    {
        var senderCard = await _db.Cards.FirstOrDefaultAsync(c => c.CardNumber == body.FromCardNumber);
        var recieverCard = await _db.Cards.FirstOrDefaultAsync(c => c.CardNumber == body.ToCardNumber);
        var openStatus = await _db.CardStatus.FirstOrDefaultAsync(c => c.Name == "Открыта");

        if (senderCard == null || recieverCard == null)
        {
            return NotFound();
        }

        if (senderCard.Balance < body.Sum)
        {
            return Conflict();
        }

        if (senderCard.ValidThroughMonth != body.FromValidThroughMonth ||
            senderCard.ValidThroughYear != body.FromValidThroughYear ||
            senderCard.Cvc != body.FromCvc ||
            senderCard.CardStatus != openStatus) return Problem();
        
        senderCard.Balance -= body.Sum;
        recieverCard.Balance += body.Sum;

        await _db.SaveChangesAsync();

        return Ok();
    }
}