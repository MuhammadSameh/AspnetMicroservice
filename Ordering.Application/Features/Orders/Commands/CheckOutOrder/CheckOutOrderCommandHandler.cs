using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Application.Models;

namespace Ordering.Application.Features.Orders.Commands
{
    public class CheckOutOrderCommandHandler : IRequestHandler<CheckOutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckOutOrderCommandHandler> _logger;

        public CheckOutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckOutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CheckOutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var orderResult = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"new Order Added {orderResult.Id}");
            await SendMail(orderResult);
            return orderResult.Id;
        }

        private async Task SendMail(Order orderResult)
        {
            var email = new Email { To = "muhammadsameh970@gmail.com", Subject = "New Order", Body = "Order Was Created" };
            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something Went Wrong With Email Service with Order: {orderResult.Id}, Exception: {ex.Message} ");
            }
        }
    }
}
