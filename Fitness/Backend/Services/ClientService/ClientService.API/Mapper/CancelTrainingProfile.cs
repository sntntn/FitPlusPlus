﻿using AutoMapper;
using ClientService.Common.Entities;
using EventBus.Messages.Events;

namespace ClientService.API.Mapper
{
    public class CancelTrainingProfile:Profile
    {
        public CancelTrainingProfile()
        {
            CreateMap<CancelTrainingInformation, TrainerCancellingTrainingEvent>().ReverseMap();
        }
    }
}
