using UnityEngine;

namespace Game.Utitlits
{
    public class Utilits
    {
        private static RaycastHit[] hits = new RaycastHit[10];
        private static Vector3 tempPosition;
        public static Vector3 GetPointFromCamera(Camera camera, Vector3 position, out bool checkUi)
        {
            RaycastHit[] hits = new RaycastHit[10];
            /*position.z = camera.nearClipPlane;*/
            if (Physics.RaycastNonAlloc(camera.ScreenPointToRay(position), hits, float.MaxValue, (1<<5)|(1<<31)) > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider == null)
                    {
                        checkUi = true;
                    }
                    else
                    {
                        tempPosition = hits[i].transform.position;
                    }
                }

                checkUi = false;
                return tempPosition;
            }

            checkUi = false;
            return camera.ScreenToWorldPoint(position);
        }
        
        
        
        public static Vector3 GetPointFromCamera(Camera camera, Vector3 position)
        {
            /*position.z = camera.nearClipPlane;*/
            if (Physics.Raycast(camera.ScreenPointToRay(position), out RaycastHit hit , float.MaxValue))
            {
                return hit.point;
            }
            return camera.ScreenToWorldPoint(position);
        }
        
        public static Vector3 GetPointFromCamera(Camera camera, Vector3 position, out RaycastHit hit, LayerMask layer)
        {
            if (Physics.Raycast(camera.ScreenPointToRay(position), out hit , float.MaxValue, layer))
            {
                return hit.point;
            }
            return camera.ScreenToWorldPoint(position);
        }

        public static string ConvertMoneyValueToString(int moneyValue)
        {
            string currency = "";
            if (moneyValue > 1000000000)
            {
                currency = $"{moneyValue / 1000000}KK";
            }
            else if (moneyValue > 1000000)
            {
                currency = $"{moneyValue / 1000}K";
            }
            else
            {
                currency = $"{moneyValue}";
            }
            return currency;
        }
    }
}