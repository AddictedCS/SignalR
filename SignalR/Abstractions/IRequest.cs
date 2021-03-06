﻿using System.Collections.Specialized;

namespace SignalR.Abstractions
{
    public interface IRequest
    {
        string Path { get; }
        NameValueCollection QueryString { get; }
        NameValueCollection Headers { get; }
        NameValueCollection Form { get; }
        NameValueCollection Cookies { get; }
    }
}
