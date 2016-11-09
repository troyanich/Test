using System;

namespace SimpleLogger
{
	public interface IConfigurator
	{

		IConfigurator AddClass<TClass> (Logger.Level level, string color = "") where TClass : class;

		IConfigurator AddClass<TClass> (string color = "") where TClass : class;

		IConfigurator AddClass(Type type, string color = "");

		bool RemoveClass<TClass> () where TClass : class;
	}
}
