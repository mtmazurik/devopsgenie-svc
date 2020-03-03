﻿using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace DevOpsGenieService.Common
{
    public interface IRepository
    {
        Task<string> CreateDocumentAsync(string db, string collection, JToken document);
    }
}