using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

var connectionString = Environment.GetEnvironmentVariable("MONGODB_URI");
if (connectionString is null)
{
    Console.WriteLine("You must set your 'MONGODB_URI' environment variable.");
    Environment.Exit(0);
}

var conventionPack = new ConventionPack
{
    new CamelCaseElementNameConvention()
};

ConventionRegistry.Register("camelCase", conventionPack, t => true);

var client = new MongoClient(connectionString);

var collection = client.GetDatabase("my_database").GetCollection<Movie>("movies");

var movie = new Movie(title: "Back to the Future");

await collection.InsertOneAsync(movie);

var filter = Builders<Movie>.Filter.Eq("Title", "Back to the Future");

var document = await collection.Find(filter).FirstOrDefaultAsync();

Console.WriteLine($"_id: {document.Id} , title: {document.Title}");

internal class Movie(string title)
{
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    public string Title { get; set; } = title;
}