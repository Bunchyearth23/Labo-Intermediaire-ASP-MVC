using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DataLayer;

namespace LaboInter.Tools
{
    public class SessionManager
    {
        private readonly ISession _session;
        public SessionManager(IHttpContextAccessor context)
        {
            _session = context.HttpContext.Session;
        }

        public Clients? CurrentUser
        {
            get
            {
                if (string.IsNullOrEmpty(_session.GetString("conn")))
                    return null;
                return JsonSerializer.Deserialize<Clients>(_session.GetString("conn"));
            }
            set
            {
                _session.SetString("conn", JsonSerializer.Serialize(value));
            }
        }

        public void Logout()
        {
            _session.Clear();
        }
    }
}
