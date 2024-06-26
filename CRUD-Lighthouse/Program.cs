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

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "categoryDetails",
        pattern: "categories/details/{id}",
        defaults: new { controller = "Home", action = "Details" }
    );

    endpoints.MapControllerRoute(
        name: "categoryDelete",
        pattern: "categories/delete/{id}",
        defaults: new { controller = "Home", action = "Delete" }
    );

    endpoints.MapControllerRoute(
        name: "categoryDeleteConfirmed",
        pattern: "categories/deleteconfirmed/{id}",
        defaults: new { controller = "Home", action = "DeleteConfirmed" }
    );

    endpoints.MapControllerRoute(
        name: "categoryCreate",
        pattern: "categories/create",
        defaults: new { controller = "Home", action = "Create" }
    );

    endpoints.MapControllerRoute(
        name: "edit",
        pattern: "categories/edit/{id?}",
        defaults: new { controller = "Home", action = "Edit" }
);

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});




app.Run();
