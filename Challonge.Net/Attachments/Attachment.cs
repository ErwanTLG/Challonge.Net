using System;
using System.Text.Json.Serialization;

namespace Challonge.Attachments
{
    internal class AttachmentData
    {
        [JsonPropertyName("match_attachment")]
        public Attachment Attachment { get; set; }
    }

    public class Attachment
    {
        /// <summary>
        /// The attachment's unique id
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// The parent match's id
        /// </summary>
        [JsonPropertyName("match_id")]
        public int MatchId { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// The description of this attachment
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// The url of the attachment
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("original_file_name")]
        public string OriginalFileName { get; set; }

        /// <summary>
        /// When this attachment was created
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// When this attachment was last updated
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonPropertyName("asset_file_name")]
        public string AssetFileName { get; set; }

        [JsonPropertyName("asset_content_type")]
        public string AssetContentType { get; set; }

        [JsonPropertyName("asset_file_size")]
        public int? AssetFileSize { get; set; }

        [JsonPropertyName("asset_url")]
        public string AssetUrl { get; set; }
    }
}
