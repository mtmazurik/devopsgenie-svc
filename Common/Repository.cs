﻿using System;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DevopsGenie.Service.Common.Models;
using devopsgenie.service.Config;
using devopsgenie.service.Common;

namespace DevopsGenie.Service.Common
{
    // DAL that uses the RepositoryModel (an exact RepoNook model object, must match the Reponook model from DevopsGenie-Reponook service)
    public class Repository : IRepository
    {
        private readonly string REPONOOK_URI;
        private HttpClient _client;
        private IJsonConfiguration _config;
        private IEncryption _encryption;

        public Repository(HttpClient client, IJsonConfiguration config, IEncryption encryption)
        {
            _client = client;
            _config = config;
            _encryption = encryption;
            REPONOOK_URI = _config.DOGREPONOOK_URI + ":" + _config.DOGREPONOOK_PORT;
        }

        public string CreateDocument(string db, string collection, string document)
        {
            string apiResponse;

            RepositoryModel repoObject = new RepositoryModel();

            repoObject._id = Guid.NewGuid();
            repoObject.key = "key";
            repoObject.tags = new string[] { "tag1", "tag2"};
            repoObject.createdBy = "DOG-SVC";
            _encryption.EncryptionKey = repoObject.createdBy;
            repoObject.createdDate = DateTime.Now;
            repoObject.modifiedBy = "DOG-SVC";
            repoObject.modifiedDate = DateTime.Now;
            repoObject.app = "DOG";
            repoObject.repository = "tenant";
            repoObject.collection = "config";
            repoObject.validate = false;
            repoObject.schemaUri = "";
            repoObject.data = JsonConvert.ToString(document); // _encryption.encrypt(document);

            HttpContent body = new StringContent(JsonConvert.SerializeObject(repoObject), Encoding.UTF8, "application/json");

            string uri = REPONOOK_URI + "/" + db + "/" + collection;

            HttpResponseMessage result = _client.SendAsync(FormatRequest(HttpMethod.Post, uri, body)).Result;

            apiResponse = result.Content.ReadAsStringAsync().Result;

            return apiResponse;
        }
        private HttpRequestMessage FormatRequest(HttpMethod method, string uri, HttpContent content = null)
        {
            HttpRequestMessage retRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri(uri),
                Method = method
            };

            retRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            retRequest.Content = content;

            return retRequest;
        }

    }
}
