namespace JustRoguelite.Utility
{
    interface IIdentifiable
    {
        private static uint _nextID { get; }
        void SetID(uint ID);
        uint GetID();
    }
}
