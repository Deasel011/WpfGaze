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
<style>
    table {border: 1px solid purple; border-collapse: collapse;}
    td   {border: 1px solid purple}
    th    {border: 1px solid purple}
</style>
<table>
        <tr>
                <th>
                    Symbol
                </th>
                <th>
                    ExpiryDate
                </th>
                <th>
                    Return
                </th>
                <th>
                    Volume
                </th>
                <th>
                    StockPrice
                </th>
                <th>
                    StrikePrice
                </th>
                <th>
                    OptionPrice
                </th>
                <th>
                    Volatility
                </th>
                <th>
                    Description
                </th>
        </tr>
        
            {{ for option in options }}
        <tr>
                <td>
                    {{option.symbol}}
                </td>
                <td>
                    {{option.expiry_date}}
                </td>
                <td>
                    {{option.return}}
                </td>
                <td>
                    {{option.volume}}
                </td>
                <td>
                    {{option.stock_price}}
                </td>
                <td>
                    {{option.strike_price}}
                </td>
                <td>
                    {{option.option_price}}
                </td>
                <td>
                    {{option.volatility}}
                </td>
                <td>
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