﻿using System.Text.Json.Serialization;

namespace DoAn3API.Dtos.TokenDto
{
    public class AuthReponseDto
    {
        [JsonPropertyName("access_token")]
        public string Token { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expired_in")]
        public int TotalSecond { get; set; }
    }
}
