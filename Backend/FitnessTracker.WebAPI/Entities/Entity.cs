namespace FitnessTracker.WebAPI.Entities
{
    public class Entity
    {
        public DateTime CreationDate { get; set; }
        public DateTime ModificationTime { get; set; }

        public Entity()
        {
            CreationDate = DateTime.Now;
            ModificationTime = DateTime.Now;
        }
    }
}
