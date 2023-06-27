using Microsoft.Extensions.DependencyInjection;
using Play.Common.MongoDb;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.AddMongo()
    .AddMongoRepository<InventoryItem>("inventoryItems");

// Add services to the container.
builder.Services.AddHttpClient<CatalogClient>(client => {
    string url = builder.Configuration.GetValue(typeof(string), "HostCatalogService").ToString();
    if (!url.EndsWith("/")) url += "/";
    url += "api/";
    client.BaseAddress = new Uri(url);
});

builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
