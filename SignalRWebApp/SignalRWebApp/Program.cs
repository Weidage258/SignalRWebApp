using SignalRWebApp.Hubs;
using SignalRWebApp.Service;
using SqlSugar;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//����SignalR����
builder.Services.AddSignalR();
// ����Session����
builder.Services.AddSession();
// ע��SqlSugar����
builder.Services.AddTransient<ISqlSugarClient>(provider =>
{
    SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
    {
        ConnectionString = builder.Configuration.GetSection("conn").Value,//���ӷ��ִ�
        DbType = DbType.SqlServer, //���ݿ�����
        IsAutoCloseConnection = true //�����trueҪ�ֶ�close
    });
    return db;
});
// ע���û�����
builder.Services.AddTransient<IUserService, UserService>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//����
app.MapHub<ChatHub>("/chatHub");
// ʹ��Session
app.UseSession();
app.Run();