﻿using MauiContactManager.Interfaces;

namespace MauiContactManager
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly IContactDatabase _contactDatabase;

        public MainPage(IContactDatabase contactDatabase)
        {
            InitializeComponent();
            _contactDatabase = contactDatabase;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}