﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRoguelite.Utility
{
    internal interface IGameList <T>
    {
        public void Add(T item);
        public void Add(IEnumerable<T> items);
        public void Remove(T item);
        public bool Remove(int id);
        public T? GetItem(int id);
        public T[] GetAll();

        public void DebugPrintList();
    }
}
