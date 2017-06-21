using System.Threading.Tasks;
using DATA.MODELS.GlobalModels;

namespace FaceSysByMvvm.Common
{
    public class AppConfigs
    {
        public static async void AsyncSelectFaceType()
        {
            ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
            await Task.Run(() =>
            {
                GlobalCache.FaceTypeList = thirft.QueryDefFaceObjType();
            }).ConfigureAwait(false);
        }
    }
}

