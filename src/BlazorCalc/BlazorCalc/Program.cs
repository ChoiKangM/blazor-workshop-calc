using BlazorCalc.Components;
using BlazorCalc.Components.Calc;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// API ������ ���ؼ� HttpClient ����ϱ� ���� �߰�.
builder.Services.AddHttpClient();
string currentDate = DateTime.Now.ToString("yyyyMMdd");

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri($" https://www.koreaexim.go.kr/site/program/financial/exchangeJSON?authkey=0MKRDFz4g6nOf8Id5lBUgxCfceeVrud1&searchdate={currentDate}&data=AP01")
    });
Console.WriteLine("api success");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// CalcState �߰�
builder.Services.AddSingleton<CalcState>();

//API�� ������ ȯ�� ���� �߰�
builder.Services.AddSingleton<GetData1>();

var app = builder.Build();

//API�� ���� ������ �޾ƿ���
using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;

var httpClient = services.GetRequiredService<HttpClient>();

HttpResponseMessage response = await httpClient.GetAsync("");


if (response.IsSuccessStatusCode)
{
    string jsonResponse = await response.Content.ReadAsStringAsync();

    // JSON ������ �Ľ�
    var exchangeRates = JsonSerializer.Deserialize<ExchangeRate[]>(jsonResponse);

    // ���͸� �� �� ����
    foreach (var rate in exchangeRates)
    {
        if (rate.result == 1 && rate.cur_unit == "USD")
        {
            var dataService = services.GetRequiredService<GetData1>();
            dataService.Rate = exchangeRates.FirstOrDefault(r => r.result == 1 && r.cur_unit == "USD");
            Console.WriteLine($"deal_bas_r: {rate.deal_bas_r}");

        }
    }
}
else
{
    Console.WriteLine("API ȣ�� ����.");
}

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


//JSON �����Ϳ� �����ϴ� ȯ�� Ŭ���� ����.
public class ExchangeRate
{
    public int result { get; set; }
    public string cur_unit { get; set; }
    public string ttb { get; set; }
    public string tts { get; set; }
    public string deal_bas_r { get; set; }
    public string bkpr { get; set; }
    public string yy_efee_r { get; set; }
    public string ten_dd_efee_r { get; set; }
    public string kftc_bkpr { get; set; }
    public string kftc_deal_bas_r { get; set; }
    public string cur_nm { get; set; }

}