using TgyMiddlewareExampleWeb.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

#region Use and Run Usage
//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("Before first middleware\n");
//    await next();
//    await context.Response.WriteAsync("After first middleware\n");
//});

//app.Use(async (context, next) =>
//{
//    await context.Response.WriteAsync("Before second middleware\n");
//    await next();
//    await context.Response.WriteAsync("After second middleware\n");
//}); 

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Terminal third middleware\n");
//}); 
#endregion

#region Map usage
//app.Map("/ornek", app =>
//{
//    app.Run(async context =>
//    {
//        context.Response.WriteAsync("a middleware for ornek url");
//    });
//});

#endregion

#region MapWhen Usage
//app.MapWhen(context => context.Request.Query.ContainsKey("name"), app =>
//{
//    app.Use(async (context, next) =>
//    {
//        await context.Response.WriteAsync("Before 1. Middleware\n");
//        await next();
//        await context.Response.WriteAsync("After 1. Middleware\n");
//    });

//    app.Run(async (context) =>
//    {
//        await context.Response.WriteAsync("Terminal 2. Middleware\n");
//    });
//});
#endregion

app.UseMiddleware<WhiteIpAdressControlMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
