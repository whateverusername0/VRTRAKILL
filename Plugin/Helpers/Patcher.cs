using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Plugin.Helpers
{
    // Adding more levels of abstraction.
    // This is a custom patcher class that uses HarmonyX, which uses BepInEx.
    // If you wanna use it for your own purposes - you have my full permission :)
    // You can find the usage of this class in the Plugin.Plugin class
    public class Patcher
    {
        public Harmony Harmony { get; set; }
        public Assembly ASS { get; private set; } = Assembly.GetCallingAssembly();
        public string Namespace { get; set; } public Type Type { get; set; }
        public string[] Namespaces { get; set; } public List<Type> Types { get; set; } = new List<Type>();

        public Patcher(Harmony _Harmony)
        {
            Harmony = _Harmony;
        }
        public Patcher(Harmony _Harmony, string _Namespace)
        {
            Harmony = _Harmony;
            Namespace = _Namespace;
        }
        public Patcher(Harmony _Harmony, string[] _Namespaces)
        {
            Harmony = _Harmony;
            Namespaces = _Namespaces;
        }
        public Patcher(Harmony _Harmony, Type _Type)
        {
            Harmony = _Harmony;
            Type = _Type;
        }
        public Patcher(Harmony _Harmony, Type[] _Types)
        {
            Harmony = _Harmony;
            Types = _Types.ToList();
        }
        public Patcher(Harmony _Harmony, Assembly _ASS)
        {
            Harmony = _Harmony;
            ASS = _ASS;
        }
        public Patcher(Harmony _Harmony, Assembly _ASS, string _Namespace)
        {
            Harmony = _Harmony;
            ASS = _ASS;
            Namespace = _Namespace;
        }
        public Patcher(Harmony _Harmony, Assembly _ASS, string[] _Namespaces)
        {
            Harmony = _Harmony;
            ASS = _ASS;
            Namespaces = _Namespaces;
        }
        public Patcher(Harmony _Harmony, Assembly _ASS, Type _Type)
        {
            Harmony = _Harmony;
            ASS = _ASS;
            Type = _Type;
        }
        public Patcher(Harmony _Harmony, Assembly _ASS, Type[] _Types)
        {
            Harmony = _Harmony;
            ASS = _ASS;
            Types = _Types.ToList();
        }

        private List<Type> GetPatches(string _Namespace = null)
        {
            IEnumerable<Type> Q;
            if (_Namespace == null)
            {
                Q = from T in ASS.GetTypes()
                    where T.IsDefined(typeof(HarmonyPatch), false)
                    select T;
            }
            else
            {
                Q = from T in ASS.GetTypes()
                    where T.Namespace == _Namespace && T.IsDefined(typeof(HarmonyPatch), false)
                    select T;
            }
            return Q.ToList();
        }
        private List<Type> GetPatches(string[] _Namespaces)
        {
            List<Type> QL = new List<Type>();
            foreach (string _Namespace in _Namespaces)
                QL.AddRange(GetPatches(_Namespace));
            return QL;
        }
        public IEnumerable<Type> GetTypes(string _Namespace)
        { return GetPatches(_Namespace); }
        public IEnumerable<Type> GetTypes(string[] _Namespaces)
        { return GetPatches(_Namespaces); }

        public void PatchAll()
        {
            List<Type> L = new List<Type>();

            if (Namespace == null && Namespaces == null && Type == null && Types == null)
                try { Harmony.PatchAll(); } catch (NullReferenceException) { Plugin.PLog.LogFatal("Could not find any patches(???) wtf?"); }
            else
            {
                if (Namespace != null) L.AddRange(GetTypes(Namespace));
                if (Namespaces != null && Namespaces.Length > 0) L.AddRange(GetTypes(Namespaces));
                if (Type != null) L.Add(Type);
                if (Types != null && Types.Count > 0) L.AddRange(Types.ToArray());
                PatchAll(L.ToArray());
            }
        }
        

        public void PatchAll(string _Namespace)
        {
            IEnumerable<Type> Q = GetPatches(_Namespace);
            foreach (Type T in Q) try { Harmony.PatchAll(T); } catch { Plugin.PLog.LogError($"Nullref with type {T}"); }
        }
        public void PatchAll(string[] _Namespaces)
        {
            IEnumerable<Type> Q = GetPatches(_Namespaces);
            foreach (Type T in Q) try { Harmony.PatchAll(T); } catch { Plugin.PLog.LogError($"Nullref with type {T}"); }
        }
        public void PatchAll(Type _T)
        => Harmony.PatchAll(_T);
        public void PatchAll(Type[] _T)
        { foreach(Type T in _T) Harmony.PatchAll(T); }

        public void UnpatchAll()
        => Harmony.UnpatchSelf();
    }
}
