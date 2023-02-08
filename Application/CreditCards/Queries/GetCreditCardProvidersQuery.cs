using Application.Common.Interfaces;
using Application.CreditCards.DTOs;
using MediatR;

namespace Application.CreditCards.Queries
{
    public record GetCreditCardProvidersQuery : IRequest<List<CreditCardProviderDTO>>;

    public class GetCreditCardProvidersQueryHandler : IRequestHandler<GetCreditCardProvidersQuery, List<CreditCardProviderDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCreditCardProvidersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CreditCardProviderDTO>> Handle(GetCreditCardProvidersQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.CreditCardProviderRepository.GetAll().
                Select(x => new CreditCardProviderDTO() { 
                    Name = x.Name, 
                    CardNumberRegEx = x.CardNumberRegEx })
                .ToListAsync();
        }
    }
}
