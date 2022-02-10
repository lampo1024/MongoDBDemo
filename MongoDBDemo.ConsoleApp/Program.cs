using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDBDemo.ConsoleApp.Models;

//var serializer = new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime);
//BsonSerializer.RegisterSerializer(serializer);
BsonSerializer.RegisterSerializer(typeof(DateTime), new BsonUtcDateTimeSerializer());
var dbName = "data_center";
var connectionString = "mongodb+srv://rector:ROvUDRfP4ZBqW5z4@cluster0.inudy.azure.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";
var client = new MongoClient(connectionString);

var databases = client.ListDatabaseNames().ToList();
foreach (var database in databases)
{
    Console.WriteLine(database);
}

var dcCollection = client.GetDatabase(dbName).GetCollection<User>("dc_user");
//var db = client.GetDatabase(dbName);
var random = new Random();
var count = 0L;
CreateUser();
FindAllUsers();

UpdateUser();
FindAllUsers();

ReplaceUser();
FindAllUsers();

ClearUser();
FindAllUsers();

Console.ReadKey();


// 创建用户
void CreateUser()
{
    dcCollection.InsertOne(new User
    {
        Age = random.Next(10, 60),
        CreatedAt = DateTime.Now,
        IsActive = true,
        Name = $"Rector_{count + 1}",
        Password = "123456",
        Order = count + 1
    });
}

void FindAllUsers()
{
    count = dcCollection.CountDocuments("{}");
    Console.WriteLine($"总用户数:{count}");
    var users = dcCollection.AsQueryable().ToList();
    foreach (var user in users)
    {
        Console.WriteLine($"id:{user.Id},name:{user.Name},password:{user.Password},created_at:{user.CreatedAt},is_active:{user.IsActive},order:{user.Order},age:{user.Age}");
    }
}

void UpdateUser()
{
    var update = Builders<User>.Update.Set("age", 36);
    dcCollection.FindOneAndUpdate(x => x.Order == 1, update);
}

void ReplaceUser()
{
    var item = dcCollection.Find(x => x.Order == 1).FirstOrDefault();
    if (item != null)
    {
        item.Age = 60;
        item.Name = "Rector Liu";
        item.Description = "修改(替换)";
        dcCollection.ReplaceOne(x => x.Order == 1, item, new ReplaceOptions());
    }
}

void ClearUser()
{
    dcCollection.DeleteMany("{}");
}

public class BsonUtcDateTimeSerializer : DateTimeSerializer
{
    public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return new DateTime(base.Deserialize(context, args).Ticks, DateTimeKind.Unspecified);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
    {
        var utcValue = new DateTime(value.Ticks, DateTimeKind.Utc);
        base.Serialize(context, args, utcValue);
    }
}