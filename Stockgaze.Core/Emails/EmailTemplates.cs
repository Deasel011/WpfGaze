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
        <tr>
            {{for option in options}}
                <td>
                    {{option.Symbol}}
                </td>
                <td>
                    {{option.Exchange}}
                </td>
                <td>
                    {{option.ExpiryDate}}
                </td>
                <td>
                    {{option.Return}}
                </td>
                <td>
                    {{option.Volume}}
                </td>
                <td>
                    {{option.StockPrice}}
                </td>
                <td>
                    {{option.StrikePrice}}
                </td>
                <td>
                    {{option.OptionPrice}}
                </td>
                <td>
                    {{option.Volatility}}
                </td>
                <td>
                    {{option.Description}}
                </td>
                <td>
                    {{option.OptionType}}
                </td>
            {{end}}
        </tr>

        </table>
        <p>{{filter}}</p>";
        
        #endregion
        

        public static string FillOptionTemplate(string filter, List<SymbolOptionModel> optionList)
        {
            var template = Template.Parse(OptionTemplate);
            return template.Render(new {
                Options = optionList.OrderByDescending(o => o.Return),
                Filter = filter
            });
        }

    }

}