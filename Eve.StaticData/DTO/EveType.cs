using System;
using System.Collections.Generic;

namespace Eve.StaticData.DTO
{
    public class EveType
    {
        public long Id { get; set; }
        public double BasePrice { get; set; }
        public long Capacity { get; set; }
        public List<Tuple<string, string>> Description { get; set; }
        public long GraphicId { get; set; }
        public long GroupId { get; set; }
        public long IconId { get; set; }
        public long MarketGroupId { get; set; }
        public long Mass { get; set; }
        public List<Tuple<string, string>> Name { get; set; }
        public long PortionSize { get; set; }
        public bool Published { get; set; }
        public bool RaceId { get; set; }
        public string SofFactionName { get; set; }
        public long Radius { get; set; }
        public long SoundId { get; set; }
        public long Volume { get; set; }
    }
}
