using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditCardProviders.Commands
{
    public record DeleteCreditCardProviderCommand(int Id) : IRequest<int>;

    public class DeleteCreditCardProviderCommandHandler : IRequestHandler<DeleteCreditCardProviderCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCreditCardProviderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteCreditCardProviderCommand request, CancellationToken cancellationToken)
        {
            var provider = new CreditCardProvider { Id = request.Id };

            _unitOfWork.CreditCardProviderRepository.Delete(provider);

            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
