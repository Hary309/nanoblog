using Nanoblog.Core.Data.Exception;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nanoblog.Core.Extensions;

namespace Nanoblog.Api.Data.Models
{
    public class Entry
    {
        [Key]
        public string Id { get; private set; }

		public User Author { get; private set; }

		public string Text { get; private set; }

        public bool Deleted { get; private set; }

		public DateTime CreateTime { get; private set; }

        public Entry()
        {
            CreateTime = DateTime.Now;
        }

        public Entry(User user, string text) : this()
		{
            SetAuthor(user);
            SetText(text);
		}

        public void SetAuthor(User user)
        {
            if (user is null)
            {
                throw new ApiException("Invalid entry user id");
            }

            Author = user;
        }

        public void SetText(string text)
        {
            if (text is null || text.IsEmpty())
            {
                throw new ApiException("Entry text is empty");
            }

            Text = text.Trim();
        }

        public void Delete()
        {
            Deleted = true;
        }
	}
}
