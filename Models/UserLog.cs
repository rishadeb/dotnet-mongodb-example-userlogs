using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserLogApi.Models
{
    public class Content
    {
        public string User {get; set;} = null!;
        public DateTime? Updated {get; set;}
        public string Log {get; set;} = null!;
    }
    
    [BsonIgnoreExtraElements()]
    public class UserLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id {get; set;}
        public string Title {get; set;} = null!;
        public string User { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? StopDate { get; set; }

        [BsonElement("items")]
        public List<string> Tags { get; set; } = null!;
        public List<Content> Content {get; set;} = null!;

    }

    public class UserLogInput
    {
        public string Title {get; set;} = null!;
        public string User { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? StopDate { get; set; }
        public List<string> Tags { get; set; } = null!;
        public string logMessage {get; set;} = null!;
    }
    public class UserLogDto
    {
        public string? Title {get; set;} = null!;
        public List<string> Tags {get; set; } = null!;
        public Content log {get; set;} = null!;

    }
}