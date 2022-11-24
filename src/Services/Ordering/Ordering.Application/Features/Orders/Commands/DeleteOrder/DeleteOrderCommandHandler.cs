using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private IOrderRepository _orderRepository;
        private IMapper _mapper;
        private ILogger<DeleteOrderCommandHandler> _logger;
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToDelete == null)
                _logger.LogError("Order Doesn't exist");

           await _orderRepository.DeleteAsync(orderToDelete);
            _logger.LogInformation($"Order {request.Id} Has Been deleted Successfully");

           return Unit.Value;
        }
    }
}
