﻿using System;
using System.IO;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TropoCSharp.Tropo;
using System.Web;

namespace TropoSamples
{
    /// <summary>
    /// An example of how to receive and process the Tropo Result object.
    /// </summary>
    public partial class TropoResult : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (StreamReader reader = new StreamReader(Request.InputStream))
            {               
                // Get the JSON submitted from Tropo. 
                string resultJSON = TropoUtilities.parseJSON(reader);
                Console.WriteLine("resultJSONis:" + resultJSON);
                HttpContext.Current.Trace.Warn(DateTime.Now.ToString() + " I Made It HerresultJSONis" + resultJSON);

                // Create a new instance of the Tropo class.
                Tropo tropo = new Tropo();

                try
                {
                    // Create a new Result object and pass in the JSON submitted from Tropo.
                    Result tropoResult = new Result(resultJSON);

                    // Get Actions container and parse.
                    JArray Actions = tropoResult.Actions;

                    // A simple example showing how to access properties of the Result object.
                    //tropo.Say("The State of the current session is " + tropoResult.State);
                    //tropo.Say("The Sequence of this Result payload is " + tropoResult.Sequence);
                    //tropo.Say("The session ID for the current session is is " + TropoUtilities.addSpaces(tropoResult.SessionId));

                    if (null != Actions)
                    {
                        tropo.Say("The test is " + TropoUtilities.removeQuotes(Actions.First["value"].ToString()));
                        tropo.Say("The frank test is " + TropoUtilities.removeQuotes(Actions.Last["value"].ToString()));

                    }
                    tropo.Say("user type is " + tropoResult.MachineDetection);
                    tropo.Say("uploadStatus is " + resultJSON);

                    //tropo.Say("The value selected by the caller is " + TropoUtilities.removeQuotes(Actions["value"].ToString()));
                }

                catch (JsonReaderException)
                {
                    tropo.Say("Sorry, an error occured. I choked on some JSON");
                }

                catch (Exception ex)
                {
                    tropo.Say("Sorry, an error occured. " + ex.Message);
                }

                finally
                {
                    Response.Write(tropo.RenderJSON());
                }
            }
        }
    }
}
