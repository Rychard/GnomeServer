using System;
using Game;
using GameLibrary;
using TypeLite;

namespace GnomeServer.Models
{
    [TsClass]
    public class Workshop
    {
        public UInt32 ID { get; set; }
        public String WorkshopID { get; set; }
        public String Name { get; set; }
        public Boolean IsSuspended { get; set; }
        public Int32 Priority { get; set; }
        public Boolean AcceptGeneratedJobs { get; set; }
        public Single Efficiency { get; set; }
        public Location Location { get; set; }
        public UInt32? AssignedGnomeID { get; set; }
        
        // WorkshopDef.MaxCapacity
        public Int32 StorageCapacityTotal { get; set; }

        // var mapCell = GnomanEmpire.Instance.Map.GetCell(Workshop.NewItemPos())
        // mapCell.Objects.Count
        public Int32 StorageCapacityUsed { get; set; }

        public Workshop()
        {
            // Empty constructor for serialization.
        }

        public Workshop(Game.Workshop workshop)
        {
            ID = workshop.ID;
            WorkshopID = workshop.WorkshopID;
            Name = workshop.Name();
            AcceptGeneratedJobs = workshop.AcceptGeneratedJobs;
            Location = new Location(workshop.Position);
            Efficiency = workshop.Efficiency();
            AssignedGnomeID = workshop.HasAssignedWorker ? workshop.Worker.ID : (UInt32?)null;
            Priority = workshop.Priority;
            IsSuspended = workshop.Suspended;
            StorageCapacityTotal = GnomanEmpire.Instance.GameDefs.WorkshopDef(workshop.WorkshopID).MaxCapacity;
            StorageCapacityUsed = GetStorageCapacityUsed(workshop);

            foreach (var workshopJob in workshop.JobQueue)
            {
                // TODO: Obtain the jobs at this workshop.
            }
        }

        private static Int32 GetStorageCapacityUsed(Game.Workshop workshop)
        {
            var newItemPosition = workshop.NewItemPos();
            var outputCell = GnomanEmpire.Instance.Map.GetCell(newItemPosition);
            return outputCell.Objects.Count;
        }
    }
}
