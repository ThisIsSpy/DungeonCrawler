using System.Collections.Generic;
using UnityEngine;

namespace ResourceLoaderSystem 
{
    public static class ResourceLoader
    {
        private static readonly Dictionary<string, AssetEntry> cache = new();
        private static readonly Dictionary<string, ResourceRequest> requestQueue = new();

        public static Object Load(string path)
        {
            if (cache.TryGetValue(path, out AssetEntry loadedEntry)) 
            {
                Debug.Log($"asset at path {path} | refCount: {loadedEntry.GetRefCount() + 1}");
                return loadedEntry.GetAsset();
            } 

            Object loadedObject = Resources.Load(path);
            if (loadedObject == null) throw new System.Exception($"no object found at path {path}");
            AssetEntry newEntry = new(loadedObject);
            cache.Add(path, newEntry);
            return loadedObject;
        }

        public static T Load<T>(string path) where T : Object
        {
            Object loadedObject = Load(path);
            if (loadedObject is not T) throw new System.Exception("your loaded object doesnt match the needed type");
            return (T)loadedObject;
        }

        public static async Awaitable<Object> LoadAsync(string path)
        {
            if (cache.TryGetValue(path, out AssetEntry loadedEntry)) return loadedEntry.GetAsset();

            if (requestQueue.TryGetValue(path, out ResourceRequest cachedRequest))
            {
                await cachedRequest;
                Object loadedFromCachedRequestObject = cachedRequest.asset;
                AssetEntry newEntryFromCache = new(loadedFromCachedRequestObject);
                cache.Add(path, newEntryFromCache);
                return newEntryFromCache.GetAsset();
            }

            ResourceRequest request = Resources.LoadAsync(path);
            requestQueue.Add(path, request);
            request.completed += op => requestQueue.Remove(path);
            await request;
            Object loadedFromRequestObject = request.asset;
            AssetEntry newEntry = new(loadedFromRequestObject);
            cache.Add(path, newEntry);
            return newEntry.GetAsset();
        }

        public static async Awaitable<T> LoadAsync<T>(string path) where T : Object
        {
            Object loadedObject = await LoadAsync(path);
            if (loadedObject is not T) throw new System.Exception("your async loaded object doesnt match the needed type");
            return (T)loadedObject;
        }

        public static void Release(string path)
        {
            if(!cache.ContainsKey(path)) throw new System.Exception($"there is no file at path {path} to release");
            Object objectToRemove = cache[path].GetAsset();
            cache.Remove(path);
            Resources.UnloadAsset(objectToRemove);
        }

        public static void DisposeReference(string path)
        {
            if (!cache.ContainsKey(path)) throw new System.Exception($"there is no file at path {path} to release");
            cache[path].DecrementRefCount();
            if (cache[path].GetRefCount() < 1) Release(path);
        }
    }
}
