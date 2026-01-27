using CorrelationId;

namespace Web.Extensions.Middleware;

public static class CorelationIdAppExtension
{
    public static IApplicationBuilder UseCorrelationAndSecurityHeaders(
        this IApplicationBuilder app,
        IWebHostEnvironment env
    )
    {
        app.UseCorrelationId();

        app.UseHsts();
        app.UseXContentTypeOptions();
        app.UseXfo(options => options.Deny());
        app.UseXXssProtection(options => options.Enabled());
        app.UseReferrerPolicy(options => options.NoReferrer());

        return app;
    }
}