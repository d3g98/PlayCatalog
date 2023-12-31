using Play.Catalog.Serivce.Entities;
using Play.Catalog.Serivce.RabbitMQ;
using Play.Common.MongoDb;

var builder = WebApplication.CreateBuilder(args);

builder.AddMongo()
    .AddMongoRepository<Item>("items");

// Add services to the container.
builder.Services.AddScoped<IRabitMQProducer, RabitMQProducer>();

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
