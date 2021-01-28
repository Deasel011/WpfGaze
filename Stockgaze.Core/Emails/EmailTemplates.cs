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
            @"<table>
        <tr>
                <th>
                    Symbol
                </th>
                <th>
                    Exchange
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
                <th>
                    OptionType
                </th>
        </tr>
        
            {{ for option in options }}
        <tr>
                <td>
                    {{option.symbol}}
                </td>
                <td>
                    {{option.exchange}}
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
                <td>
                    {{option.option_type}}
                </td>
        </tr>
            {{end}}
        </table>
        <p>{{count}} items in table</p>
        <p>{{filter}}</p>";
        
        #endregion
        

        public static string FillOptionTemplate(string filter, List<SymbolOptionModel> optionList)
        {
            IEnumerable<SymbolOptionModel> options = optionList.OrderByDescending(o => o.Return);
            var template = Template.Parse(OptionTemplate);
            return template.Render(new {
                Options = options,
                Count = options.Count(),
                Filter = filter
            });
        }

    }

}