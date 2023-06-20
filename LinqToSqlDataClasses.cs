using System.Collections.Generic;

namespace ACOfferMaker
{
    partial class DeviceSet
    {
        public List<Device> deviceSetList { get; set; }
        public void AddToDeviceSet(Device device) 
        {
            deviceSetList = new List<Device>();
            deviceSetList.Add(device);
        }
    }

    partial class Offer
    {
        
    }

    partial class Device
    {
        // creating property to view full info in list box
        public string FullInfo 
        {
            get
            {
                return $"{this.ProducerName} - {this.ModelFullName}";
            }
        }
    }
}