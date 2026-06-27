using trabajo_kaneko.Repository;
using trabalho_kaneko.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<trabalho_kaneko.Data.DbContext>();
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<PaisRepository>();
builder.Services.AddScoped<EstadoRepository>();
builder.Services.AddScoped<CidadeRepository>();
builder.Services.AddScoped<FornecedorRepository>();
builder.Services.AddScoped<CargoRepository>();
builder.Services.AddScoped<FuncionarioRepository>();
builder.Services.AddScoped<GrupoRepository>();
builder.Services.AddScoped<MarcaRepository>();
builder.Services.AddScoped<ProdutoRepository>();
builder.Services.AddScoped<FormaPagamentoRepository>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
