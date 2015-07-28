using System;
using Acr.UserDialogs;
using Xamarin.Forms;
using Geofence.Plugin.Abstractions;

namespace Mais
{
    public class CrossGeofenceListener : IGeofenceListener
    {
        #region IGeofenceListener implementation

        public void OnMonitoringStarted(string region)
        {
            UserDialogs.Instance.Alert(String.Format("Geofence iniciado"));
        }

        public void OnMonitoringStopped()
        {
            UserDialogs.Instance.Alert("Geofence finalizado");
        }

        public void OnMonitoringStopped(string identifier)
        {
            UserDialogs.Instance.Alert(String.Format("Geofence finalizado por {0} - via Event Trigger", identifier));
        }

        public void OnRegionStateChanged(GeofenceResult result)
        {
            switch (result.Transition)
            {
                case GeofenceTransition.Entered:
                    break;
    
                case GeofenceTransition.Exited:
                    break;
    
                case GeofenceTransition.Stayed:
                    break;
    
                default:
                    break;
            }
    
            /*if (result.Transition == GeofenceTransition.Entered)
    //				SendMail(new []{ "aunanue@solidmation.com", "jbravo.br@gmail.com" },
    //					"Email sent by the Region State Changed event, Entered the radius area",
    //					"Message Test from Geofence App");
    			{
    				var msg = String.Format("{0} - {1}{2}", "Region State Changed event trigger, Entered!", DateTime.Now.ToString(), Environment.NewLine);
    				WriteFile(msg);
    			}
    			else if (result.Transition == GeofenceTransition.Exited)
    //				SendMail(new []{ "aunanue@solidmation.com", "jbravo.br@gmail.com" },
    //					"Email sent by the Region State Changed event, Exited the radius area",
    //					"Message Test from Geofence App");
    			{
    				var msg = String.Format("{0} - {1}{2}", "Region State Changed event trigger, Exited!", DateTime.Now.ToString(), Environment.NewLine);
    				WriteFile(msg);
    			}
    			else if (result.Transition == GeofenceTransition.Stayed)
    //				SendMail(new []{ "aunanue@solidmation.com", "jbravo.br@gmail.com" },
    //					"Email sent by the Region State Changed event, Stayed at the radius area",
    //					"Message Test from Geofence App");
    			{
    				var msg = String.Format("{0} - {1}{2}", "Region State Changed event trigger, Stayed!", DateTime.Now.ToString(), Environment.NewLine);
    				WriteFile(msg);
    			}
    			else
    //				SendMail(new []{ "aunanue@solidmation.com", "jbravo.br@gmail.com" },
    //					"Email sent by the Region State Changed event, Unknow",
    //					"Message Test from Geofence App");
    			{
    				var msg = String.Format("{0} - {1}{2}", "Region State Changed event trigger, Unknow!", DateTime.Now.ToString(), Environment.NewLine);
    				WriteFile(msg);
    			}*/
        }

        public void OnError(string error)
        {
            UserDialogs.Instance.Alert(String.Format("Erro: {0} - via Event Trigger", error));
        }

        #endregion
    
    }
}

