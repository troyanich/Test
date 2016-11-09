using SimpleLogger.Logging;
using SimpleLogger.Logging.Formatters;
using UnityEngine;
using SimpleGelf.Core;
using System;
using System.Collections;
using System.Text;
using UniRx;
using System.Collections.Generic;

namespace SimpleGelf.GraylogLogger
{
	public class LogServerHandler : ILoggerHandler 
	{
		readonly GelfMessageSerializer _gelfSerializer = new GelfMessageSerializer();
		readonly UTF8Encoding encoding = new UTF8Encoding();
		readonly ILoggerFormatter _loggerFormatter;

		public LogServerHandler() : this(new DefaultLoggerFormatter()) { }

		public LogServerHandler(ILoggerFormatter loggerFormatter)
		{
			_loggerFormatter = loggerFormatter;
		}

		string _host;
		Dictionary<string, string> _headers;
		public LogServerHandler Configure(string host)
		{
			_host = host;
			var form = new WWWForm ();
			_headers = form.headers;
			_headers["Content-Type"] = "application/json";
			return this;
		}

		string _username;
		string _buildVersion;
		string _deviceName;
		string _deviceModel;
		string _deviceUniqueIdentifier;

		public LogServerHandler SetParameters(string username, string buildVersion, string deviceName, string deviceModel, string deviceUniqueIdentifier)
		{
			_username = username;
			_buildVersion = buildVersion;
			_deviceName = deviceName;
			_deviceModel = deviceModel;
			_deviceUniqueIdentifier = deviceUniqueIdentifier;
			return this;
		}

		void HandleLog(string logString, string stackTrace, LogType type) {
			if (type == LogType.Exception) {
				Debug.LogWarning ("Catch exception");
				var mes = string.Format ("{0} \n {1}", logString, stackTrace);
				PushMessage (encoding.GetBytes (mes));
			}
		}
		
		public void Publish(LogMessage logMessage)
		{
			if (string.IsNullOrEmpty (_host))
				return;

			var str = _loggerFormatter.ApplyFormat (logMessage);

			var builder = new GelfMessageBuilder (str, "Heroes Rage, " + Application.platform );

			var level = GelfLevel.Debug;
			if (logMessage.Level == SimpleLogger.Logger.Level.Error) {
				level = GelfLevel.Error;
			} else if (logMessage.Level == SimpleLogger.Logger.Level.Warning) {
				level = GelfLevel.Warning;
			}

			builder.SetLevel (level);
			builder.SetTimestamp (DateTime.Now);
	
			if (!string.IsNullOrEmpty (_username)) {
				builder.SetAdditionalField ("user_name", _username);
			}

			if (!string.IsNullOrEmpty (_buildVersion)) {
				builder.SetAdditionalField ("build", _buildVersion);
			}

			if (!string.IsNullOrEmpty (_deviceName)) {
				builder.SetAdditionalField ("device_name", _deviceName);
			}

			if (!string.IsNullOrEmpty (_deviceModel)) {
				builder.SetAdditionalField ("device_model", _deviceModel);
			}

			if (!string.IsNullOrEmpty (_deviceUniqueIdentifier)) {
				builder.SetAdditionalField ("device_id", _deviceUniqueIdentifier);
			}

			if (!string.IsNullOrEmpty (logMessage.CallingClass)) {
				builder.SetAdditionalField ("calling_class", logMessage.CallingClass);
			}

			if (!string.IsNullOrEmpty (logMessage.CallingMethod)) {
				builder.SetAdditionalField ("calling_method", logMessage.CallingMethod);
			}

			if (logMessage.LineNumber > 0) {
				builder.SetAdditionalField ("line_number", logMessage.LineNumber.ToString ());
			}

			var mes = _gelfSerializer.Serialize (builder.ToMessage ());
		
			Debug.Log ("Mes = " + mes);

			PushMessage (encoding.GetBytes (mes));
		}


		void PushMessage(byte[] postData)
		{
			ObservableWWW.Post (_host, postData, _headers).ObserveOnMainThread ().Subscribe (mes => {}, ex =>  Debug.LogError ("Logserver, ex = " + ex));
		}

	}
}

