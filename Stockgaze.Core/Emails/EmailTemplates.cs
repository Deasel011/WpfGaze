//  ==========================================================================
//  Copyright (C) 2020 by Genetec, Inc.
//  All rights reserved.
//  May be used only in accordance with a valid Source Code License Agreement.
//  ==========================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Scriban;
using Stockgaze.Core.Models;

namespace Stockgaze.Core.Emails
{

    public class EmailTemplates
    {

        #region templates
        private const string OptionTemplate =
            @"
<table style='border: 1px solid purple; border-collapse: collapse;'>
        <tr>
                <th style='border: 1px solid purple'>
                    Symbol
                </th>
                <th style='border: 1px solid purple'>
                    ExpiryDate
                </th>
                <th style='border: 1px solid purple'>
                    Return
                </th>
                <th style='border: 1px solid purple'>
                    Volume
                </th>
                <th style='border: 1px solid purple'>
                    StockPrice
                </th>
                <th style='border: 1px solid purple'>
                    StrikePrice
                </th>
                <th style='border: 1px solid purple'>
                    OptionPrice
                </th>
                <th style='border: 1px solid purple'>
                    Volatility
                </th>
                <th style='border: 1px solid purple'>
                    Description
                </th>
        </tr>
        
            {{ for option in options }}
        <tr>
                <td style='border: 1px solid purple'>
                    {{option.symbol}}
                </td>
                <td style='border: 1px solid purple'>
                    {{option.expiry_date}}
                </td>
                <td style='border: 1px solid purple'>
                    {{option.return}}
                </td>
                <td style='border: 1px solid purple'>
                    {{option.volume}}
                </td>
                <td style='border: 1px solid purple'>
                    {{option.stock_price}}
                </td>
                <td style='border: 1px solid purple'>
                    {{option.strike_price}}
                </td>
                <td style='border: 1px solid purple'>
                    {{option.option_price}}
                </td>
                <td style='border: 1px solid purple'>
                    {{option.volatility}}
                </td>
                <td style='border: 1px solid purple'>
                    {{option.description}}
                </td>
        </tr>
            {{end}}
        </table>
        <p>{{count}} items in table</p>
        <p>{{filter}}</p>";
        
        #endregion
        

        public static string FillOptionTemplate(string filter, List<SymbolOptionModel> optionList)
        {
            IEnumerable<SymbolOptionModel> options = optionList.Where(option => option.Return >= 0.05).OrderByDescending(o => o.Return).ToList();
            var template = Template.Parse(OptionTemplate);
            return template.Render(new {
                Options = options,
                Count = options.Count(),
                Filter = filter
            });
        }

    }

}