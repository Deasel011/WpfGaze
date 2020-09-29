//  ==========================================================================
//  Copyright (C) 2020 by Genetec, Inc.
//  All rights reserved.
//  May be used only in accordance with a valid Source Code License Agreement.
//  ==========================================================================

using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;

namespace StockgazePipelines
{

    public static class DataflowExtensions
    {

        public static IEnumerable<IDisposable> LinkToNonNull<TOutput>(
            this ISourceBlock<TOutput> source,
            ITargetBlock<TOutput> target)
        {
            return new []{source.LinkTo(target, (x)=>x!=null),source.LinkTo(DataflowBlock.NullTarget<TOutput>())};
        }

    }

}