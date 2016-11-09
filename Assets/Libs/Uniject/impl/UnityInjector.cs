using System;
using Ninject;
using Ninject.Injection;
using UnityEngine;
using Uniject.Impl;
using System.Collections.Generic;
using Ninject.Modules;

public class UnityInjector
{
    private static IKernel kernel;
   
    public static IKernel Get() {
        if (kernel == null) 
			kernel = GetNewKernel ();
        return kernel;
    }

	static IKernel GetNewKernel()
	{
		var k = new StandardKernel (new UnityNinjectSettings (), new Ninject.Modules.INinjectModule[] {
			new UnityModule (),
			new LateBoundModule(),
		} );

//		GameObject listener = new GameObject();
//		UnityEngine.Object.DontDestroyOnLoad(listener);
//		listener.name = "LevelLoadListener";
//		listener.AddComponent<LevelLoadListener>();

		return k;
	}



	public static IKernel Get(INinjectModule[] modules) {
		if (kernel == null) 
			kernel = GetNewKernel (modules);

		return kernel;
	}   

	public static void RecreateKernel()
	{
		kernel = GetNewKernel ();
	}

	public static void RecreateKernel(INinjectModule[] modules)
	{
		kernel = GetNewKernel (modules);
	}


	public static IKernel GetNewKernel(INinjectModule[] modules)
	{
		var mods = new INinjectModule[] {
			new UnityModule (),
			new LateBoundModule (),
		};

		var allModules = new List<INinjectModule> (mods);
		allModules.AddRange (modules);

		var k = new StandardKernel (new UnityNinjectSettings (), allModules.ToArray ());

//		GameObject listener = new GameObject();
//		UnityEngine.Object.DontDestroyOnLoad(listener);
//		listener.name = "LevelLoadListener";
//		listener.AddComponent<LevelLoadListener>();

		return k;
	}



    public static object levelScope = new object();
    private static object scoper(Ninject.Activation.IContext context) {
        return levelScope;
    }
}

