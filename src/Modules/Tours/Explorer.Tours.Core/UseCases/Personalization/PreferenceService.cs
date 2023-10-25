﻿using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Personalization;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Personalization
{
    public class PreferenceService : CrudService<PreferenceDto, Preference>, IPreferenceService
    {
        public PreferenceService(ICrudRepository<Preference> repository, IMapper mapper) : base(repository, mapper) { }

    }
}