using Application.CreditCardProviders.Commands;
using Application.CreditCardProviders.DTOs;
using Application.CreditCardProviders.Queries;
using Infrastructure.Persistence.EF;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Index()
        {
            var providerDTOs = await _mediator.Send(new GetCreditCardProvidersQuery());

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
        public async Task<IActionResult> Create([Bind("Name,CardNumberRegEx")] CreditCardProviderDTO provider)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new CreateCreditCardProviderCommand(provider));

                return RedirectToAction(nameof(Index));
            }
            return View(provider);
        }

        // GET: CreditCardProvidersController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providerDTO = await _mediator.Send(new GetCreditCardProviderQuery(id.Value));

            if (providerDTO == null)
            {
                return NotFound();
            }

            return View(providerDTO);
        }

        // POST: CreditCardProvidersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CardNumberRegEx")] CreditCardProviderDTO provider)
        {
            if (id != provider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(new UpdateCreditCardProviderCommand(provider));
                }
                catch (DbUpdateConcurrencyException)
                {
                    provider = await _mediator.Send(new GetCreditCardProviderQuery(id));

                    if (provider == null)
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

            return View(provider);
        }

        // GET: CreditCardProvidersController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providerDTO = await _mediator.Send(new GetCreditCardProviderQuery(id.Value));

            if (providerDTO == null)
            {
                return NotFound();
            }

            return View(providerDTO);
        }

        // POST: CreditCardProvidersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCreditCardProviderCommand(id));

            return RedirectToAction(nameof(Index));
        }
    }
}
