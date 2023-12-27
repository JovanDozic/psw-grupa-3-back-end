
﻿using Explorer.BuildingBlocks.Core.Domain;
﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Channels;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Tours.API.Dtos.TourExecutions;
using Explorer.Tours.Core.Domain.EventSourcingDomain;
using Explorer.Tours.Core.Domain.Tours;
using Explorer.Tours.Core.Domain.Utilities;
using Newtonsoft.Json;
using static Explorer.Tours.API.Enums.TourEnums;

namespace Explorer.Tours.Core.Domain.TourExecutions
{
    public class TourExecution: EventSourcedAggregate

    {
        private const int PointProximity = 100;
        public TourExecutionStatus Status { get; set; }
        public Position? Position { get; set; }
        public List<PointTask>? Tasks { get; set; } = new List<PointTask>();
        public int TourId { get; set; }
        public TourExecution(){}

        [JsonConstructor]
        public TourExecution(TourExecutionStatus status, Position position, List<PointTask> tasks)
        {
            Status = status;
            Position = position;
            Tasks = tasks;
        }
        public TourExecution(List<Point> points, int tourId)
        {
            Validate(points);
            PrepareInitialPosition(points);
            PrepareTasks(points);
            Status = TourExecutionStatus.Active;
            TourId=tourId;
        }
        public void UpdatePosition(Position currentPosition)
        {
            if (Status != TourExecutionStatus.Active) return;
            if (!Position.IsChanged(currentPosition)) return;
            Position = currentPosition;
            var currentPoint = Tasks.FirstOrDefault(x => !x.Done) ?? throw new Exception("Exception! All Tour Points passed!");
            var inProximity = DistanceCalculator.CalculateDistance(currentPoint, currentPosition) * 1000 <= PointProximity;
            if (inProximity)
            {
                currentPoint.Done = true;
                currentPoint.DoneOn = currentPosition.LastActivity;
            }
            Status = Tasks.TrueForAll(x => x.Done) ? TourExecutionStatus.Completed : Status;
        }

        public void QuitTourExecution()
        {
            Status = TourExecutionStatus.Abandoned;
        }

        private static void Validate(IReadOnlyCollection<Point> points)
        {
            if (points.Count < 0) throw new ArgumentException("Exception! No points found!");
        }
        private void PrepareTasks(IReadOnlyCollection<Point> points)
        {
            Tasks = new(
                points.Select(x => new PointTask(x, TaskType.Point, DateTime.MinValue, false)));
        }
        private void PrepareInitialPosition(IReadOnlyCollection<Point> points)
        {
            var firsPoint = points.FirstOrDefault() ?? throw new ArgumentException("Exception! No points found!");
            Position = new Position(firsPoint.Latitude, firsPoint.Longitude, DateTime.MinValue);
        }


        public double PercentageOfDone(TourExecution tourExecution)
        {
            if (tourExecution.Tasks == null || tourExecution.Tasks.Count == 0)
            {
                return 0.0;
            }

            int totalTasks = tourExecution.Tasks.Count;
            int completedTasks = tourExecution.Tasks.Count(task => task.Done);

            double percentage = (double)completedTasks / totalTasks * 100.0;

            return percentage;
        }


        public bool IsLastActivityWithinWeek(TourExecution tourExecution)
        {
            if (tourExecution.Position == null) return true;
            TimeSpan timeDifference = DateTime.Now - tourExecution.Position.LastActivity;
            return timeDifference.TotalDays > 7.0;
        }

        //Events

        public void UpdatePositionEvent(int id, Position currentPosition, DateTime time)
        {
            Causes(new UserLocationNoted(id, currentPosition, time));
        }

        public void QuitTourEvent(int id, DateTime time)
        {
            Causes(new TourQuit(id, time));
        }

        public void CompleteTourEvent(int id, Position currentPosition, DateTime time)
        {
            if (Status != TourExecutionStatus.Active) return;
            if (!Position.IsChanged(currentPosition)) return;
            Position = currentPosition;
            var currentPoint = Tasks.FirstOrDefault(x => !x.Done) ?? throw new Exception("Exception! All Tour Points passed!");
            var inProximity = DistanceCalculator.CalculateDistance(currentPoint, currentPosition) * 1000 <= PointProximity;
            if (inProximity)
            {
                currentPoint.Done = true;
                currentPoint.DoneOn = currentPosition.LastActivity;
            }

            Causes(new TourCompleted(id, currentPosition, time));
        }

        public override void Apply(DomainEvent @event)
        {
            When((dynamic)@event);
            Version = Version++;
        }

        private void Causes(DomainEvent @event)
        {
            Changes.Add(@event);
            Apply(@event);
        }

        private void When(TourQuit tour)
        {
            Status = TourExecutionStatus.Abandoned;
        }

        private void When(Position currentPosition)
        {
            Position = currentPosition;

        }

        private void When(TourCompleted tour)
        {
            Status = Tasks.TrueForAll(x => x.Done) ? TourExecutionStatus.Completed : Status;
        }

    }
}
