using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UserLogApi.Models;

namespace UserLogApi.Services
{
    public class LogsService
    {
        private readonly IMongoCollection<UserLog> _userlogsCollection;

        public LogsService(IOptions<UserDatabaseSettings> userDatabaseSettings)
        {
            // Setup Mongo DB client
            var mongoClient = new MongoClient(userDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(userDatabaseSettings.Value.DatabaseName);

            _userlogsCollection = mongoDatabase.GetCollection<UserLog>(userDatabaseSettings.Value.UserLogsCollectionName);
        }

        // Get all userlogs
        public async Task <List<UserLog>> GetAsync() =>
            await _userlogsCollection.Find(_ => true).ToListAsync();

        // Get log by id
        public async Task <UserLog?> GetAsync(string id) =>
            await _userlogsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Create a new log
        public async Task CreateAsync(UserLog newLog) =>
            await _userlogsCollection.InsertOneAsync(newLog);

        // Update a log entry
        public async Task UpdateAsync(string id, UserLogDto log)
        {
            var filter = Builders<UserLog>.Filter.Eq(x => x.Id, id);
            UpdateDefinition<UserLog> update = Builders<UserLog>.Update
            .Set("Title", log.Title)
            .Set("Tags", log.Tags)
            .Push("Content", log.log);
            await _userlogsCollection.UpdateOneAsync(filter, update);
        }

        // Delete a log
        public async Task RemoveAsync(string id)
        {
            var filter = Builders<UserLog>.Filter.Eq(x => x.Id, id);
            await _userlogsCollection.DeleteOneAsync(filter);
            return;
        }

        // Find by tag
        public async Task <List<UserLog>> GetByTagAsync(string tags)
        {
            List <string> tagList = tags.Split(',').ToList();
            var filter = Builders<UserLog>.Filter.AnyIn(x=>x.Tags, tagList);
            return await _userlogsCollection.Find(filter).ToListAsync();
        }

    }
}