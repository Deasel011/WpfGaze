//  ==========================================================================
//  Copyright (C) 2020 by Genetec, Inc.
//  All rights reserved.
//  May be used only in accordance with a valid Source Code License Agreement.
//  ==========================================================================

using System;
using System.Windows;
using System.Windows.Controls;
using Stockgaze.Core.Pipelines;

namespace OptionGaze.Pipelines
{

    public partial class PipelinesUserControl : UserControl
    {

        private PipelinesUCVM m_pipelinesUCVM;

        public PipelinesUserControl() { try
            {
                InitializeComponent();
                m_pipelinesUCVM = new PipelinesUCVM();
                DataContext = m_pipelinesUCVM;
                m_pipelinesUCVM.Initialize();
            }
            catch (Exception e)
            {
                MessageBox.Show($"message:{e.Message} stacktrace:{e.StackTrace}");
            }}

    }

}