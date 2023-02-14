using Application.Common.Interfaces;
using Application.CreditCardProviders.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.CreditCardProviders.Queries
{
    public record GetCreditCardProvidersQuery : IRequest<List<CreditCardProviderDTO>>;

    public class GetCreditCardProvidersQueryHandler : IRequestHandler<GetCreditCardProvidersQuery, List<CreditCardProviderDTO>>
    {
        private readonly IGenericRepository<CreditCardProvider> _creditCardProviderRepository;

        public GetCreditCardProvidersQueryHandler(IGenericRepository<CreditCardProvider> creditCardProviderRepository)
        {
            _creditCardProviderRepository = creditCardProviderRepository;
        }

        public async Task<List<CreditCardProviderDTO>> Handle(GetCreditCardProvidersQuery request, CancellationToken cancellationToken)
        {
            return await _creditCardProviderRepository.GetAll()
                .Select(x => new CreditCardProviderDTO() { 
                    Id = x.Id,
                    Name = x.Name, 
                    CardNumberRegEx = x.CardNumberRegEx,
                    LastModified = x.LastModified
                })
                .ToAsyncEnumerable()
                .ToListAsync(cancellationToken);
        }
    }
}
