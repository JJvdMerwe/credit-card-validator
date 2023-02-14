using Application.Common.Interfaces;
using Application.CreditCardProviders.DTOs;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditCardProviders.Commands
{
    public record CreateCreditCardProviderCommand(CreditCardProviderDTO Data) : IRequest<int>;

    public class CreateCreditCardProviderCommandHandler : IRequestHandler<CreateCreditCardProviderCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<CreditCardProvider> _creditCardProviderRepository;

        public CreateCreditCardProviderCommandHandler(
            IUnitOfWork unitOfWork,
            IGenericRepository<CreditCardProvider> creditCardProviderRepository)
        {
            _unitOfWork = unitOfWork;
            _creditCardProviderRepository = creditCardProviderRepository;
        }

        public async Task<int> Handle(CreateCreditCardProviderCommand request, CancellationToken cancellationToken)
        {
            var provider = new CreditCardProvider
            {
                CardNumberRegEx = request.Data.CardNumberRegEx,
                Name = request.Data.Name,
                LastModified = DateTime.UtcNow,
            };

            _creditCardProviderRepository.Add(provider);

            return await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
