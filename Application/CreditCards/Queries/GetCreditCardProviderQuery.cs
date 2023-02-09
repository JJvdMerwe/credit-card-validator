using Application.Common.Interfaces;
using Application.CreditCards.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditCards.Queries
{
    public record GetCreditCardProviderQuery(int Id) : IRequest<CreditCardProviderDTO>;

    public class GetCreditCardProviderQueryHandler : IRequestHandler<GetCreditCardProviderQuery, CreditCardProviderDTO?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCreditCardProviderQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreditCardProviderDTO?> Handle(GetCreditCardProviderQuery request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.CreditCardProviderRepository.GetByIdAsync(request.Id);

            if (entity == null)
            {
                return null;
            }

            return new CreditCardProviderDTO()
                        {
                            Id = entity.Id,
                            Name = entity.Name,
                            CardNumberRegEx = entity.CardNumberRegEx,
                            LastModified = entity.LastModified
                        };
        }
    }
}
