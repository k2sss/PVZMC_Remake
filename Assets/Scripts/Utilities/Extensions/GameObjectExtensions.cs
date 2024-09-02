using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MyClass.Extensions.TransformExtension
{
    public static class GameObjectExtensions
    {

        public static List<Mob> GetNearbyPlants(this Transform gobject, float Range)
        {
            GameObject[] allPlants = GameObject.FindGameObjectsWithTag("Plants");
            List<Mob> nearbyPlants = new List<Mob>();
            foreach (var item in allPlants)
            {
                if ((item.transform.position - gobject.position).magnitude < Range)
                {
                    nearbyPlants.Add(item.GetComponent<Mob>());
                }
            }

            Mob player = GameObject.FindGameObjectWithTag("Player").GetComponent<Mob>();
            if ((player.transform.position - gobject.position).magnitude < Range)
                nearbyPlants.Add(player);


            return nearbyPlants;
        }


    }
}

