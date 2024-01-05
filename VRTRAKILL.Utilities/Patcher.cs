using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace VRTRAKILL.Utilities
{
    // If you wanna use it for your own purposes - you have my full permission :)
    // You can find the usage of this class in the Plugin.Plugin class

    /// <summary>
    /// Uses BepInEx'd HarmonyX to simplify patching in plugins/mods.
    /// </summary>
    public class Patcher
    {
        public Harmony Harmony { get; set; }
        public Assembly Ass { get; set; } = Assembly.GetCallingAssembly();

        public string Namespace { get; set; } 
        public string[] Namespaces { get; set; } 
        public Type Type { get; set; }
        public Type[] Types { get; set; }

        public BepInEx.Logging.ManualLogSource Log { get; set; }

        public Patcher(Harmony _Harmony) { Harmony = _Harmony; }
        public Patcher(Harmony _Harmony, string _Namespace) : this(_Harmony) { Namespace = _Namespace; }
        public Patcher(Harmony _Harmony, string[] _Namespaces) : this(_Harmony) { Namespaces = _Namespaces; }
        public Patcher(Harmony _Harmony, Type _Type) : this(_Harmony) { Type = _Type; }
        public Patcher(Harmony _Harmony, Type[] _Types) : this(_Harmony) { Types = _Types.ToArray(); }
        public Patcher(Harmony _Harmony, Assembly _ASS) : this(_Harmony) { Ass = _ASS; }
        public Patcher(Harmony _Harmony, Assembly _ASS, string _Namespace) : this(_Harmony, _ASS) { Namespace = _Namespace; }
        public Patcher(Harmony _Harmony, Assembly _ASS, string[] _Namespaces) : this(_Harmony, _ASS) { Namespaces = _Namespaces; }
        public Patcher(Harmony _Harmony, Assembly _ASS, Type _Type) : this(_Harmony, _ASS) { Type = _Type; }
        public Patcher(Harmony _Harmony, Assembly _ASS, Type[] _Types) : this(_Harmony, _ASS) { Types = _Types.ToArray(); }

        private List<Type> GetPatches(string _Namespace = null)
        {
            IEnumerable<Type> Q;
            if (_Namespace == null)
            {
                Q = from T in Ass.GetTypes()
                    where T.IsDefined(typeof(HarmonyPatch), false)
                    select T;
            }
            else
            {
                Q = from T in Ass.GetTypes()
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
        private IEnumerable<Type> GetTypes(string _Namespace)
        { return GetPatches(_Namespace); }
        private IEnumerable<Type> GetTypes(string[] _Namespaces)
        { return GetPatches(_Namespaces); }

        public void PatchAll()
        {
            List<Type> L = new List<Type>();

            if (Namespace == null && Namespaces == null && Type == null && Types == null)
                try { Harmony.PatchAll(); } catch (NullReferenceException) { Log.LogFatal("Could not find any patches(???) wtf?"); }
            else
            {
                if (Type != null) L.Add(Type);
                if (Types != null && Types.Length > 0) L.AddRange(Types.ToArray());
                if (Namespace != null) L.AddRange(GetTypes(Namespace));
                if (Namespaces != null && Namespaces.Length > 0) L.AddRange(GetTypes(Namespaces));
                Patch(L.ToArray());
            }
        }
        

        public void Patch(string _Namespace)
        {
            Log.LogInfo($"Patching Namespace \"{_Namespace}\"...");
            IEnumerable<Type> Q = GetTypes(_Namespace);
            foreach (Type T in Q) try { Harmony.PatchAll(T); } catch { Log.LogError($"Nullref with type {T}"); }
        }
        public void Patch(string[] _Namespaces)
        {
            IEnumerable<Type> Q = GetTypes(_Namespaces);
            foreach (Type T in Q) try { Harmony.PatchAll(T); } catch { Log.LogError($"Nullref with type {T}"); }
        }
        public void Patch(Type _T)
        {
            Log.LogInfo($"Patching Type \"{nameof(_T)}\"...");
            Harmony.PatchAll(_T);
        }
        public void Patch(Type[] _T)
        { foreach(Type T in _T) Harmony.PatchAll(T); }

        public void UnpatchAll()
        => Harmony.UnpatchSelf();
    }
}
