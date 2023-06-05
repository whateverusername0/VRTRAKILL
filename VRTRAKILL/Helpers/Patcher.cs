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
        public Assembly ASS { get; private set; }
        public string Namespace { get; private set; } public Type Type { get; private set; }
        public string[] Namespaces { get; private set; } public Type[] Types { get; private set; }

        // That's a LOT of constructors that i'm not so proud of
        public Patcher(Harmony _Harmony)
        {
            Harmony = _Harmony;
            ASS = Assembly.GetCallingAssembly();
        }
        public Patcher(Harmony _Harmony, Assembly _ASS)
        {
            Harmony = _Harmony;
            ASS = _ASS;
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
            Types = _Types;
        }

        private List<Type> GetPatches(string _Namespace = null)
        {
            IEnumerable<Type> Q;
            if (Namespace == null)
            {
                Q = from T in ASS.GetTypes()
                    where T.IsClass && T.IsDefined(typeof(HarmonyPatch), true)
                    select T;
            }
            else
            {
                Q = from T in ASS.GetTypes()
                    where T.IsClass && T.Namespace == _Namespace && T.IsDefined(typeof(HarmonyPatch), true)
                    select T;
            }
            return Q.ToList<Type>();
        }
        private List<Type> GetPatches(string[] _Namespaces)
        {
            List<Type> QL = new List<Type>();
            foreach (string _Namespace in _Namespaces)
            {
                IEnumerable<Type> Q = from T in ASS.GetTypes()
                                      where T.IsClass && T.Namespace == _Namespace && T.IsDefined(typeof(HarmonyPatch), true)
                                      select T;
                QL.AddRange(Q.ToList<Type>());
            }
            return QL;
        }

        public void PatchAll()
        {
            // this looks like something YandereDev would do but it works
            if (Namespace != null) PatchAll(Namespace);
            if (Namespaces != null) PatchAll(Namespaces);
            if (Type != null) PatchAll(Type);
            if (Types != null) PatchAll(Types);
        }
        public void PatchAll(string _Namespace)
        {
            IEnumerable<Type> Q = GetPatches(Namespace);
            foreach (Type T in Q) Harmony.PatchAll(T);
        }
        public void PatchAll(string[] _Namespaces)
        {
            IEnumerable<Type> Q = GetPatches(Namespaces);
            foreach (Type T in Q) Harmony.PatchAll(T);
        }
        public void PatchAll(Type _T)
        => Harmony.PatchAll(_T);
        public void PatchAll(Type[] _T)
        { foreach(Type T in _T) Harmony.PatchAll(T); }

        public void UnpatchAll()
        => Harmony.UnpatchSelf();
    }
}
