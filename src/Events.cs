using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariousEntity;

namespace Events
{
    public abstract class Event
    {
        public string Name {  get; private set; }
        public string Description { get; private set; }
        public Event(string name, string description) {
            Name = name;
            Description = description;
        }
    }
}
