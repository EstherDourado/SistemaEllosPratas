using EllosPratas.Data;
using EllosPratas.Services.CarrinhoVenda;
using EllosPratas.Services.Categoria;
using EllosPratas.Services.Loja;
using EllosPratas.Services.Produtos;
using EllosPratas.Services.Venda;
//using EllosPratas.Services.Cliente;
//using EllosPratas.Services.Estoque;
//using EllosPratas.Services.Funcionario;
using EllosPratas.Services.Caixa;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços do MVC e o suporte ao Newtonsoft.Json
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });
//Conectando ao banco de dados SQL Server
builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // O carrinho expira após 30 min de inatividade
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();


//Configurando a injeção de dependência
builder.Services.AddScoped<IProdutosInterface, ProdutosService>();
builder.Services.AddScoped<IClientesInterface, ClienteService>();
builder.Services.AddScoped<ICategoriasInterface, CategoriasServices>();
builder.Services.AddScoped<ICarrinhoInterface, CarrinhoServices>();
builder.Services.AddScoped<IVendasInterface, VendasServices>();
builder.Services.AddScoped<ILojaInterface, LojaService>();
builder.Services.AddScoped<IFuncionariosInterface, FuncionarioService>();
builder.Services.AddScoped<ICaixaInterface, CaixaService>();

var cultureInfo = new System.Globalization.CultureInfo("pt-BR");
System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
