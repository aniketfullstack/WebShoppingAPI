﻿using System.Text.Json;

namespace WebShoppingAPI.Errors
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
