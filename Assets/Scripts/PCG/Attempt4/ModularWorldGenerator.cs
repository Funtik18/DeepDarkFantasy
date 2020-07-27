using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModularWorldGenerator : MonoBehaviour
{
    public Module[] Modules;
    public Module StartModule;

    public Module EndModule;

    public int RoomCount = 5;

    int iterations = 400;
    IEnumerator Start()
    //void Start()
    {
        var startModule = (Module)Instantiate(StartModule, transform.position, transform.rotation);
        var pendingExits = new List<ModuleConnector>(startModule.GetExits());
        RoomCount--;
        while (RoomCount > 0)
        {
            var newExits = new List<ModuleConnector>();

            foreach (var pendingExit in pendingExits)
            {
                iterations = 10;
                while (iterations > 0)
                {
                    iterations--;
                    if (iterations == 0)
                        RoomCount = 0;
                    var newTag = GetRandom(pendingExit.Tags);
                    var newModulePrefab = GetRandomWithTag(Modules, newTag);
                    var newModule = (Module)Instantiate(newModulePrefab);
                    //
                    var newModuleExits = newModule.GetExits();
                    var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                    MatchExits(pendingExit, exitToMatch);
                    yield return new WaitForSeconds(0.1f);
                    if (newModule == null)
                    {
                        //print("sd");
                        newModuleExits = null;
                    }
                    else
                    {
                        //print(newTag);
                        if (newTag == "room")
                            RoomCount--;

                        //if (newExits != null)
                        newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
                        break;
                    }
                }
            }
            pendingExits = newExits;
            if (pendingExits.Count == 0)
            {
                
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
        return array[Random.Range(0, array.Length)];
    }


    private static Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch)
    {
        var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
        return GetRandom(matchingModules);
    }


    private static float Azimuth(Vector3 vector)
    {
        return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
    }
}
