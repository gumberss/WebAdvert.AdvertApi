using System;
using System.Threading.Tasks;
using AdvertApi.Models;
using AutoMapper;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace AdvertApi.Services
{
    public class DynamoDBAdvertStorage : IAdvertStorageService
    {
        private readonly IMapper _mapper;
        private readonly IAmazonDynamoDB _client;

        public DynamoDBAdvertStorage(IMapper mapper, IAmazonDynamoDB client)
        {
            _mapper = mapper;
            _client = client;
        }

        public async Task<string> Add(AdvertModel model)
        {
            var dbModel = _mapper.Map<AdvertDbModel>(model);
            dbModel.Id = Guid.NewGuid().ToString();
            dbModel.CreationDateTime = DateTime.UtcNow;
            dbModel.Status = AdvertStatus.Pending;

            using (var client = new AmazonDynamoDBClient())
            using (var context = new DynamoDBContext(client))
            {
                await context.SaveAsync(dbModel);
            }

            return dbModel.Id;
        }

        public async Task<bool> CheckHealthAsync()
        {
            using (var context = new DynamoDBContext(_client))
            {
                var tableData = await _client.DescribeTableAsync("Adverts");

                return String.Compare(tableData.Table.TableStatus, "Active", true) == 0;
            }
        }

        public async Task Confirm(ConfirmAdvertModel model)
        {
            
            using (var context = new DynamoDBContext(_client))
            {
                var record = await context.LoadAsync<AdvertDbModel>(model.Id);

                if (record == null)
                {
                    throw new KeyNotFoundException($"A record with id = {model.Id} was not found");
                }

                if (model.Status == AdvertStatus.Active)
                {
                    record.Status = AdvertStatus.Active;
                    await context.SaveAsync(record);
                }
                else
                {
                    await context.DeleteAsync(record);
                }
            }
        }
    }
}
