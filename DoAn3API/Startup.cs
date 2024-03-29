using DoAn3API.DataContext;
using DoAn3API.Services.Authenticate;
using DoAn3API.Services.Categories;
using DoAn3API.Services.Firebase;
using DoAn3API.Services.Permissions;
using DoAn3API.Services.Products;
using DoAn3API.Services.Roles;
using DoAn3API.Services.StoreService;
using DoAn3API.Services.Users;
using Infastructure.Data;
using Infastructure.Repositories;
using Infastructure.Repositories.Catalogs.CategoryRepo;
using Infastructure.Repositories.Catalogs.ProductCategoryRepo;
using Infastructure.Repositories.PermissionRepo;
using Infastructure.Repositories.ProductImageRepo;
using Infastructure.Repositories.ProductRepo;
using Infastructure.Repositories.RoleRepo;
using Infastructure.Repositories.UserRepo;
using Infastructure.Repositories.UserRoleRepo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DoAn3API
{
    public class Startup
    {
        const string SECRET_KEY = "KeyOfMyshop10121994";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<DapperContext>();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            services.AddDbContext<AppDbContext>(options =>
               //options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddAutoMapper(typeof(Startup));
            #region Config Sercure API by JWT
            //Register configuration and validate token
            string issuer = Configuration["Token:Issuer"];
            string issuer2 = Configuration.GetSection("Token:Issuer").Get<string>();
            string signingKey = Configuration.GetValue<string>("Token:Issuer");
            string SECRET_KEY = Configuration.GetValue<string>("Token:Key");

            var SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = SIGNING_KEY, //The key also defined in jwtController
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = issuer

                    };
                });

            #endregion Config Sercure API by JWT
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api do an 3", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            #region Register Service
            /*Register Service, Repos DI here*/

            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IStorageService, FileStorageService>();
            services.AddScoped<IFirebaseService, FirebaseService>();

            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionService, PermissionService>();




            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            #endregion   Register Service
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api do an 3 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(options => options
                          .SetIsOriginAllowed(x => true)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials()
                          );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
