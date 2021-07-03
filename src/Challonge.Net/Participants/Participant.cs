using System;
using System.Text.Json.Serialization;

namespace Challonge.Participants
{
    internal class ParticipantData
    {
        [JsonPropertyName("participant")]
        public Participant Participant { get; set; }
    }

    public class Participant
    {
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        /// <summary>
        /// When this participant checked-in
        /// </summary>
        [JsonPropertyName("checked_in_at")]
        public DateTimeOffset? CheckedInAt { get; set; }

        /// <summary>
        /// When this participant was created
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// The rank of the participant at the end of the tournament
        /// </summary>
        [JsonPropertyName("final_rank")]
        public int? FinalRank { get; set; }

        /// <summary>
        /// Two stage tournaments only: the id of the group this participant is attached to
        /// </summary>
        [JsonPropertyName("group_id")]
        public int? GroupId { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// The participant's unique id
        /// </summary>
        /// <remarks>Each participant is attached to a tournament, so multiple participants can share the same id,
        /// if they are not playing in the same tournament.</remarks>
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

        /// <summary>
        /// The name displayed in the bracket/schedule
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Whether or not this participant is currently on the waiting list of the tournament
        /// </summary>
        [JsonPropertyName("on_waiting_list")]
        public bool OnWaitingList { get; set; }

        /// <summary>
        /// The participant's seed
        /// </summary>
        [JsonPropertyName("seed")]
        public int Seed { get; set; }

        /// <summary>
        /// The id of the tournament this participant is attached to
        /// </summary>
        [JsonPropertyName("tournament_id")]
        public int TournamentId { get; set; }

        /// <summary>
        /// When this participant was last updated
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// The challonge username of this participant (only set if the participant has a challonge account)
        /// </summary>
        [JsonPropertyName("challonge_username")]
        public string ChallongeUsername { get; set; }

        /// <summary>
        /// Whether or not the email of the participant has been verified by Challonge!
        /// </summary>
        [JsonPropertyName("challonge_email_address_verified")]
        public bool ChallongeEmailAddressVerified { get; set; }

        [JsonPropertyName("removable")]
        public bool Removable { get; set; }

        [JsonPropertyName("participatable_or_invitation_attached")]
        public bool ParticipatableOrInvitaionAttached { get; set; }

        [JsonPropertyName("confirm_remove")]
        public bool ConfirmRemove { get; set; }

        /// <summary>
        /// Whether or not this participant has been invited to the tournament, but has not responded yet
        /// </summary>
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

        /// <summary>
        /// Whether or not the participant is allowed to check-in
        /// </summary>
        [JsonPropertyName("can_check_in")]
        public bool CanCheckIn { get; set; }

        /// <summary>
        /// Whether or not the participant checked in
        /// </summary>
        [JsonPropertyName("checked_in")]
        public bool CheckedIn { get; set; }

        [JsonPropertyName("reactiviatable")]
        public bool Reactivatable { get; set; }
    }
}
