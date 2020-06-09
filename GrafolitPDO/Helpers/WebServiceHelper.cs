using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace GrafolitPDO.Helpers
{
    public static class WebServiceHelper
    {
        private static string BaseWebServiceURI
        {
            get
            {
                return WebConfigurationManager.AppSettings["BaseWebService"].ToString();
            }
        }

        private static string WebServiceSignInURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["ValuesController"].ToString();
            }
        }

        private static string WebServiceClientURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["ClientController"].ToString();
            }
        }

        private static string WebServiceEmployeeURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["EmployeeController"].ToString();
            }
        }

        private static string WebServiceDashboardURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["DashboardController"].ToString();
            }
        }

        private static string WebServiceInquiryURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["InquiryController"].ToString();
            }
        }

        private static string WebServiceOrderPDOURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["OrderPDOController"].ToString();
            }
        }

        private static string WebServiceSettingsURL
        {
            get
            {
                return BaseWebServiceURI + WebConfigurationManager.AppSettings["SettingsController"].ToString();
            }
        }

        public static string SignIn(string username, string pass)
        {
            return WebServiceSignInURL + "SignInPDO?username=" + username + "&password=" + pass;
        }

        public static string GetWebServiceLogFile()
        {
            return WebServiceSignInURL + "GetWebServiceLogFile";
        }

        public static string GetUtilityServiceLogFile()
        {
            return WebServiceSignInURL + "GetUtilityServiceLogFile";
        }


        #region Client

        public static string GetClientsFromDb()
        {
            return WebServiceClientURL + "GetAllClients";
        }
        public static string GetClientsFromDb(int employeeID)
        {
            return WebServiceClientURL + "GetAllClients?employeeID=" + employeeID.ToString();
        }

        public static string GetClientsFromDb(string typeCode)
        {
            return WebServiceClientURL + "GetAllClients?employeeID=0&typeCode=" + typeCode;
        }

        public static string GetClientsFromDb(int employeeID, string typeCode)
        {
            return WebServiceClientURL + "GetAllClients?employeeID=" + employeeID.ToString() + "&typeCode=" + typeCode;
        }

        public static string GetClientByID(int id)
        {
            return WebServiceClientURL + "GetClientByID?clientID=" + id.ToString();
        }

        public static string GetClientByCode(string sKoda)
        {
            return WebServiceClientURL + "GetClientByCode?sKoda=" + sKoda;
        }

        public static string GetClientByID(int id, int employeeID)
        {
            return WebServiceClientURL + "GetClientByID?clientID=" + id.ToString() + "&employeeID=" + employeeID.ToString();
        }

        public static string SaveClientDataChanges()
        {
            return WebServiceClientURL + "SaveClientData";
        }

        public static string DeleteClient(int id)
        {
            return WebServiceClientURL + "DeleteClient?clientID=" + id;
        }


        public static string SaveContactPersonChanges()
        {
            return WebServiceClientURL + "SaveContactPersonToClient";
        }

        public static string DeleteContactPerson(int contactPersonID, int clientID)
        {
            return WebServiceClientURL + "DeleteContactPerson?contactPersonID=" + contactPersonID + "&clientID=" + clientID;
        }

        public static string GetContactPersonModelListByName(string SupplierName)
        {
            return WebServiceClientURL + "GetContactPersonModelListByName?SupplierName=" + SupplierName;
        }

        public static string GetContactPersonModelListByClientID(int ClientID)
        {
            return WebServiceClientURL + "GetContactPersonModelListByClientID?ClientID=" + ClientID;
        }

        public static string SaveClientEmployeeChanges()
        {
            return WebServiceClientURL + "SaveClientEmployee";
        }

        public static string DeleteClientEmployee(int clientID, int employeeID)
        {
            return WebServiceClientURL + "DeleteClientEmployee?clientID=" + clientID + "&employeeID=" + employeeID;
        }

        public static string ClientEmployeeExist(int clientID, int employeeID)
        {
            return WebServiceClientURL + "ClientEmployeeExist?clientID=" + clientID + "&employeeID=" + employeeID;
        }

        public static string GetClientTypeByID(int id)
        {
            return WebServiceClientURL + "GetClientTypeByCode?id=" + id;
        }

        public static string GetClientTypeByCode(string typeCode)
        {
            return WebServiceClientURL + "GetAllClients?typeCode=" + typeCode;
        }

        public static string GetClientTypes()
        {
            return WebServiceClientURL + "GetClientTypes";
        }

        public static string GetLanguages()
        {
            return WebServiceClientURL + "GetLanguages";
        }

        public static string GetDepartments()
        {
            return WebServiceClientURL + "GetDepartments";
        }


        public static string GetClientTransportTypes()
        {
            return WebServiceClientURL + "GetAllTransportTypes";
        }
        public static string GetClientTransportTypeByID(int id)
        {
            return WebServiceClientURL + "GetTransportTypeByID?transportTypeID=" + id;
        }
        public static string SaveClientTransportType()
        {
            return WebServiceClientURL + "SaveTransportTypeData";
        }
        public static string DeleteClientTransportType(int transportTypeID)
        {
            return WebServiceClientURL + "DeleteTransportType?transportTypeID=" + transportTypeID;
        }

        public static string GetClientByName(string clientName)
        {
            return WebServiceClientURL + "GetClientByName?clientName=" + clientName;
        }

        public static string GetClientByNameOrInsert(string clientName)
        {
            return WebServiceClientURL + "GetClientByNameOrInsert?clientName=" + clientName;
        }
        #endregion

        #region Employee

        public static string GetAllEmployees()
        {
            return WebServiceEmployeeURL + "GetAllEmployees";
        }

        public static string GetAllEmployeesByRoleID(int roleID)
        {
            return WebServiceEmployeeURL + "GetAllEmployeesByRoleID?roleID=" + roleID;
        }

        public static string GetEmployeeByID(int employeeId)
        {
            return WebServiceEmployeeURL + "GetEmployeeByID?employeeId=" + employeeId;
        }

        public static string SaveEmployee()
        {
            return WebServiceEmployeeURL + "SaveEmployee";
        }

        public static string DeleteEmployee(int employeeID)
        {
            return WebServiceEmployeeURL + "DeleteEmployee?employeeID=" + employeeID;
        }

        public static string GetRoles()
        {
            return WebServiceEmployeeURL + "GetRoles";
        }
        #endregion

        #region Dashboard

        public static string GetDashboardPDOData()
        {
            return WebServiceDashboardURL + "GetDashboardPDOData";
        }

        #endregion

        #region Inquiry

        public static string GetAllInquiries()
        {
            return WebServiceInquiryURL + "GetAllInquiries";
        }

        public static string GetAllPurchases()
        {
            return WebServiceOrderPDOURL + "GetAllPurchases";
        }

        public static string GetInquiryByID(int inquiryID, bool bOnlySelected, int iSelDobaviteljID)
        {
            return WebServiceInquiryURL + "GetInquiryByID?inquiryID=" + inquiryID + "&bOnlySelected=" + bOnlySelected + "&iSelDobaviteljID=" + iSelDobaviteljID;
        }

        public static string SaveInquiry()
        {
            return WebServiceInquiryURL + "SaveInquiry";
        }

        public static string SaveInquiryPurchase()
        {
            return WebServiceInquiryURL + "SaveInquiryPurchase";
        }

        public static string DeleteInquiryByID(int inquiryID)
        {
            return WebServiceInquiryURL + "DeleteInquiry?inquiryID=" + inquiryID;
        }

        public static string CopyInquiryByID(int inquiryID)
        {
            return WebServiceInquiryURL + "CopyInquiryByID?inquiryID=" + inquiryID;
        }


        public static string GetInquiryPositionByID(int inquiryPosID)
        {
            return WebServiceInquiryURL + "GetInquiryPositionByID?inquiryPosID=" + inquiryPosID;
        }

        public static string CopyInquiryPositionByID(int inquiryPosID)
        {
            return WebServiceInquiryURL + "CopyInquiryPositionByID?inquiryPosID=" + inquiryPosID;
        }

        public static string SaveInquiryPosition()
        {
            return WebServiceInquiryURL + "SaveInquiryPosition";
        }

        public static string SaveInquiryPositions()
        {
            return WebServiceInquiryURL + "SaveInquiryPositions";
        }

        public static string DeleteInquiryPosition(int inquiryPosID)
        {
            return WebServiceInquiryURL + "DeleteInquiryPosition?inquiryPosID=" + inquiryPosID;
        }


        public static string GetInquiryStatusByID(int statusID)
        {
            return WebServiceInquiryURL + "GetInquiryStatusByID?statusID=" + statusID;
        }

        public static string GetInquiryStatusByCode(string statusCode)
        {
            return WebServiceInquiryURL + "GetInquiryStatusByCode?statusCode=" + statusCode;
        }

        public static string GetInquiryStatuses()
        {
            return WebServiceInquiryURL + "GetRecallStatuses";
        }

        public static string GetSupplierByName(string name)
        {
            return WebServiceInquiryURL + "GetSupplierByName?name=" + name;
        }

        public static string GetCategoryList()
        {
            return WebServiceInquiryURL + "GetCategoryList";
        }

        public static string GetPantheonUsers()
        {
            return WebServiceInquiryURL + "GetPantheonUsers";
        }

        public static string GetBuyerList()
        {
            return WebServiceInquiryURL + "GetBuyerList";
        }

        public static string GetAllStatuses()
        {
            return WebServiceInquiryURL + "GetAllStatuses";
        }

        public static string DeleteInquiryPositionArtikles()
        {
            return WebServiceInquiryURL + "DeleteInquiryPositionArtikles";
        }

        public static string LockInquiry(int inquiryID, int userId)
        {
            return WebServiceInquiryURL + "LockInquiry?inquiryID=" + inquiryID + "&userID=" + userId;
        }

        public static string UnLockInquiry(int inquiryID, int userId)
        {
            return WebServiceInquiryURL + "UnLockInquiry?inquiryID=" + inquiryID + "&userID=" + userId;
        }

        public static string IsInquiryLocked(int inquiryID)
        {
            return WebServiceInquiryURL + "IsInquiryLocked?inquiryID=" + inquiryID;
        }

        public static string UnLockInquiriesByUserID(int userID)
        {
            return WebServiceInquiryURL + "UnLockInquiriesByUserID?userID=" + userID;
        }

        public static string GetInquiryPositionsGroupedBySupplier(int inquiryID)
        {
            return WebServiceInquiryURL + "GetInquiryPositionsGroupedBySupplier?inquiryID=" + inquiryID;
        }

        public static string SaveInquiryPositionSupplierPdfReport()
        {
            return WebServiceInquiryURL + "SaveInquiryPositionSupplierPdfReport";
        }

        #endregion

        #region OrderPDO

        public static string GetOrderList()
        {
            return WebServiceOrderPDOURL + "GetOrderList";
        }

        public static string GetOrderByID(int oID)
        {
            return WebServiceOrderPDOURL + "GetOrderByID?oID=" + oID;
        }

        public static string GetOrderPositionsByOrderID(int oID)
        {
            return WebServiceOrderPDOURL + "GetOrderPositionsByOrderID?oID=" + oID;
        }

        public static string GetOrderByInquiryIDForNewOrder(int iID)
        {
            return WebServiceOrderPDOURL + "GetOrderByInquiryIDForNewOrder?iID=" + iID;
        }

        public static string SaveOrderModel()
        {
            return WebServiceOrderPDOURL + "SaveOrderModel";
        }

        public static string CheckPantheonArtikles()
        {
            return WebServiceOrderPDOURL + "CheckPantheonArtikles";
        }

        public static string SaveAndCreateOrder()
        {
            return WebServiceOrderPDOURL + "SaveAndCreateOrder";
        }

        public static string DeleteOrder(int orderID)
        {
            return WebServiceOrderPDOURL + "DeleteOrder?orderID=" + orderID;
        }

        public static string ResetOrderStatusByID(int orderID)
        {
            return WebServiceOrderPDOURL + "ResetOrderStatusByID?orderID=" + orderID;
        }

        public static string SaveOrderPositionsModel()
        {
            return WebServiceOrderPDOURL + "SaveOrderPositionsModel";
        }

        public static string DeleteOrderPosition(int orderPosID)
        {
            return WebServiceOrderPDOURL + "DeleteOrderPosition?orderPosID=" + orderPosID;
        }

        public static string GetProductByName(string name)
        {
            return WebServiceOrderPDOURL + "GetProductByName?name=" + name;
        }

        public static string GetProductBySupplierAndName(string supplier, string name)
        {
            return WebServiceOrderPDOURL + "GetProductBySupplierAndName?supplier=" + supplier + "&name=" + name;
        }


        #endregion

        #region Settings

        public static string GetSettings()
        {
            return WebServiceSettingsURL + "GetAppSettings";
        }

        public static string GetAllEmails()
        {
            return WebServiceSettingsURL + "GetAllEmails";
        }

        public static string SaveSettings()
        {
            return WebServiceSettingsURL + "SaveSettings";
        }

        public static string RunSQLString(string sSQL)
        {
            return WebServiceSettingsURL + "RunSQLString?sSQL=" + sSQL;
        }

        public static string CreateMailCopy(int mailID)
        {
            return WebServiceSettingsURL + "CreateMailCopy?mailID="+ mailID;
        }

        public static string CreateMailCopy()
        {
            return WebServiceSettingsURL + "CreateMailCopy";
        }

        public static string GetMailByID(int mailID)
        {
            return WebServiceSettingsURL + "GetMailByID?mailID=" + mailID;
        }
        #endregion

        #region "Admin"
        public static string CreatePDFAndSendPDOOrdersMultiple()
        {
            return WebServiceOrderPDOURL + "CreatePDFAndSendPDOOrdersMultiple";
        }

        public static string RunPantheon(string sFile, string sArgs)
        {
            return WebServiceOrderPDOURL + "RunPantheon?sFile=" + sFile + "&sArgs=" + sArgs;
        }

        public static string ChangeConfigValue(string sConfigName, string sConfigValue)
        {
            return WebServiceOrderPDOURL + "ChangeConfigValue?sConfigName=" + sConfigName + "&sConfigValue=" + sConfigValue;
        }

        public static string GetConfigValue(string sConfigName)
        {
            return WebServiceOrderPDOURL + "GetConfigValue?sConfigName=" + sConfigName;
        }
        #endregion
    }
}   