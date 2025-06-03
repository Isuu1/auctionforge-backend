using Supabase;
using Supabase.Gotrue;

namespace Supabase.Services
{
    public class SupabaseService
    {
        private readonly Supabase.Client _supabase;

        public SupabaseService(IConfiguration configuration)
        {
            var url = configuration["Supabase:Url"];
            var key = configuration["Supabase:AnonKey"];

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            _supabase = new Supabase.Client(url, key, options);
        }

        public Supabase.Client GetClient() => _supabase;
    }
}