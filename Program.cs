using AllRiskSolutions_Desafio.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.AddStartup();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();