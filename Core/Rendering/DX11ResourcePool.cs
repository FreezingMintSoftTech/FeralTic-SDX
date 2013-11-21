﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;

namespace FeralTic.DX11
{
    public class DX11ResourcePoolEntry<T> where T : class , IDisposable
    {
        private bool islocked;
        public bool IsLocked { get { return this.islocked; } }

        public T Element { get; private set; }

        public DX11ResourcePoolEntry(T element)
        {
            this.Element = element;
            this.Lock();
        }

        public void Lock()
        {
            this.islocked = true;
        }

        public void UnLock()
        {
            this.islocked = false;
        }

        public static implicit operator T(DX11ResourcePoolEntry<T> entry)
        {
            return entry.Element;
        }
    }


    public class DX11ResourcePool<T> : IDisposable where T : class, IDisposable
    {
        protected List<DX11ResourcePoolEntry<T>> pool = new List<DX11ResourcePoolEntry<T>>();
        protected DX11Device device;


        public DX11ResourcePool(DX11Device device)
        {
            this.device = device;
        }

        public int Count
        {
            get { return this.pool.Count; }
        }

        public void UnlockAll()
        {
            foreach (DX11ResourcePoolEntry<T> entry in this.pool)
            {
                if (entry.IsLocked) { entry.UnLock(); }
            }
        }

        public bool UnLock(T resource)
        {
            foreach (DX11ResourcePoolEntry<T> entry in this.pool)
            {
                if (entry.Element == resource && entry.IsLocked)
                {
                    entry.UnLock();
                    return true;
                }
            }
            return false;
        }

        public void ClearUnlocked()
        {
            List<DX11ResourcePoolEntry<T>> todelete = new List<DX11ResourcePoolEntry<T>>();
            foreach (DX11ResourcePoolEntry<T> entry in this.pool)
            {
                if (!entry.IsLocked) { todelete.Add(entry); }
            }

            foreach (DX11ResourcePoolEntry<T> entry in todelete)
            {
                this.pool.Remove(entry);
                entry.Element.Dispose();
            }
        }

        public void Dispose()
        {
            foreach (DX11ResourcePoolEntry<T> entry in this.pool)
            {
                entry.Element.Dispose();
            }
            this.pool.Clear();
        }
    }
}
