using System.Collections.Generic;
using System;


namespace SimpleLogger.Logging
{
	internal class LogPublisher : ILoggerHandlerManager, IConfigurator
    {
        private readonly IList<ILoggerHandler> _loggerHandlers;
        private readonly IList<LogMessage> _messages;
		private readonly IDictionary<string, string> _allowedClasses = new Dictionary<string, string> ();

        public LogPublisher()
        {
            _loggerHandlers = new List<ILoggerHandler>();
            _messages = new List<LogMessage>();
			AllowedAll = true;
        }

		internal bool AllowedAll { get; set; }

		internal bool AllowedColor { set; get; }

		public IConfigurator AddClass<TClass> (Logger.Level level, string color = "") where TClass : class
		{
			AddClass<TClass> (color);
			return this;
		}

		public IConfigurator AddClass<TClass> (string color = "") where TClass : class
		{
			Type typeFromHandle = typeof(TClass);
			if (!_allowedClasses.ContainsKey (typeFromHandle.Name)) {
				_allowedClasses.Add (typeFromHandle.Name, color);
				AllowedAll = false;
			}
			return this;
		}

		public IConfigurator AddClass(Type type, string color = "")
		{
			if (type == null)
				return this;

			if (!_allowedClasses.ContainsKey (type.Name)) {
				_allowedClasses.Add (type.Name, color);
				AllowedAll = false;
			}
			return this;
		}

		public bool RemoveClass<TClass> () where TClass : class
		{
			Type typeFromHandle = typeof(TClass);
			return _allowedClasses.ContainsKey (typeFromHandle.Name) && _allowedClasses.Remove (typeFromHandle.Name);
		}

        public void Publish(LogMessage logMessage)
        {
			if (!_allowedClasses.ContainsKey (logMessage.CallingClass))
				return;

			if (AllowedColor) {
				logMessage.Color = _allowedClasses [logMessage.CallingClass];
			}
            _messages.Add(logMessage);
            foreach (var loggerHandler in _loggerHandlers)
                loggerHandler.Publish(logMessage);
        }

        public ILoggerHandlerManager AddHandler(ILoggerHandler loggerHandler)
        {
            if (loggerHandler != null)
                _loggerHandlers.Add(loggerHandler);
            return this;
        }

        public bool RemoveHandler(ILoggerHandler loggerHandler)
        {
            return _loggerHandlers.Remove(loggerHandler);
        }

        public IEnumerable<LogMessage> Messages
        {
            get { return _messages; }
        }
    }
}
