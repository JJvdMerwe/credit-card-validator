using Application.Common.Interfaces;
using Application.CreditCards.DTOs;
using MediatR;

namespace Application.CreditCards.Queries
{
    public record GetCreditCardsQuery : IRequest<List<CreditCardDTO>>;

    public class GetCreditCardsQueryHandler : IRequestHandler<GetCreditCardsQuery, List<CreditCardDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCreditCardsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CreditCardDTO>> Handle(GetCreditCardsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.CreditCardRepository.GetAll()
                .Select(x => new CreditCardDTO()
                {
                    Number = x.Number,
                    ProviderName = x.Provider.Name,
                    DateCreated = x.DateCreated
                })
                .ToListAsync(cancellationToken);
        }
    }
}
