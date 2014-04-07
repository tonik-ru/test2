using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUpdaterService" in both code and config file together.
[ServiceContract]
public interface IUpdaterService
{

	[OperationContract]
	List<UpdateFileInfo> GetAppFiles(string applicationName, int clientID, ref int release);

	[OperationContract]
	Stream GetFile(string applicationName, string fileName, int clientID);
}
