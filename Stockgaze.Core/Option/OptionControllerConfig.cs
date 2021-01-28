//  ==========================================================================
//  Copyright (C) 2020 by Genetec, Inc.
//  All rights reserved.
//  May be used only in accordance with a valid Source Code License Agreement.
//  ==========================================================================

using System;
using System.Collections.Generic;

namespace Stockgaze.Core.Option
{

    public class OptionControllerConfig
    {

        public bool FilterInfiniteReturn { get; set; } = true;

        public int FilterMinVolume { get; set; } = 0;

        public bool SearchNasdaq { get; set; } = false;

        public bool SearchTsx { get; set; } = false;

        public bool SearchNyse { get; set; } = false;

        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddDays(14);

        public double MaxStrikePrice { get; set; } = 999.99;

        public double MinStrikePrice { get; set; } = 0.1;

        public List<string> Emails { get; set; } = new List<string>();

        public bool FilterStrikeGreaterThanStock { get; set; }

        public override string ToString() { return $"--ExpiryDate {ExpiryDate.ToShortDateString()} --MaxStrikePrice {MaxStrikePrice} --MinStrikePrice {MinStrikePrice} --SearchNasdaq {SearchNasdaq} --SearchTsx {SearchTsx} --SearchNyse {SearchNyse} --FilterInfiniteReturn {FilterInfiniteReturn} --FilterMinVolume{FilterMinVolume} --Emails {string.Join(";", Emails)}"; }

    }

}