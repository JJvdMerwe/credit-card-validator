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

        public CreateCreditCardProviderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateCreditCardProviderCommand request, CancellationToken cancellationToken)
        {
            var provider = new CreditCardProvider
            {
                CardNumberRegEx = request.Data.CardNumberRegEx,
                Name = request.Data.Name,
                LastModified = DateTime.UtcNow,
            };

            _unitOfWork.CreditCardProviderRepository.Add(provider);

            return await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
