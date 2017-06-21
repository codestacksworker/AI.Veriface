using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;

namespace FaceSysByMvvm.Model
{
    public class WarningMessageModel
    {
        ObservableCollection<MyCmpFaceLogWidthImgModel> _compareLogDatas;
        public ObservableCollection<MyCmpFaceLogWidthImgModel> CompareLogDatas
        {
            get
            {
                if (_compareLogDatas.Count > 100)
                {
                    try
                    {
                        int i = _compareLogDatas.Count - 1;
                        do
                        {
                            _compareLogDatas.RemoveAt(i);
                            i--;
                        } while (i > 50);
                    }
                    catch (System.Exception ex)
                    {
                        string err = ex.Message;
                    }
                }

                return _compareLogDatas;
            }
            set
            {
                _compareLogDatas = value;
            }
        }

        MyCmpFaceLogWidthImgModel _compareLogData;
        public MyCmpFaceLogWidthImgModel CompareLogData
        {
            get
            {
                return _compareLogData;
            }
            set
            {
                _compareLogData = value;
            }
        }

        IList _curCompareLogDatas
            = new ObservableCollection<MyCmpFaceLogWidthImgModel>();
        public IList CurCompareLogDatas
        {
            get { return _curCompareLogDatas; }
            set { _curCompareLogDatas = value; }
        }
        

        int _flag;
        public int Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
    }
}
