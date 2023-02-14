using Application.Common.Interfaces;
using Application.CreditCardProviders.DTOs;
using Application.CreditCardProviders.Queries;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditCardProviders.Commands
{
    public record UpdateCreditCardProviderCommand(CreditCardProviderDTO Data) : IRequest<int>;

    public class UpdateCreditCardProviderCommandHandler : IRequestHandler<UpdateCreditCardProviderCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<CreditCardProvider> _creditCardProviderRepository;

        public UpdateCreditCardProviderCommandHandler(
            IUnitOfWork unitOfWork,
            IGenericRepository<CreditCardProvider> creditCardProviderRepository)
        {
            _unitOfWork = unitOfWork;
            _creditCardProviderRepository = creditCardProviderRepository;
        }

        public async Task<int> Handle(UpdateCreditCardProviderCommand request, CancellationToken cancellationToken)
        {
            var provider = new CreditCardProvider
            {
                Id = request.Data.Id,
                CardNumberRegEx = request.Data.CardNumberRegEx,
                LastModified = DateTime.UtcNow,
                Name = request.Data.Name,
            };

            _creditCardProviderRepository.Update(provider);
            return await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

}
