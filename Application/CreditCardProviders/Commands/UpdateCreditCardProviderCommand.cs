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

        public UpdateCreditCardProviderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            _unitOfWork.CreditCardProviderRepository.Update(provider);
            return await _unitOfWork.SaveChangesAsync();
        }
    }

}
