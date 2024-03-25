using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.IntegrationEvents;
using NerdStore.Pagamento;
using NerdStore.Pagamento.Commands;
using NerdStore.Pedido;
using NerdStore.Pedido.Commands;
using NerdStore.WebApp.Data;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Persistence.InMem;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Rebus.Transport.InMem;

var nomeFila = "fila_rebus";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddRebus(configure => configure
    .Transport(t => t.UseInMemoryTransport(new InMemNetwork(), nomeFila))
    //.Transport(t => t.UseRabbitMq("amqp://localhost", nomeFila))
    //.Subscriptions(s => s.StoreInMemory())
    .Routing(r =>
    {
        r.TypeBased()
            .MapAssemblyOf<Message>(nomeFila)
            .MapAssemblyOf<RealizarPedidoCommand>(nomeFila)
            .MapAssemblyOf<RealizarPagamentoCommand>(nomeFila);
    })
    .Sagas(s => s.StoreInMemory())
    .Options(o =>
    {
        o.SetNumberOfWorkers(1);
        o.SetMaxParallelism(1);
        o.SetBusName("Demo Rebus");
    })
);

// Register handlers 
builder.Services.AutoRegisterHandlersFromAssemblyOf<PagamentoCommandHandler>();
builder.Services.AutoRegisterHandlersFromAssemblyOf<PedidoSaga>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


using var activator = new BuiltinHandlerActivator();

var subscriber = Configure.With(activator)
    .Transport(t => t.UseInMemoryTransport(new InMemNetwork(), nomeFila))
    .Start();

await subscriber.Subscribe<PedidoRealizadoEvent>();
await subscriber.Subscribe<PagamentoRealizadoEvent>();
await subscriber.Subscribe<PedidoFinalizadoEvent>();
await subscriber.Subscribe<PagamentoRecusadoEvent>();
await subscriber.Subscribe<PedidoCanceladoEvent>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
