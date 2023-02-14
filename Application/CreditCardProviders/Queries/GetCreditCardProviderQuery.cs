using Application.Common.Interfaces;
using Application.CreditCardProviders.DTOs;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditCardProviders.Queries
{
    public record GetCreditCardProviderQuery(int Id) : IRequest<CreditCardProviderDTO>;

    public class GetCreditCardProviderQueryHandler : IRequestHandler<GetCreditCardProviderQuery, CreditCardProviderDTO?>
    {
        private readonly IGenericRepository<CreditCardProvider> _creditCardProviderRepository;

        public GetCreditCardProviderQueryHandler(IGenericRepository<CreditCardProvider> creditCardProviderRepository)
        {
            _creditCardProviderRepository = creditCardProviderRepository;
        }

        public async Task<CreditCardProviderDTO?> Handle(GetCreditCardProviderQuery request, CancellationToken cancellationToken)
        {
            var entity = await _creditCardProviderRepository.GetByIdAsync(request.Id, cancellationToken);

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
