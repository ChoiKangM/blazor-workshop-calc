using BlazorCalc.Components;
using BlazorCalc.Components.Calc;

var builder = WebApplication.CreateBuilder(args);
// API ������ ���ؼ� HttpClient ����ϱ� ���� HttpClient ���� �߰�.

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// CalcState �߰�
builder.Services.AddSingleton<CalcState>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


