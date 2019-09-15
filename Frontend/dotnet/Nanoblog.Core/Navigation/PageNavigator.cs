﻿using Nanoblog.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace Nanoblog.Core.Navigation
{
    public class PageNavigator : IPageNavigator
    {
        static PageNavigator _instance;

        Dictionary<Type, Type> _types = new Dictionary<Type, Type>();

        IMainWindow _mainWindow;

        Stack<PageData> _pageStack = new Stack<PageData>();

        public PageData CurrentPage { get; set; }

        static public PageNavigator Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new PageNavigator();
                }

                return _instance;
            }
        }

        public void SetMainWindow(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void Register<TPage, TPageViewModel>()
        {
            _types.Add(typeof(TPageViewModel), typeof(TPage));
        }

        public void Navigate<TPageViewModel>()
        {
            _pageStack.Clear();

            var pageData = CreatePageData(typeof(TPageViewModel));

            SetPageData(pageData);
        }

        public void Navigate<TPageViewModel, TParameter>(TParameter parameter)
        {
            _pageStack.Clear();

            var pageData = CreatePageData(typeof(TPageViewModel), parameter);

            SetPageData(pageData);
        }

        public void Push<TPageViewModel>()
        {
            var pageData = CreatePageData(typeof(TPageViewModel));

            SetPageData(pageData);
        }

        public void Push<TPageViewModel, TParameter>(TParameter parameter)
        {
            var pageData = CreatePageData(typeof(TPageViewModel), parameter);

            SetPageData(pageData);
        }

        public void Pop()
        {
            if (_pageStack.Count > 2)
            {
                _pageStack.Pop();
                var pageData = _pageStack.Peek();

                CurrentPage = pageData;
                _mainWindow.SetPageData(pageData.Page, pageData.ViewModel);
            }
        }

        void SetPageData(PageData pageData)
        {
            _pageStack.Push(CurrentPage);

            CurrentPage = pageData;
            _mainWindow.SetPageData(pageData.Page, pageData.ViewModel);
        }

        PageData CreatePageData(Type viewModelType)
        {
            Type pageType = _types[viewModelType];

            var page = Activator.CreateInstance(pageType);
            var pageViewModel = (BaseViewModel)Activator.CreateInstance(viewModelType);

            return new PageData { Page = page, ViewModel = pageViewModel };
        }

        PageData CreatePageData<TParameter>(Type viewModelType, TParameter parameter)
        {
            Type pageType = _types[viewModelType];

            var page = Activator.CreateInstance(pageType);
            var pageViewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, parameter);

            return new PageData { Page = page, ViewModel = pageViewModel };
        }
    }
}
