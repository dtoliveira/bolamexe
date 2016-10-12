using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TopSunday.SK
{
    public abstract class SKController : Controller 
    {
        _Alerts alerts = null;
        public _Alerts Alerts
        {
            get
            {
                if (alerts == null) alerts = new _Alerts(this.TempData);
                return alerts;
            }
        }

        public class _Alerts
        {
            TempDataDictionary tempData = null;
            public _Alerts(TempDataDictionary tempData)
            {
                this.tempData = tempData;
            }

            public void Danger(string message)
            {
                if (tempData.ContainsKey(AlertType.DANGER))
                {
                    ((List<string>)tempData[AlertType.DANGER]).Add(message);
                }
                else
                {
                    tempData.Add(AlertType.DANGER, new List<string> { message });
                }
            }

            public void Info(string message)
            {
                if (tempData.ContainsKey(AlertType.INFO))
                {
                    ((List<string>)tempData[AlertType.INFO]).Add(message);
                }
                else
                {
                    tempData.Add(AlertType.INFO, new List<string> { message });
                }
            }

            public void Success(string message)
            {
                if (tempData.ContainsKey(AlertType.SUCCESS))
                {
                    ((List<string>)tempData[AlertType.SUCCESS]).Add(message);
                }
                else
                {
                    tempData.Add(AlertType.SUCCESS, new List<string> { message });
                }
            }

            public void Warning(string message)
            {
                if (tempData.ContainsKey(AlertType.WARNING))
                {
                    ((List<string>)tempData[AlertType.WARNING]).Add(message);
                }
                else
                {
                    tempData.Add(AlertType.WARNING, new List<string> { message });
                }
            }
        }
    }
}