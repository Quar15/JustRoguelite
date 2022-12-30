using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRoguelite.Utility
{
    interface IIdentifiable
    {
        private static uint _nextID { get; }
        void SetID(uint ID);
        uint GetID();
        void DebugLog(string? localization);
    }
}
