using System;
using System.Runtime.Serialization;

namespace Utils
{
    public class UriEx : Uri
    {
        #region " Properties "

        public readonly QueryString QueryString;

        public string Url => $"{Scheme}://{Authority}{AbsolutePath}";

        #endregion

        #region " Constructors "
        public UriEx(string uriString) : base(uriString) => QueryString = new QueryString(Query);

        public UriEx(string uriString,
                     UriKind uriKind) : base(uriString,
                                             uriKind) => QueryString = new QueryString(Query);

        public UriEx(Uri baseUri,
                     string relativeUri) : base(baseUri,
                                                relativeUri) => QueryString = new QueryString(Query);

        public UriEx(Uri baseUri,
                     Uri relativeUri) : base(baseUri,
                                             relativeUri) => QueryString = new QueryString(Query);

        public UriEx(SerializationInfo serializationInfo,
                     StreamingContext streamingContext) : base(serializationInfo,
                                                               streamingContext) => QueryString = new QueryString(Query);

        #endregion

    }
}
