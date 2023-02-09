using Application.CreditCards.Commands;
using Application.CreditCards.DTOs;
using Application.CreditCards.Queries;
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
        public async Task<ActionResult> Index()
        {
            var creditCardProviders = await _mediator.Send(new GetCreditCardProvidersQuery());

            return View(creditCardProviders);
        }

        // GET: CreditCardProvidersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CreditCardProvidersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreditCardProvidersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CreditCardProvidersController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var provider = await _mediator.Send(new GetCreditCardProviderQuery(id.Value));

            if (provider == null)
            {
                return NotFound();
            }

            return View(provider);
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CreditCardProvidersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
