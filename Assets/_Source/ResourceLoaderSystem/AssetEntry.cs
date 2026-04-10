using UnityEngine;

namespace ResourceLoaderSystem 
{
    public struct AssetEntry
    {
        private Object asset;
        private int refCount;

        public AssetEntry(Object asset)
        {
            this.asset = asset;
            refCount = 1;
        }

        //don't use any other method to get assets!!!
        public Object GetAsset()
        {
            refCount++;
            return asset;
        }

        public int GetRefCount() => refCount;
        public void DecrementRefCount() => refCount--;
    }
}