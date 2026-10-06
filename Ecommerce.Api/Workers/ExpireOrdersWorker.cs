
using Ecommerce.Api.Repositories.Interfaces;

namespace Ecommerce.Api.Workers
{
    public class ExpireOrdersWorker : BackgroundService
    {
        private readonly ILogger<ExpireOrdersWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ExpireOrdersWorker(ILogger<ExpireOrdersWorker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var repository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                        var count = repository.ExpireUnpaidOrders();
                        _logger.LogInformation("Expired " + count + " orders");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An error occurred while updating expired orders");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}