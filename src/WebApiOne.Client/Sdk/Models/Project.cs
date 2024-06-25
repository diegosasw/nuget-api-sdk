// <auto-generated/>
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
namespace WebApiOne.Client.Sdk.Models
{
    #pragma warning disable CS1591
    public class Project : IParsable
    #pragma warning restore CS1591
    {
        /// <summary>The id property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Id { get; set; }
#nullable restore
#else
        public string Id { get; set; }
#endif
        /// <summary>The isStarted property</summary>
        public bool? IsStarted { get; private set; }
        /// <summary>The members property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<WebApiOne.Client.Sdk.Models.Member>? Members { get; set; }
#nullable restore
#else
        public List<WebApiOne.Client.Sdk.Models.Member> Members { get; set; }
#endif
        /// <summary>The name property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>The startsOn property</summary>
        public DateTimeOffset? StartsOn { get; set; }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="WebApiOne.Client.Sdk.Models.Project"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static WebApiOne.Client.Sdk.Models.Project CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new WebApiOne.Client.Sdk.Models.Project();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "id", n => { Id = n.GetStringValue(); } },
                { "isStarted", n => { IsStarted = n.GetBoolValue(); } },
                { "members", n => { Members = n.GetCollectionOfObjectValues<WebApiOne.Client.Sdk.Models.Member>(WebApiOne.Client.Sdk.Models.Member.CreateFromDiscriminatorValue)?.ToList(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "startsOn", n => { StartsOn = n.GetDateTimeOffsetValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("id", Id);
            writer.WriteCollectionOfObjectValues<WebApiOne.Client.Sdk.Models.Member>("members", Members);
            writer.WriteStringValue("name", Name);
            writer.WriteDateTimeOffsetValue("startsOn", StartsOn);
        }
    }
}
