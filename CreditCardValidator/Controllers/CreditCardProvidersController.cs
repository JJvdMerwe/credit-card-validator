using Application.CreditCardProviders.Commands;
using Application.CreditCardProviders.DTOs;
using Application.CreditCardProviders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreditCardValidator.Controllers
{
    public class CreditCardProvidersController : Controller
    {
        private readonly IMediator _mediator;

        public CreditCardProvidersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: CreditCardProvidersController
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var providerDTOs = await _mediator.Send(new GetCreditCardProvidersQuery(), cancellationToken);

            return View(providerDTOs);
        }

        // GET: CreditCardProvidersController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CreditCardProvidersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CardNumberRegEx")] CreditCardProviderDTO providerDTO, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new CreateCreditCardProviderCommand(providerDTO), cancellationToken);

                return RedirectToAction(nameof(Index));
            }

            return View(providerDTO);
        }

        // GET: CreditCardProvidersController/Edit/5
        public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providerDTO = await _mediator.Send(new GetCreditCardProviderQuery(id.Value), cancellationToken);

            if (providerDTO == null)
            {
                return NotFound();
            }

            return View(providerDTO);
        }

        // POST: CreditCardProvidersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CardNumberRegEx")] CreditCardProviderDTO providerDTO, CancellationToken cancellationToken)
        {
            if (id != providerDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(new UpdateCreditCardProviderCommand(providerDTO), cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    providerDTO = await _mediator.Send(new GetCreditCardProviderQuery(id), cancellationToken);

                    if (providerDTO == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(providerDTO);
        }

        // GET: CreditCardProvidersController/Delete/5
        public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providerDTO = await _mediator.Send(new GetCreditCardProviderQuery(id.Value), cancellationToken);

            if (providerDTO == null)
            {
                return NotFound();
            }

            return View(providerDTO);
        }

        // POST: CreditCardProvidersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteCreditCardProviderCommand(id), cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
