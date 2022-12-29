using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRoguelite.Utility
{
    interface IIdentifiable
    {
        private static int _nextID { get; }
        void SetID(int ID);
        int GetID();
        void DebugLog(string? localization);
    }
}
