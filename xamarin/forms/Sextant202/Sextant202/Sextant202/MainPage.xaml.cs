﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI.XamForms;
using Xamarin.Forms;

namespace Sextant202
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ReactiveContentPage<MainPageViewModel>
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }
}
