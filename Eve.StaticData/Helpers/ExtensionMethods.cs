using Eve.StaticData.DTO;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace Eve.StaticData.Helpers
{
    public static class ExtensionMethods
    {
        public static EveType ToEveType(this KeyValuePair<YamlNode, YamlNode> yamlNode)
        {
            return new EveType {
            };
        }
    }
}
