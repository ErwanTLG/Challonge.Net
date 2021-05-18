using System;
using System.Text.Json.Serialization;

namespace Challonge.Participants
{
    internal class ParticipantData
    {
        [JsonPropertyName("participant")]
        public Participant Participant { get; set; }
    }

    public struct Participant
    {
        [JsonPropertyName("acitve")]
        public bool Active { get; set; }

        [JsonPropertyName("checked_in_at")]
        public DateTimeOffset? CheckedInAt { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        [JsonPropertyName("final_rank")]
        public int? FinalRank { get; set; }

        [JsonPropertyName("group_id")]
        public int? GroupId { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("invitation_id")]
        public int? InvitationId { get; set; }

        [JsonPropertyName("invite_email")]
        public string InviteEmail { get; set; }

        /// <summary>
        /// Max: 255 characters. Multi-purpose field that is only visible via the API 
        /// and handy for site integration (e.g. key to your users table)
        /// </summary>
        [JsonPropertyName("misc")]
        public string Misc { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("on_waiting_list")]
        public bool OnWaitingList { get; set; }

        [JsonPropertyName("seed")]
        public int Seed { get; set; }

        [JsonPropertyName("tournament_id")]
        public int TournamentId { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        [JsonPropertyName("challonge_username")]
        public string ChallongeUsername { get; set; }

        [JsonPropertyName("challonge_email_address_verified")]
        public string ChallongeEmailAddressVerified { get; set; }

        [JsonPropertyName("removable")]
        public bool Removable { get; set; }

        [JsonPropertyName("participatable_or_invitation_attached")]
        public bool ParticipatableOrInvitaionAttached { get; set; }

        [JsonPropertyName("confirm_remove")]
        public bool ConfirmRemove { get; set; }

        [JsonPropertyName("invitation_pending")]
        public bool InvitationPending { get; set; }

        [JsonPropertyName("display_name_with_invitation_email_address")]
        public string DisplayNameWithInvitationEmailAddress { get; set; }

        [JsonPropertyName("email_hash")]
        public string EmailHash { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("attached_participatable_portrait_url")]
        public string AttachedParticipatablePortraitUrl { get; set; }

        [JsonPropertyName("can_check_in")]
        public bool CanCheckIn { get; set; }

        [JsonPropertyName("checked_in")]
        public bool CheckedIn { get; set; }

        [JsonPropertyName("reactiviatable")]
        public bool Reactivatable { get; set; }
    }
}
