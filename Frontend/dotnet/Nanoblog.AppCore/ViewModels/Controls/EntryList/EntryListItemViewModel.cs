﻿using Nanoblog.Common;
using Nanoblog.Common.Dto;
using Nanoblog.AppCore.Navigation;
using Nanoblog.ApiService;
using Nanoblog.AppCore.ViewModels.Pages;
using System.Windows.Input;

namespace Nanoblog.AppCore.ViewModels.Controls.EntryList
{
    public class EntryListItemViewModel : ItemListItemViewModel
    {
        private EntryDto _entryData;
        private int _commentsCount;

        public string Id { get => _entryData.Id; }

        public int CommentsCount
        {
            get => _commentsCount;
            set => Update(ref _commentsCount, value);
        }

        public ICommand ShowCommentsCommand { get; set; }

        public EntryListItemViewModel(EntryDto entry)
        {
            ShowCommentsCommand = new RelayCommand(OnShowComments);
            DeleteCommand = new RelayCommand(OnDelete);
            ChangeVoteCommand = new RelayCommand<KarmaValue>(OnChangeVote);

            _entryData = entry;
            UserName = entry.Author.UserName;
            Date = entry.CreateTime.ToString();
            Text = entry.Text;
            CommentsCount = entry.CommentsCount;
            KarmaCount = entry.KarmaCount;
            UserVote = entry.UserVote;

            IsDeletable = entry.Author.Id == App.CurrentUser.Id;
        }

        void OnShowComments()
        {
            if (!InsideDetail)
            {
                PageNavigator.Instance.Push(new EntryDetailPageViewModel(this));
            }
        }

        async void OnDelete()
        {
            if (IsDeletable)
            {
                await EntryService.Instance.Delete(_entryData.Id);

                Deleted = true;

                if (InsideDetail)
                {
                    PageNavigator.Instance.Pop();
                }
            }
        }

        async void OnChangeVote(KarmaValue karmaValue)
        {
            await OnChangeVote(KarmaService.VoteFor.Entry, karmaValue, _entryData.Id);
        }


    }
}
