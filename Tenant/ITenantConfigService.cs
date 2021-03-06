﻿using DevopsGenie.Service.Common;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace DevopsGenie.Service.Tenant
{
    public interface ITenantConfigService
    {
        string CreateConfig(string body);
        string ReadConfig(string tenantId);
    }
}