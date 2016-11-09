﻿using System;
using System.Collections.Generic;

namespace SimpleGelf.Core
{
    public sealed class GelfMessageBuilder
    {
        private readonly Dictionary<string, string> additionalFields = new Dictionary<string, string>();
        private readonly string message;
        private readonly string host;
        private DateTime? timestamp;
        private GelfLevel? level;

        public GelfMessageBuilder(string message, string host)
        {
            this.message = message;
            this.host = host;
        }

       
        public GelfMessageBuilder SetAdditionalField(string key, string value)
        {
			if (!additionalFields.ContainsKey (key)) {
				additionalFields.Add (key, value);
			}
            return this;
        }

        
        public GelfMessageBuilder SetTimestamp(DateTime value)
        {
            timestamp = value;
            return this;
        }

        
        public GelfMessageBuilder SetLevel(GelfLevel value)
        {
            level = value;
            return this;
        }

        
        public GelfMessage ToMessage()
        {
            if(timestamp == null)
                throw new ArgumentNullException();
            if(level == null)
                throw new ArgumentNullException();

            return new GelfMessage
                {
                    Host = host,
                    FullMessage = message,
					ShortMessage = Truncate(message, 200),
                    Level = level.Value,
                    Timestamp = timestamp.Value,
                    AdditionalFields = additionalFields
                };
        }

		string Truncate(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length <= maxLength ? value : value.Substring(0, maxLength); 
		}
    }
}