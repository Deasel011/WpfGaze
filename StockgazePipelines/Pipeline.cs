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

    public interface IPipeline
    {
        IDisposable BuildPipeline();

    }
    public class Pipeline: IPipeline
    {

        public List<PipelineItem> PipelineItems;

        public IDisposable BuildPipeline() { throw new NotImplementedException(); }

    }

    public class PipelineItem
    {

        public IDataflowBlock block;

    }

}