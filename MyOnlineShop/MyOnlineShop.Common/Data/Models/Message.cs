using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyOnlineShop.Common.Data.Models
{
    public class Message
    {
        private string serializedData;

        public Message(object data)
        {
            this.Data = data;
        }

        private Message()
        {
        }

        public int Id { get; private set; }

        public Type Type { get; private set; }

        public bool Published { get; private set; }

        public void MarkAsPublished()
        {
            this.Published = true;
        }

        [NotMapped]

        public object Data

        { // Make sure you ignore null values!

            get => JsonConvert.DeserializeObject(this.serializedData, this.Type);

            set 
            {
                this.Type = value.GetType();

                this.serializedData = JsonConvert.SerializeObject(value);
            }
        }
    }
}
