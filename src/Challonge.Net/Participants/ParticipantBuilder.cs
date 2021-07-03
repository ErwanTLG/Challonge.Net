namespace Challonge.Participants
{
    public class ParticipantBuilder
    {
        /// <summary>
        /// The name displayed in the bracket/schedule
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The challonge username of this participant
        /// </summary>
        public string ChallongeUsername { get; set; }

        /// <summary>
        /// The verified email address attached to this participant
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The participant's seed
        /// </summary>
        public int? Seed { get; set; }

        /// <summary>
        /// Max: 255 characters. Multi-purpose field that is only visible via the API 
        /// and handy for site integration (e.g. key to your users table)
        /// </summary>
        public string Misc { get; set; }
        
        public ParticipantBuilder() { }

        public ParticipantBuilder(Participant participant)
        {
            Name = participant.Name;
            ChallongeUsername = participant.ChallongeUsername;
            Email = participant.InviteEmail;
            Seed = participant.Seed;
            Misc = participant.Misc;
        }

        /// <summary>
        /// Creates the <see cref="Participant"/> with the corresponding properties
        /// </summary>
        /// <returns></returns>
        public Participant ToParticipant()
        {
            Participant participant = new Participant
            {
                Name = Name,
                ChallongeUsername = ChallongeUsername,
                InviteEmail = Email,
                Misc = Misc
            };
            
            if (Seed != null)
                participant.Seed = Seed.Value;

            return participant;
        }
    }
}