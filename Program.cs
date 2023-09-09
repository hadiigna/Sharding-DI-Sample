using Sharding_DI_Sample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<UserIdHeaderSwaggerFilter>();
});

//**重要** DIでHttpContext(リクエストヘッダーなど) を参照するには予めAccessorをDIに登録する必要がある
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRepositoryProvider, RepositoryProvider>();
builder.Services.AddScoped<IGameApiContext, GameApiContext>();

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