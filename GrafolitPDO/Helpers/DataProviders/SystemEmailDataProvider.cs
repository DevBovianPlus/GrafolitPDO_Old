using DatabaseWebService.Models.Client;
using DatabaseWebService.ModelsPDO.Settings;
using GrafolitPDO.Common;
using GrafolitPDO.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrafolitPDO.Helpers.DataProviders
{
    public class SystemEmailDataProvider : ServerMasterPage
    {
        public void SetSystemEmailMessageModel(PDOEmailModel model)
        {
            AddValueToSession(Enums.SystemEmailMessageSession.SystemMessageModel, model);
        }

        public PDOEmailModel GetSystemEmailMessageModel()
        {
            if (SessionHasValue(Enums.SystemEmailMessageSession.SystemMessageModel))
                return (PDOEmailModel)GetValueFromSession(Enums.SystemEmailMessageSession.SystemMessageModel);

            return null;
        }
    }
}