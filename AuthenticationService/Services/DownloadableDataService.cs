using AuthenticationService.Data;
using AuthenticationService.Data.Messages;
using AuthenticationService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public sealed class DownloadableDataService : IDownloadableDataService
    {
        private readonly ApplicationDbContext db;
        private readonly ILogger<RabbitmqUserPublisher> logger;

        public DownloadableDataService(ApplicationDbContext db, ILogger<RabbitmqUserPublisher> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task<List<DownloadablePersonalData>> GetData(string username)
        {
            var data = await this.db.DownloadablePersonalDatas
                .Where(d => d.ApplicationUserName == username)
                .ToListAsync();

            var toDelete = data.GroupBy(d => d.DomainName)
                .SelectMany(g => g.OrderByDescending(d => d.RecordedAt).Skip(1));

            this.db.RemoveRange(toDelete);

            await this.db.SaveChangesAsync();

            return data.GroupBy(d => d.DomainName)
                .Select(g => g.OrderByDescending(d => d.RecordedAt).First())
                .ToList();
        }

        public Task HandleAsyncEvent(object sender, BasicDeliverEventArgs args)
        {
            var message = Encoding.UTF8.GetString(args.Body.ToArray());
            return args.RoutingKey switch
            {
                "downloadable-personal-data.new" => AddData(),
                _ => Default(),
            };

            async Task AddData()
            {
                var newData = JsonConvert.DeserializeObject<DownloadablePersonalDataIM>(message)
                    ?? throw new InvalidOperationException($"Could not deserialize {typeof(DownloadablePersonalDataIM)} from {message}");

                var profile = new DownloadablePersonalData(newData);

                this.db.DownloadablePersonalDatas.Add(profile);

                await this.db.SaveChangesAsync();
            }

            Task Default()
            {
                this.logger.LogWarning("Could not handle message" +
                                        $" with routing key {args.RoutingKey} and body {message}");
                return Task.CompletedTask;
            }
        }
    }
}