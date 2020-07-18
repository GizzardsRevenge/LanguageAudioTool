using System;
using System.Xml;

namespace LanguageAudioTool
{
    public class Job
    {
        public enum Type
        {
            AddSection,
            AddSilence
        }

        private int _parameter;
        private Type _type;

        public Job()
        {
            _type = Type.AddSilence;
            _parameter = 0;
        }

        public Job(Type type, int parameter)
        {
            _type = type;
            _parameter = parameter;
        }

        public override string ToString()
        {
            switch (_type)
            {
                case Type.AddSection: return "Play the section at " + _parameter.ToString() + "% speed"; 
                case Type.AddSilence:
                {
                    if (Int32.Parse(_parameter.ToString()) == 1)
                        return "== Play 1 second of silence ==";
                    else
                        return "== Play " + _parameter.ToString() + " seconds of silence ==";
                }
            }

            // Should never get here
            return base.ToString();
        }

        public Type JobType
        {
            get { return _type; }
        }

        public int Parameter
        {
            get { return _parameter; }
        }

        public void FromXMLElement(XmlElement element)
        {
            _type = (Type)Enum.Parse(typeof(Type), element.GetAttribute("Type"));
            _parameter = int.Parse(element.GetAttribute("Parameter"));
        }

        public XmlElement ToXMLElement(XmlDocument doc)
        {
            // Note that this XML is for internal use, not meant for the end user to modify
            XmlElement root = doc.CreateElement("Job");

            root.SetAttribute("Type", _type.ToString());
            root.SetAttribute("Parameter", _parameter.ToString());

            return root;
        }
    }
}
