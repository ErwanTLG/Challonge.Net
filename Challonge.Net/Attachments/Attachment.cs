using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text;

namespace Challonge.Attachments
{
    internal class AttachmentData
    {
        [JsonPropertyName("match_attachment")]
        public Attachment Attachment { get; set; }
    }

    public struct Attachment
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("match_id")]
        public int MatchId { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("original_file_name")]
        public string OriginalFileName { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

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
