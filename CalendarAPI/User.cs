using System;

namespace CalendarAPI
{
    internal enum GrantTypes
    {
        authorization_code,
        refresh_token
    }

    public class User
    {
        public readonly string Name;
        public readonly string EMail;
        public readonly string Password;
        public readonly string Domain;

        public User(string name, string email, string password, string domain)
        {
            Name = name;
            EMail = email;
            Password = password;
            Domain = domain;
        }


        public static bool operator ==(User x, User y)
        {
            if ((object)x != null)
            {
                return x.Equals(y);
            }

            if ((object)y != null)
            {
                return y.Equals(x);
            }

            return true;
        }

        public static bool operator !=(User x, User y)
        {
            return !(x == y);
        }

        public static implicit operator string(User user)
        {
            return user == null ? null : user.Name;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is User && ToString().Equals(obj.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Domain) ? Name : string.Format("{0}/{1}", Domain, Name);
        }
    }

    public class GoogleUser : User
    {
        public readonly string ClientId;
        public readonly string ClientSecret;
        public readonly string RefreshToken;
        public GoogleUser(string name, string eMail, string password)
            : this(name, eMail, password, null, null, null)
        { }

        public GoogleUser(string name, string eMail, string password, string clientId, string clientSecret)
            : this(name, eMail, password, clientId, clientSecret, null)
        { }

        public GoogleUser(string name, string eMail, string password, string clientId, string clientSecret, string refreshToken)
            : base(name, eMail, password, "gmail.com")
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            RefreshToken = refreshToken;
        }

    }
}