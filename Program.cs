using DrivingStatistic.AL;
using DrivingStatistic.BLL;
using DrivingStatistic.DAL;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<IGetCountryService, GetCountryService>(client =>
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd("DrivingStatistic/1.0 (suchen8282@hotmail.com)");
});

builder.Services.AddSingleton<DbInit>();
ALExtentions.AddAL(builder.Services);
BLLExtentions.AddBLL(builder.Services);
DALExtentions.AddDAL(builder.Services);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbInit = scope.ServiceProvider.GetRequiredService<DbInit>();
    await dbInit.DatabaseInitAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o => o.EnableTryItOutByDefault());
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
