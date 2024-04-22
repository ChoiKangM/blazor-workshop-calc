using BlazorCalc.Components;
using BlazorCalc.Components.Calc;

//API �������� ����� ��.
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;

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


