﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.BuildingBlocks.Core.Domain
{
    public abstract class DomainEvent
    {
        public DomainEvent(int aggregateId)
        {
            Id = aggregateId;
        }

        public int Id { get; private set; } 
        
    }
}
