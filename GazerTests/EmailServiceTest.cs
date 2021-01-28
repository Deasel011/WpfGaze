//  ==========================================================================
//  Copyright (C) 2020 by Genetec, Inc.
//  All rights reserved.
//  May be used only in accordance with a valid Source Code License Agreement.
//  ==========================================================================

using System;
using System.Collections.Generic;
using Stockgaze.Core.Models;
using Stockgaze.Core.Services;
using Stockgaze.Core.WPFTools;
using Xunit;

namespace GazerTests
{

    public class EmailServiceTest
    {

        [Fact]
        public void TestSendDummyEmail()
        {
            var emailService = new EmailService();
            
            emailService.SendOptionResults("Test", String.Empty, new List<string>{"philippe.deslongchamps@gmail.com"}, new List<SymbolOptionModel>() );

            var a = 1;
        }

    }

}