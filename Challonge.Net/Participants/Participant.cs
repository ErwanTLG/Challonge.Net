using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Challonge.Participants
{
    internal class ParticipantData
    {
        public Participant Participant { get; set; }
    }

    public struct Participant
    {
        [JsonPropertyName("acitve")]
        public bool Active { get; internal set; }

        [JsonPropertyName("checked_in_at")]
        public DateTimeOffset? CheckedInAt { get; internal set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; internal set; }

        [JsonPropertyName("final_rank")]
        public int? FinalRank { get; internal set; }

        [JsonPropertyName("group_id")]
        public int? GroupId { get; internal set; }

        [JsonPropertyName("icon")]
        public string Icon { get; internal set; }

        [JsonPropertyName("id")]
        public int Id { get; internal set; }

        [JsonPropertyName("invitation_id")]
        public int? InvitationId { get; internal set; }

        [JsonPropertyName("invite_email")]
        public string InviteEmail { get; set; }

        [JsonPropertyName("misc")]
        public string Misc { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("on_waiting_list")]
        public bool OnWaitingList { get; internal set; }

        [JsonPropertyName("seed")]
        public int Seed { get; set; }

        [JsonPropertyName("tournament_id")]
        public int TournamentId { get; internal set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset UpdatedAt { get; internal set; }

        [JsonPropertyName("challonge_username")]
        public string ChallongeUsername { get; internal set; }

        [JsonPropertyName("challonge_email_address_verified")]
        public string ChallongeEmailAddressVerified { get; internal set; }

        [JsonPropertyName("removable")]
        public bool Removable { get; internal set; }

        [JsonPropertyName("participatable_or_invitation_attached")]
        public bool ParticipatableOrInvitaionAttached { get; internal set; }

        [JsonPropertyName("confirm_remove")]
        public bool ConfirmRemove { get; internal set; }

        [JsonPropertyName("invitation_pending")]
        public bool InvitationPending { get; internal set; }

        [JsonPropertyName("display_name_with_invitation_email_address")]
        public string DisplayNameWithInvitationEmailAddress { get; internal set; }

        [JsonPropertyName("email_hash")]
        public string EmailHash { get; internal set; }

        [JsonPropertyName("username")]
        public string Username { get; internal set; }

        [JsonPropertyName("attached_participatable_portrait_url")]
        public string AttachedParticipatablePortraitUrl { get; internal set; }

        [JsonPropertyName("can_check_in")]
        public bool CanCheckIn { get; internal set; }

        [JsonPropertyName("checked_in")]
        public bool CheckedIn { get; internal set; }

        [JsonPropertyName("reactiviatable")]
        public bool Reactivatable { get; internal set; }
    }
}
