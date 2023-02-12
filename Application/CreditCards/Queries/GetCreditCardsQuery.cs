using Application.Common.Interfaces;
using Application.CreditCardProviders.DTOs;
using Application.CreditCards.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var list = await _unitOfWork.CreditCardRepository.GetAll().ToListAsync();

            return await _unitOfWork.CreditCardRepository.GetAll()
                .Select(x => new CreditCardDTO()
                {
                    Number = x.Number,
                    ProviderName = x.Provider.Name,
                    DateCreated = x.DateCreated
                })
                .ToListAsync();
        }
    }
}
