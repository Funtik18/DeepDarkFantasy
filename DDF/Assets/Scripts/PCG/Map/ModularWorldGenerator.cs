using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace DDF.PCG.MAP
{
    public class ModularWorldGenerator : MonoBehaviour
    {
        public Module[] Modules;
        public Module StartModule;

        public Module EndModule;

        public int RoomCount = 5;

        static bool bossroom = false;
        int iterations = 400;
        IEnumerator Start()
        //void Start()
        {
            float TimeToStop = 0;
            var startModule = (Module)Instantiate(StartModule, transform.position, transform.rotation);
            var pendingExits = new List<ModuleConnector>(startModule.GetExits());
            RoomCount--;
            while (RoomCount > 0)////////////может уйти в бесконечность
            {
                var newExits = new List<ModuleConnector>();

                foreach (var pendingExit in pendingExits)
                {
                    iterations = 25;
                    while (iterations > 0)
                    {
                        iterations--;

                        var newTag = GetRandom(pendingExit.Tags);

                        var newModulePrefab = GetRandomWithTag(Modules, newTag);
                        var newModule = (Module)Instantiate(newModulePrefab);
                        //
                        var newModuleExits = newModule.GetExits();
                        var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                        MatchExits(pendingExit, exitToMatch);
                        yield return new WaitForSeconds(0.02f);
                        if (iterations == 1)
                        {
                            newModulePrefab = EndModule;
                            newModule = (Module)Instantiate(newModulePrefab);

                            newModuleExits = newModule.GetExits();
                            exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                            MatchExits(pendingExit, exitToMatch);
                        }

                        if (newModule == null)
                        {
                            //print("sd");
                            newModuleExits = null;
                        }
                        else
                        {
                            //print(newTag);
                            if (newTag == "room")
                            {
                                RoomCount--;
                                TimeToStop = 0;
                            }
                            if (newTag == "BossRoom")
                            {
                                TimeToStop = 0;
                                RoomCount--;
                                //Modules=Modules.Except(new Module[] { newModulePrefab }.ToArray()));
                                /*var newModuleArr = Modules.ToList();
                                newModuleArr.Remove(newModulePrefab);
                                Array.Clear(Modules,0,Modules.Length);
                                Modules = newModuleArr.ToArray();*/
                                print("Boss has been spawned");
                                bossroom = true;
                                Modules = Modules.Where(w => w != newModulePrefab).ToArray();

                                foreach (var item in Modules)
                                {
                                    item.RemoveTags("BossRoom");
                                }
                            }

                            //if (newExits != null)
                            newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
                            break;
                        }
                    }

                }
                pendingExits = newExits;
                TimeToStop += Time.deltaTime;
                if (TimeToStop > 30)
                {
                    break;
                }
            }
            foreach (var pendingExit in pendingExits)
            {

                var newModulePrefab = EndModule;
                var newModule = (Module)Instantiate(newModulePrefab);

                var newModuleExits = newModule.GetExits();
                var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                MatchExits(pendingExit, exitToMatch);

            }
        }


        private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit)
        {

            var newModule = newExit.transform.parent;
            var forwardVectorToMatch = -oldExit.transform.forward;
            var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
            newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
            var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
            newModule.transform.position += correctiveTranslation;
            //newExit.CheckExit();
        }


        private static TItem GetRandom<TItem>(TItem[] array)
        {
            Debug.Log(array + " " + array.Length);
            return array[UnityEngine.Random.Range(0, array.Length)];

        }


        private static Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch)
        {
            if (!bossroom)
            {
                var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
                Debug.Log(tagToMatch);
                return GetRandom(matchingModules);
            }
            if (bossroom)
            {
                var matchingModules = modules.Where(m => !m.Tags.Contains(tagToMatch)).ToArray();
                Debug.Log(tagToMatch);
                return GetRandom(matchingModules);
            }
            return null;
        }


        private static float Azimuth(Vector3 vector)
        {
            return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
        }
    }
}