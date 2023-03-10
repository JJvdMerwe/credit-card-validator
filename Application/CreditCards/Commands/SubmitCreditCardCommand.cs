using Application.Common.Interfaces;
using Application.CreditCards.DTOs;
using Application.CreditCards.Results;
using Application.CreditCards.Utilities;
using Domain.Entities;
using MediatR;

namespace Application.CreditCards.Commands
{
    public record SubmitCreditCardCommand(CreditCardDTO Data) : IRequest<SubmitCreditCardResult>;

    public class SubmitCreditCardCommandHandler : IRequestHandler<SubmitCreditCardCommand, SubmitCreditCardResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<CreditCardProvider> _creditCardProviderRepository;
        private readonly IGenericRepository<CreditCard> _creditCardRepository;

        public SubmitCreditCardCommandHandler(
            IUnitOfWork unitOfWork,
            IGenericRepository<CreditCardProvider> creditCardProviderRepository,
            IGenericRepository<CreditCard> creditCardRepository
            )
        {
            _unitOfWork = unitOfWork;
            _creditCardProviderRepository = creditCardProviderRepository;
            _creditCardRepository = creditCardRepository;
        }

        public async Task<SubmitCreditCardResult> Handle(SubmitCreditCardCommand request, CancellationToken cancellationToken)
        {
            LuhnValidator luhnValidator = new LuhnValidator();

            if (!luhnValidator.Validate(request.Data.Number))
            {
                return new SubmitCreditCardResult
                {
                    IsSuccess = false,
                    Message = "This number failed the Luhn Check. It is not a valid credit card number."
                };
            }

            if (await CreditCardExists(request.Data.Number, cancellationToken))
            {
                return new SubmitCreditCardResult
                {
                    IsSuccess = false,
                    Message = "This number has already been submitted."
                };
            }

            var providers = await _creditCardProviderRepository.GetAll()
                .ToAsyncEnumerable()
                .ToListAsync(cancellationToken);
            Dictionary<int, string> regularExpressions = providers.ToDictionary(x => x.Id, x => x.CardNumberRegEx);

            var regExValidator = new RegExValidator();
            var providerIds = regExValidator.Validate(regularExpressions, request.Data.Number);

            if (providerIds.Count() == 0 || providerIds.Count() > 1)
            {
                var result = new SubmitCreditCardResult
                {
                    IsSuccess = false
                };

                if (providerIds.Count() == 0)
                {
                    result.Message = "The number does not match any of the configured providers.";
                }
                else
                {
                    result.Message = "Multiple providers match this number. Please check your provider configuration.";
                }

                return result;
            }

            var matchedProviderId = providerIds.First();
            var matchedProvider = providers
                                    .Where(x => x.Id == matchedProviderId)
                                    .First();

            var creditCard = new CreditCard
            {
                DateCreated = DateTime.UtcNow,
                Number = request.Data.Number,
                Provider = matchedProvider
            };

            _creditCardRepository.Add(creditCard);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var providerName = matchedProvider.Name;

            return new SubmitCreditCardResult
            {
                IsSuccess = true,
                Message = $"{providerName} card number successfully submitted."
            };
        }

        private async Task<bool> CreditCardExists(string number, CancellationToken cancellationToken)
        {
            return await _creditCardRepository.GetAll()
                .ToAsyncEnumerable()
                .AnyAsync(x => x.Number == number, cancellationToken);
        }
    }
}
