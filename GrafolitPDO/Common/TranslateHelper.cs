using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DatabaseWebService.Common.Enums.Enums;
using static GrafolitPDO.Common.Enums;

namespace GrafolitPDO.Common
{
    public static class TranslateHelper
    {
        public static string GetTranslateValueByContentAndLanguage(Language langT, ReportContentType _ReportCType)
        {
            string RetStr = "";

            switch (_ReportCType)
            {
                case ReportContentType.GREETINGS:
                    switch (langT)
                    {
                        case Language.ANG:
                            RetStr = "Hello, <br><br><br>We kindly ask for the delivery date for the material:";
                            break;
                        case Language.HRV:
                            RetStr = "Poštovani, <br><br><br>Molimo za rok isporuke za material:";
                            break;
                        case Language.SLO:
                            RetStr = "Pozdravljeni, <br><br><br>  Vljudno prosimo za rok dobave za material:";
                            break;
                        default:
                            break;
                    }
                    break;
                case ReportContentType.REGARDS:
                    switch (langT)
                    {
                        case Language.ANG:
                            RetStr = "Thank you and best regards,";
                            break;
                        case Language.HRV:
                            RetStr = "Hvala i srdačan pozdrav,";
                            break;
                        case Language.SLO:
                            RetStr = "Hvala in lep pozdrav,";
                            break;
                        default:
                            break;
                    }
                    break;
                case ReportContentType.INQUIRY:
                    switch (langT)
                    {
                        case Language.ANG:
                            RetStr = "ENQUIRY";
                            break;
                        case Language.HRV:
                            RetStr = "UPIT";
                            break;
                        case Language.SLO:
                            RetStr = "POVPRAŠEVANJE";
                            break;
                        default:
                            break;
                    }
                    break;
                case ReportContentType.MATERIAL:
                    switch (langT)
                    {
                        case Language.ANG:
                            RetStr = "MATERIAL";
                            break;
                        case Language.HRV:
                            RetStr = "ARTIKEL";
                            break;
                        case Language.SLO:
                            RetStr = "ARTIKEL";
                            break;
                        default:
                            break;
                    }
                    break;
                case ReportContentType.QUANTITY:
                    switch (langT)
                    {
                        case Language.ANG:
                            RetStr = "QUANTITY";
                            break;
                        case Language.HRV:
                            RetStr = "KOLIČINA";
                            break;
                        case Language.SLO:
                            RetStr = "KOLIČINA";
                            break;
                        default:
                            break;
                    }
                    break;
                case ReportContentType.NOTES:
                    switch (langT)
                    {
                        case Language.ANG:
                            RetStr = "NOTES";
                            break;
                        case Language.HRV:
                            RetStr = "OPOMBA";
                            break;
                        case Language.SLO:
                            RetStr = "OPOMBA";
                            break;
                        default:
                            break;
                    }
                    break;
            }


            return RetStr;
        }
    }
}