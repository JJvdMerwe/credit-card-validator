using Application.CreditCardProviders.Queries;
using Application.CreditCards.Commands;
using Application.CreditCards.DTOs;
using Application.CreditCards.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace CreditCardValidator.Controllers
{
    public class CreditCardsController : Controller
    {
        private readonly IMediator _mediator;

        public CreditCardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: CreditCardProvidersController
        public async Task<IActionResult> Index(string searchString, CancellationToken cancellationToken)
        {
            var cardDTOs = await _mediator.Send(new GetCreditCardsQuery(searchString), cancellationToken);

            return View(cardDTOs);
        }

        // GET: CreditCardProvidersController/Create
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var providerDTOs = await _mediator.Send(new GetCreditCardProvidersQuery(), cancellationToken);
            ViewBag.ProviderDTOs = providerDTOs;

            return View();
        }

        // POST: CreditCardProvidersController/Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit([Bind("Number")] CreditCardDTO creditCardDTO, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(creditCardDTO.Number))
                {
                    var result = await _mediator.Send(new SubmitCreditCardCommand(creditCardDTO), cancellationToken);

                    if (result.IsSuccess)
                    {
                        ViewBag.SuccessMessage = result.Message;
                    } 
                    else
                    {
                        ModelState.AddModelError("Number", result.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("Number", "The Number field is required.");
                }
            }

            var providerDTOs = await _mediator.Send(new GetCreditCardProvidersQuery(), cancellationToken);
            ViewBag.ProviderDTOs = providerDTOs;

            return View(creditCardDTO);
        }
    }
}
