using System;

namespace Nanoblog.Common.Dto
{
    public class EntryDto
    {
        public string Id { get; set; }

        public AuthorDto Author { get; set; }

        public string Text { get; set; }

        public DateTime CreateTime { get; set; }

        public int CommentsCount { get; set; }

        public int KarmaCount { get; set; }

        // reaction of user to this entry
        public KarmaValue UserVote { get; set; }
    }
}
