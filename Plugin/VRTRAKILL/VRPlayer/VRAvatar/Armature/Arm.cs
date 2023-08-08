using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature
{
    public enum ArmType
    {
        Feedbacker,
        Knuckleblaster,
        Spear,
        Whiplash,
        Sandboxer
    }

    public class Arm
    {
        public ArmType Type { get; set; }
        public Transform GameObjecT { get; set; }
        public Transform Root { get; set; }
        public Transform Clavicle { get; set; }
        public Transform UpperArm { get; set; }
        public Transform Forearm { get; set; }

        public Hand Hand { get; set; }

        public static Arm FeedbackerPreset(Transform GameObjectT)
        {
            try
            {
                Arm A = new Arm
                {
                    Type = ArmType.Feedbacker,
                    GameObjecT = GameObjectT,
                    Root = GameObjectT.GetChild(0).GetChild(0)
                };
                A.Clavicle = A.Root;
                A.UpperArm = A.Clavicle.GetChild(0);
                A.Forearm = A.UpperArm.GetChild(0);

                A.Hand = Hand.FeedbackerPreset(A.Forearm.GetChild(0));
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }
        public static Arm KnuckleblasterPreset(Transform GameObjectT)
        {
            try
            {
                Arm A = new Arm
                {
                    Type = ArmType.Knuckleblaster,
                    GameObjecT = GameObjectT,
                    Root = GameObjectT.GetChild(1)
                };
                A.Clavicle = A.Root.GetChild(1);
                A.UpperArm = A.Clavicle.GetChild(0);
                A.Forearm = A.Clavicle.GetChild(1);

                A.Hand = Hand.KnuckleblasterPreset(A.Forearm.GetChild(0));
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }
        public static Arm SpearPreset(Transform T)
        {
            try
            {
                Arm A = new Arm
                {
                    Type = ArmType.Spear,
                    GameObjecT = T
                };
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }
        public static Arm WhiplashPreset(Transform GameObjectT)
        {
            try
            {
                Arm A = new Arm
                {
                    Type = ArmType.Whiplash,
                    GameObjecT = GameObjectT,
                    Root = GameObjectT.GetChild(0).GetChild(1)
                };
                A.Clavicle = A.Root.GetChild(1);
                A.UpperArm = A.Clavicle.GetChild(0);
                A.Forearm = A.Clavicle.GetChild(1);

                A.Hand = Hand.FeedbackerPreset(A.Forearm.GetChild(0));
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }
        public static Arm SandboxerPreset(Transform GameObjectT)
        {
            try
            {
                Arm A = new Arm
                {
                    Type = ArmType.Sandboxer,
                    GameObjecT = GameObjectT,
                    Root = GameObjectT.GetChild(0)
                };
                A.Clavicle = A.Root;
                A.UpperArm = A.Clavicle.GetChild(0);
                A.Forearm = A.UpperArm.GetChild(0);

                A.Hand = Hand.FeedbackerPreset(A.Forearm.GetChild(0));
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }

        public static Arm MRFeedbackerPreset(Transform GameObjectT)
        {
            try
            {
                Arm A = new Arm
                {
                    Type = ArmType.Feedbacker,
                    GameObjecT = GameObjectT,
                    Root = GameObjectT.GetChild(1)
                };
                A.Clavicle = A.Root.GetChild(0);
                A.UpperArm = A.Clavicle.GetChild(0);
                A.Forearm = A.UpperArm.GetChild(0);
                A.Hand = Hand.MRFeedbackerPreset(A.Forearm.GetChild(0));
                return A;
            }
            catch (System.Exception E)
            {
                Plugin.PLog.LogError($"{E.Message}\n{E.Source}");
                return null;
            }
        }
        public static Arm MRKnuckleblasterPreset(Transform GameObjectT)
        {
            Arm A = new Arm
            {
                Type = ArmType.Knuckleblaster,
                GameObjecT = GameObjectT,
                Root = GameObjectT.GetChild(1)
            };
            A.Clavicle = A.Root.GetChild(0);
            A.UpperArm = A.Clavicle.GetChild(0);
            A.Forearm = A.UpperArm.GetChild(0);
            A.Hand = Hand.MRKnuckleblasterPreset(A.Forearm.GetChild(0));
            return A;
        }
        public static Arm MRWhiplashPreset(Transform GameObjectT)
        {
            Arm A = new Arm
            {
                Type = ArmType.Whiplash,
                GameObjecT = GameObjectT,
                Root = GameObjectT.GetChild(1)
            };
            A.Clavicle = A.Root.GetChild(0);
            A.UpperArm = A.Clavicle.GetChild(0);
            A.Forearm = A.UpperArm.GetChild(0);
            A.Hand = Hand.MRFeedbackerPreset(A.Forearm.GetChild(0));
            return A;
        }
        public static Arm MRSpearPreset(Transform GameObjectT)
        {
            Arm A = new Arm
            {
                Type = ArmType.Spear,
                GameObjecT = GameObjectT
            };
            return A;
        }
        public static Arm MRSandboxerPreset(Transform GameObjectT)
        {
            Arm A = new Arm
            {
                Type = ArmType.Sandboxer,
                GameObjecT = GameObjectT,
                Root = GameObjectT.GetChild(1)
            };
            A.Clavicle = A.Root.GetChild(0);
            A.UpperArm = A.Clavicle.GetChild(0);
            A.Forearm = A.UpperArm.GetChild(0);
            A.Hand = Hand.MRFeedbackerPreset(A.Forearm.GetChild(0));
            return A;
        }
    }
}
