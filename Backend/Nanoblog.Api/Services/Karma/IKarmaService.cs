﻿using Nanoblog.Api.Data.Models;
using Nanoblog.Common;
using Nanoblog.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nanoblog.Api.Services.Karma
{
    public interface IKarmaService
    {
        Task GiveKarmaAsync(string authorId, string itemId, KarmaValue value);

        Task RemoveKarmaAsync(string authorId, string itemId);

        IEnumerable<KarmaDto> GetKarma(string itemId);

        Task<int> CountKarmaAsync(string itemId);

        Task<KarmaValue> GetUserVoteAsync(string authorId, string itemId);
    }
}
