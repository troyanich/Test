using System;
using TinyJSON;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SimpleGelf.Core
{
    public sealed class GelfMessageSerializer
    {
        public string Serialize(GelfMessage message)
        {
            var duration = message.Timestamp.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
			Dictionary<string, object> dict = new Dictionary<string, object> 
			{
				{ "version", message.Version },
				{ "host", message.Host },
				{ "short_message", message.ShortMessage },
				{ "full_message", message.FullMessage },
				{ "timestamp", duration.TotalSeconds },
				{ "level", (int)message.Level }
			};

            foreach (var additionalField in message.AdditionalFields)
            {
                var key = additionalField.Key;
                var value = additionalField.Value;
				dict.Add(key.StartsWith("_") ? key : "_" + key, value);
            }

			return JSON.Dump (dict);

        }
    }
}