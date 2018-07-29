using System;
using System.Collections.Generic;

namespace Eve.StaticData.DTO
{
    public class TypeID
    {
        public long Id;
        public double BasePrice;
        public long Capacity;
        public List<Tuple<string, string>> Description;
        public long GraphicId;
        public long GroupId;
        public long IconId;
        public long MarketGroupId;
        public long Mass;
        public List<Tuple<string, string>> Name;
        public long PortionSize;
        public bool Published;
        public bool RaceId;
        public string SofFactionName;
        public long Radius;
        public long SoundId;
        public long Volume;
    }
}
