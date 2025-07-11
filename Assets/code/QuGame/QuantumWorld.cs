using UnityEngine;

namespace QuantumWorld
{
    /// <summary>
    /// Contains Axis based on quantum world.
    /// </summary>
    public static class QuAxis
    {
        /// <summary>
        /// The X axis in the quantum world is defined as the -Z axis in Unity.
        /// </summary>
        public static Vector3 X = new Vector3(0, 0, -1);
        /// <summary>
        /// The Y axis in the quantum world is defined as the X axis in Unity.
        /// </summary>
        public static Vector3 Y = new Vector3(1, 0, 0);
        /// <summary>
        /// The Z axis in the quantum world is defined as the Y axis in Unity.
        /// </summary>
        public static Vector3 Z = new Vector3(0, 1, 0);

        /// <summary>
        /// QUVector3 help to convert the Quantum Vector3 to Unity Vector3 .
        /// </summary>
        /// <param name="x">QuAxis X </param>
        /// <param name="y">QuAxis Y </param>
        /// <param name="z">QuAxis Z </param>
        /// <returns>The Unity Vector3 based on QuVector3</returns>
        public static Vector3 QuAxisToUnityAxis(Vector3 quAxis)
        {
            // Convert the quantum axis to Unity axis
            float x = quAxis.x;
            float y = quAxis.y;
            float z = quAxis.z;
            // The quantum world is right-handed, so we need to swap the axes accordingly
            // X -> -Z, Y -> X, Z -> Y
            return new Vector3(y, z, -x).normalized;
        }
    }

    public static class QuGate
    {
        /// <summary>
        /// This is all the qubit gates we used in the quantum world.
        /// </summary>
        public static string[] gateList = new string[]
        {
            "X", "Y", "Z", "X/2", "Y/2", "Z/2", "🔍", "H", "CX"
        };
    }
}

