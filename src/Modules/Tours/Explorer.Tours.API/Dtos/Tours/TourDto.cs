﻿using static Explorer.Tours.API.Enums.TourEnums;

namespace Explorer.Tours.API.Dtos.Tours
{
    public class TourDto
    {
        public long TourId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Difficult { get; set; }
        public TourStatus Status { get; set; }
        public double Price { get; set; }
        public List<PointDto>? Points { get; set; }
        public List<TagDto>? Tags { get; set; }
        public List<RequiredTimeDto>? RequiredTimes { get; set; }
        public GuideDto Guide { get; set; }
        public float? Length { get; set; }
        public DateTime? PublishTime { get; set; }
        public DateTime? ArhiveTime { get; set; }
    }
}
