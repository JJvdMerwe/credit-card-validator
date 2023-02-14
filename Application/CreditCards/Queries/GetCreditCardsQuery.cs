using Application.Common.Interfaces;
using Application.CreditCards.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.CreditCards.Queries
{
    public record GetCreditCardsQuery : IRequest<List<CreditCardDTO>>;

    public class GetCreditCardsQueryHandler : IRequestHandler<GetCreditCardsQuery, List<CreditCardDTO>>
    {
        private readonly IGenericRepository<CreditCard> _creditCardRepository;

        public GetCreditCardsQueryHandler(IGenericRepository<CreditCard> creditCardRepository)
        {
            _creditCardRepository = creditCardRepository;
        }

        public async Task<List<CreditCardDTO>> Handle(GetCreditCardsQuery request, CancellationToken cancellationToken)
        {
            return await _creditCardRepository.GetAll()
                .Select(x => new CreditCardDTO()
                {
                    Number = x.Number,
                    ProviderName = x.Provider.Name,
                    DateCreated = x.DateCreated
                })
                .ToAsyncEnumerable()
                .ToListAsync(cancellationToken);
        }
    }
}
