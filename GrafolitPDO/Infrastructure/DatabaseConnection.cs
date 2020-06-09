using DatabaseWebService.Models;
using DatabaseWebService.Models.Client;
using DatabaseWebService.ModelsOTP;
using DatabaseWebService.ModelsOTP.Client;
using DatabaseWebService.ModelsOTP.Order;
using DatabaseWebService.ModelsOTP.Recall;
using DatabaseWebService.ModelsOTP.Route;
using DatabaseWebService.ModelsOTP.Tender;
using Newtonsoft.Json;
using GrafolitPDO.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using DatabaseWebService.ModelsPDO.Inquiry;
using DatabaseWebService.ModelsPDO.Order;
using DatabaseWebService.ModelsPDO;
using DatabaseWebService.ModelsPDO.Settings;
using DatabaseWebService.Models.Employee;

namespace GrafolitPDO.Infrastructure
{
    public class DatabaseConnection
    {
        public WebResponseContentModel<UserModel> SignIn(string username, string password)
        {
            WebResponseContentModel<UserModel> user = new WebResponseContentModel<UserModel>();
            try
            {
                user = GetResponseFromWebRequest<WebResponseContentModel<UserModel>>(WebServiceHelper.SignIn(username, password), "get");
            }
            catch (Exception ex)
            {
                user.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }
            return user;
        }

        public WebResponseContentModel<byte[]> GetWebServiceLogFile()
        {
            WebResponseContentModel<byte[]> user = new WebResponseContentModel<byte[]>();
            try
            {
                user = GetResponseFromWebRequest<WebResponseContentModel<byte[]>>(WebServiceHelper.GetWebServiceLogFile(), "get");
            }
            catch (Exception ex)
            {
                user.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }
            return user;
        }

        public WebResponseContentModel<byte[]> GetUtilityServiceLogFile()
        {
            WebResponseContentModel<byte[]> user = new WebResponseContentModel<byte[]>();
            try
            {
                user = GetResponseFromWebRequest<WebResponseContentModel<byte[]>>(WebServiceHelper.GetUtilityServiceLogFile(), "get");
            }
            catch (Exception ex)
            {
                user.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }
            return user;
        }


        #region Client

        public WebResponseContentModel<List<ClientSimpleModel>> GetAllClients()
        {
            WebResponseContentModel<List<ClientSimpleModel>> dt = new WebResponseContentModel<List<ClientSimpleModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientSimpleModel>>>(WebServiceHelper.GetClientsFromDb(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<ClientSimpleModel>> GetAllClients(string typeCode)
        {
            WebResponseContentModel<List<ClientSimpleModel>> dt = new WebResponseContentModel<List<ClientSimpleModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientSimpleModel>>>(WebServiceHelper.GetClientsFromDb(typeCode), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<ClientSimpleModel>> GetAllClients(int employeeID)
        {
            WebResponseContentModel<List<ClientSimpleModel>> dt = new WebResponseContentModel<List<ClientSimpleModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientSimpleModel>>>(WebServiceHelper.GetClientsFromDb(employeeID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<ClientSimpleModel>> GetAllClients(int employeeID, string typeCode)
        {
            WebResponseContentModel<List<ClientSimpleModel>> dt = new WebResponseContentModel<List<ClientSimpleModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientSimpleModel>>>(WebServiceHelper.GetClientsFromDb(employeeID, typeCode), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<ClientFullModel> GetClient(int clientID)
        {
            WebResponseContentModel<ClientFullModel> client = new WebResponseContentModel<ClientFullModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<ClientFullModel>>(WebServiceHelper.GetClientByID(clientID), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<ClientFullModel> GetClientByCode(string sKoda)
        {
            WebResponseContentModel<ClientFullModel> client = new WebResponseContentModel<ClientFullModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<ClientFullModel>>(WebServiceHelper.GetClientByCode(sKoda), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<ClientFullModel> GetClient(int clientID, int employeeID)
        {
            WebResponseContentModel<ClientFullModel> client = new WebResponseContentModel<ClientFullModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<ClientFullModel>>(WebServiceHelper.GetClientByID(clientID, employeeID), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<ClientFullModel> SaveClientChanges(ClientFullModel newData)
        {
            WebResponseContentModel<ClientFullModel> model = new WebResponseContentModel<ClientFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<ClientFullModel>>(WebServiceHelper.SaveClientDataChanges(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteClient(int clientID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteClient(clientID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<ContactPersonModel> SaveContactPersonChanges(ContactPersonModel newData)
        {
            WebResponseContentModel<ContactPersonModel> model = new WebResponseContentModel<ContactPersonModel>();
            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<ContactPersonModel>>(WebServiceHelper.SaveContactPersonChanges(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteContactPerson(int contactPersonID, int clientID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteContactPerson(contactPersonID, clientID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<ContactPersonModel>> GetContactPersonModelListByName(string SupplierName)
        {
            WebResponseContentModel<List<ContactPersonModel>> client = new WebResponseContentModel<List<ContactPersonModel>>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<List<ContactPersonModel>>>(WebServiceHelper.GetContactPersonModelListByName(SupplierName), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<List<ContactPersonModel>> GetContactPersonModelListByClientID(int ClientID)
        {
            WebResponseContentModel<List<ContactPersonModel>> client = new WebResponseContentModel<List<ContactPersonModel>>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<List<ContactPersonModel>>>(WebServiceHelper.GetContactPersonModelListByClientID(ClientID), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<ClientEmployeeModel> SaveClientEmployeeChanges(ClientEmployeeModel newData)
        {
            WebResponseContentModel<ClientEmployeeModel> model = new WebResponseContentModel<ClientEmployeeModel>();
            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<ClientEmployeeModel>>(WebServiceHelper.SaveClientEmployeeChanges(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteClientEmployee(int clientID, int employeeID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteClientEmployee(clientID, employeeID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> ClientEmployeeExist(int clientID, int employeeID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.ClientEmployeeExist(clientID, employeeID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<ClientType> GetClientTypeByCode(string typeCode)
        {
            WebResponseContentModel<ClientType> dt = new WebResponseContentModel<ClientType>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<ClientType>>(WebServiceHelper.GetClientTypeByCode(typeCode), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<ClientType> GetClientTypeById(int id)
        {
            WebResponseContentModel<ClientType> dt = new WebResponseContentModel<ClientType>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<ClientType>>(WebServiceHelper.GetClientTypeByID(id), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<ClientType>> GetClientTypes()
        {
            WebResponseContentModel<List<ClientType>> dt = new WebResponseContentModel<List<ClientType>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientType>>>(WebServiceHelper.GetClientTypes(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<LanguageModel>> GetLanguages()
        {
            WebResponseContentModel<List<LanguageModel>> dt = new WebResponseContentModel<List<LanguageModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<LanguageModel>>>(WebServiceHelper.GetLanguages(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<DepartmentModel>> GetDepartments()
        {
            WebResponseContentModel<List<DepartmentModel>> dt = new WebResponseContentModel<List<DepartmentModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<DepartmentModel>>>(WebServiceHelper.GetDepartments(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<ClientTransportType>> GetAllTransportTypes()
        {
            WebResponseContentModel<List<ClientTransportType>> dt = new WebResponseContentModel<List<ClientTransportType>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<ClientTransportType>>>(WebServiceHelper.GetClientTransportTypes(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<ClientTransportType> GetTransportTypeByID(int transportTypeID)
        {
            WebResponseContentModel<ClientTransportType> transportType = new WebResponseContentModel<ClientTransportType>();
            try
            {
                transportType = GetResponseFromWebRequest<WebResponseContentModel<ClientTransportType>>(WebServiceHelper.GetClientTransportTypeByID(transportTypeID), "get");
            }
            catch (Exception ex)
            {
                transportType.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return transportType;
        }

        public WebResponseContentModel<ClientTransportType> SaveTransportType(ClientTransportType newData)
        {
            WebResponseContentModel<ClientTransportType> model = new WebResponseContentModel<ClientTransportType>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<ClientTransportType>>(WebServiceHelper.SaveClientTransportType(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteTransportType(int transportTypeID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteClientTransportType(transportTypeID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<ClientFullModel> GetClientByName(string clientName)
        {
            WebResponseContentModel<ClientFullModel> client = new WebResponseContentModel<ClientFullModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<ClientFullModel>>(WebServiceHelper.GetClientByName(clientName), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<ClientFullModel> GetClientByNameOrInsert(string clientName)
        {
            WebResponseContentModel<ClientFullModel> client = new WebResponseContentModel<ClientFullModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<ClientFullModel>>(WebServiceHelper.GetClientByNameOrInsert(clientName), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<List<ClientSimpleModel>> GetSupplierByName(string name)
        {
            WebResponseContentModel<List<ClientSimpleModel>> client = new WebResponseContentModel<List<ClientSimpleModel>>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<List<ClientSimpleModel>>>(WebServiceHelper.GetSupplierByName(name), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }
        #endregion

        #region Employee

        public WebResponseContentModel<List<EmployeeFullModel>> GetAllEmployees()
        {
            WebResponseContentModel<List<EmployeeFullModel>> dt = new WebResponseContentModel<List<EmployeeFullModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<EmployeeFullModel>>>(WebServiceHelper.GetAllEmployees(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<EmployeeFullModel>> GetAllEmployeesByRoleID(int roleID)
        {
            WebResponseContentModel<List<EmployeeFullModel>> dt = new WebResponseContentModel<List<EmployeeFullModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<EmployeeFullModel>>>(WebServiceHelper.GetAllEmployeesByRoleID(roleID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<EmployeeFullModel> GetEmployeeByID(int employeeID)
        {
            WebResponseContentModel<EmployeeFullModel> dt = new WebResponseContentModel<EmployeeFullModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<EmployeeFullModel>>(WebServiceHelper.GetEmployeeByID(employeeID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<EmployeeFullModel> SaveEmployee(EmployeeFullModel newData)
        {
            WebResponseContentModel<EmployeeFullModel> model = new WebResponseContentModel<EmployeeFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<EmployeeFullModel>>(WebServiceHelper.SaveEmployee(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteEmployee(int employeeID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteEmployee(employeeID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<RoleModel>> GetRoles()
        {
            WebResponseContentModel<List<RoleModel>> dt = new WebResponseContentModel<List<RoleModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<RoleModel>>>(WebServiceHelper.GetRoles(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }
        #endregion

        #region Dashboard

        public WebResponseContentModel<DashboardPDOModel> GetDashboardPDOData()
        {
            WebResponseContentModel<DashboardPDOModel> dt = new WebResponseContentModel<DashboardPDOModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<DashboardPDOModel>>(WebServiceHelper.GetDashboardPDOData(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        #endregion

        #region Inquiry

        public WebResponseContentModel<List<InquiryModel>> GetAllInquiries()
        {
            WebResponseContentModel<List<InquiryModel>> dt = new WebResponseContentModel<List<InquiryModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<InquiryModel>>>(WebServiceHelper.GetAllInquiries(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<List<InquiryModel>> GetAllPurchases()
        {
            WebResponseContentModel<List<InquiryModel>> dt = new WebResponseContentModel<List<InquiryModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<InquiryModel>>>(WebServiceHelper.GetAllPurchases(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<InquiryFullModel> GetInquiryByID(int inquiryID, bool bOnlySelected, int iSelDobaviteljID)
        {
            WebResponseContentModel<InquiryFullModel> client = new WebResponseContentModel<InquiryFullModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<InquiryFullModel>>(WebServiceHelper.GetInquiryByID(inquiryID, bOnlySelected, iSelDobaviteljID), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<InquiryFullModel> SaveInquiry(InquiryFullModel newData)
        {
            WebResponseContentModel<InquiryFullModel> model = new WebResponseContentModel<InquiryFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<InquiryFullModel>>(WebServiceHelper.SaveInquiry(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<InquiryFullModel> SaveInquiryPurchase(InquiryFullModel newData)
        {
            WebResponseContentModel<InquiryFullModel> model = new WebResponseContentModel<InquiryFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<InquiryFullModel>>(WebServiceHelper.SaveInquiryPurchase(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteInquiry(int inquiryID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteInquiryByID(inquiryID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> CopyInquiryByID(int inquiryID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.CopyInquiryByID(inquiryID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<InquiryPositionModel> GetInquiryPositionByID(int inquiryPosID)
        {
            WebResponseContentModel<InquiryPositionModel> client = new WebResponseContentModel<InquiryPositionModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<InquiryPositionModel>>(WebServiceHelper.GetInquiryPositionByID(inquiryPosID), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<InquiryPositionModel> CopyInquiryPositionByID(int inquiryPosID)
        {
            WebResponseContentModel<InquiryPositionModel> client = new WebResponseContentModel<InquiryPositionModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<InquiryPositionModel>>(WebServiceHelper.CopyInquiryPositionByID(inquiryPosID), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<InquiryPositionModel> SaveInquiryPosition(InquiryPositionModel newData)
        {
            WebResponseContentModel<InquiryPositionModel> model = new WebResponseContentModel<InquiryPositionModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<InquiryPositionModel>>(WebServiceHelper.SaveInquiryPosition(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<InquiryPositionModel>> SaveInquiryPositions(List<InquiryPositionModel> newData)
        {
            WebResponseContentModel<List<InquiryPositionModel>> model = new WebResponseContentModel<List<InquiryPositionModel>>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<List<InquiryPositionModel>>>(WebServiceHelper.SaveInquiryPositions(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteInquiryPosition(int inquiryPosID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteInquiryPosition(inquiryPosID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<InquiryStatusModel> GetInquiryStatusByID(int inquiryStatID)
        {
            WebResponseContentModel<InquiryStatusModel> client = new WebResponseContentModel<InquiryStatusModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<InquiryStatusModel>>(WebServiceHelper.GetInquiryStatusByID(inquiryStatID), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<InquiryStatusModel> GetInquiryStatusByCode(string statusCode)
        {
            WebResponseContentModel<InquiryStatusModel> client = new WebResponseContentModel<InquiryStatusModel>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<InquiryStatusModel>>(WebServiceHelper.GetInquiryStatusByCode(statusCode), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<List<InquiryStatusModel>> GetInquiryStatuses()
        {
            WebResponseContentModel<List<InquiryStatusModel>> client = new WebResponseContentModel<List<InquiryStatusModel>>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<List<InquiryStatusModel>>>(WebServiceHelper.GetInquiryStatuses(), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<List<ClientSimpleModel>> GetBuyerList()
        {
            WebResponseContentModel<List<ClientSimpleModel>> client = new WebResponseContentModel<List<ClientSimpleModel>>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<List<ClientSimpleModel>>>(WebServiceHelper.GetBuyerList(), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<List<InquiryStatus>> GetAllStatuses()
        {
            WebResponseContentModel<List<InquiryStatus>> client = new WebResponseContentModel<List<InquiryStatus>>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<List<InquiryStatus>>>(WebServiceHelper.GetAllStatuses(), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<List<ProductCategory>> GetCategoryList()
        {
            WebResponseContentModel<List<ProductCategory>> client = new WebResponseContentModel<List<ProductCategory>>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<List<ProductCategory>>>(WebServiceHelper.GetCategoryList(), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<List<PantheonUsers>> GetPantheonUsers()
        {
            WebResponseContentModel<List<PantheonUsers>> client = new WebResponseContentModel<List<PantheonUsers>>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<List<PantheonUsers>>>(WebServiceHelper.GetPantheonUsers(), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }




        public WebResponseContentModel<List<InquiryPositionArtikelModel>> DeleteInquiryPositionArtikles(List<InquiryPositionArtikelModel> newData)
        {
            WebResponseContentModel<List<InquiryPositionArtikelModel>> model = new WebResponseContentModel<List<InquiryPositionArtikelModel>>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<List<InquiryPositionArtikelModel>>>(WebServiceHelper.DeleteInquiryPositionArtikles(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> LockInquiry(int inquiryID, int userID)
        {
            WebResponseContentModel<bool> lockInquiry = new WebResponseContentModel<bool>();
            try
            {
                lockInquiry = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.LockInquiry(inquiryID, userID), "get");
            }
            catch (Exception ex)
            {
                lockInquiry.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return lockInquiry;
        }

        public WebResponseContentModel<bool> UnLockInquiry(int inquiryID, int userID)
        {
            WebResponseContentModel<bool> unLockInquiry = new WebResponseContentModel<bool>();
            try
            {
                unLockInquiry = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.UnLockInquiry(inquiryID, userID), "get");
            }
            catch (Exception ex)
            {
                unLockInquiry.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return unLockInquiry;
        }

        public WebResponseContentModel<bool> IsInquiryLocked(int inquiryID)
        {
            WebResponseContentModel<bool> isLocked = new WebResponseContentModel<bool>();
            try
            {
                isLocked = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.IsInquiryLocked(inquiryID), "get");
            }
            catch (Exception ex)
            {
                isLocked.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return isLocked;
        }

        public WebResponseContentModel<bool> UnLockInquiriesByUserID(int userID)
        {
            WebResponseContentModel<bool> isLocked = new WebResponseContentModel<bool>();
            try
            {
                isLocked = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.UnLockInquiriesByUserID(userID), "get");
            }
            catch (Exception ex)
            {
                isLocked.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return isLocked;
        }

        public WebResponseContentModel<List<GroupedInquiryPositionsBySupplier>> GetInquiryPositionsGroupedBySupplier(int inquiryID)
        {
            WebResponseContentModel<List<GroupedInquiryPositionsBySupplier>> list = new WebResponseContentModel<List<GroupedInquiryPositionsBySupplier>>();
            try
            {
                list = GetResponseFromWebRequest<WebResponseContentModel<List<GroupedInquiryPositionsBySupplier>>>(WebServiceHelper.GetInquiryPositionsGroupedBySupplier(inquiryID), "get");
            }
            catch (Exception ex)
            {
                list.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return list;
        }

        public WebResponseContentModel<GroupedInquiryPositionsBySupplier> SaveInquiryPositionSupplierPdfReport(GroupedInquiryPositionsBySupplier newData)
        {
            WebResponseContentModel<GroupedInquiryPositionsBySupplier> model = new WebResponseContentModel<GroupedInquiryPositionsBySupplier>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<GroupedInquiryPositionsBySupplier>>(WebServiceHelper.SaveInquiryPositionSupplierPdfReport(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }
        #endregion

        #region OrderPDO

        public WebResponseContentModel<List<OrderPDOFullModel>> GetOrderList()
        {
            WebResponseContentModel<List<OrderPDOFullModel>> orderList = new WebResponseContentModel<List<OrderPDOFullModel>>();
            try
            {
                orderList = GetResponseFromWebRequest<WebResponseContentModel<List<OrderPDOFullModel>>>(WebServiceHelper.GetOrderList(), "get");
            }
            catch (Exception ex)
            {
                orderList.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return orderList;
        }

        public WebResponseContentModel<OrderPDOFullModel> GetOrderByID(int oID)
        {
            WebResponseContentModel<OrderPDOFullModel> order = new WebResponseContentModel<OrderPDOFullModel>();
            try
            {
                order = GetResponseFromWebRequest<WebResponseContentModel<OrderPDOFullModel>>(WebServiceHelper.GetOrderByID(oID), "get");
            }
            catch (Exception ex)
            {
                order.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return order;
        }



        public WebResponseContentModel<List<OrderPDOPositionModel>> GetOrderPositionsByOrderID(int oID)
        {
            WebResponseContentModel<List<OrderPDOPositionModel>> orderPosList = new WebResponseContentModel<List<OrderPDOPositionModel>>();
            try
            {
                orderPosList = GetResponseFromWebRequest<WebResponseContentModel<List<OrderPDOPositionModel>>>(WebServiceHelper.GetOrderPositionsByOrderID(oID), "get");
            }
            catch (Exception ex)
            {
                orderPosList.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return orderPosList;
        }

        public WebResponseContentModel<InquiryFullModel> GetOrderByInquiryIDForNewOrder(int iID)
        {
            WebResponseContentModel<InquiryFullModel> newOrder = new WebResponseContentModel<InquiryFullModel>();
            try
            {
                newOrder = GetResponseFromWebRequest<WebResponseContentModel<InquiryFullModel>>(WebServiceHelper.GetOrderByInquiryIDForNewOrder(iID), "get");
            }
            catch (Exception ex)
            {
                newOrder.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return newOrder;
        }

        public WebResponseContentModel<OrderPDOFullModel> SaveOrder(OrderPDOFullModel newData)
        {
            WebResponseContentModel<OrderPDOFullModel> model = new WebResponseContentModel<OrderPDOFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<OrderPDOFullModel>>(WebServiceHelper.SaveOrderModel(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<InquiryFullModel> CheckPantheonArtikles(InquiryFullModel newData)
        {
            WebResponseContentModel<InquiryFullModel> model = new WebResponseContentModel<InquiryFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<InquiryFullModel>>(WebServiceHelper.CheckPantheonArtikles(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<InquiryFullModel> SaveAndCreateOrder(InquiryFullModel newData)
        {
            WebResponseContentModel<InquiryFullModel> model = new WebResponseContentModel<InquiryFullModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<InquiryFullModel>>(WebServiceHelper.SaveAndCreateOrder(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteOrder(int orderID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteOrder(orderID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> ResetOrderStatusByID(int orderID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.ResetOrderStatusByID(orderID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }


        public WebResponseContentModel<List<OrderPDOPositionModel>> SaveOrderPositionsModel(List<OrderPDOPositionModel> newData)
        {
            WebResponseContentModel<List<OrderPDOPositionModel>> model = new WebResponseContentModel<List<OrderPDOPositionModel>>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<List<OrderPDOPositionModel>>>(WebServiceHelper.SaveOrderPositionsModel(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> DeleteOrderPosition(int orderPosID)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.DeleteOrderPosition(orderPosID), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<List<ProductModel>> GetProductByName(string name)
        {
            WebResponseContentModel<List<ProductModel>> client = new WebResponseContentModel<List<ProductModel>>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<List<ProductModel>>>(WebServiceHelper.GetProductByName(name), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        public WebResponseContentModel<List<ProductModel>> GetProductBySupplierAndName(string supplier, string name)
        {
            WebResponseContentModel<List<ProductModel>> client = new WebResponseContentModel<List<ProductModel>>();
            try
            {
                client = GetResponseFromWebRequest<WebResponseContentModel<List<ProductModel>>>(WebServiceHelper.GetProductBySupplierAndName(supplier, name), "get");
            }
            catch (Exception ex)
            {
                client.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return client;
        }

        #endregion

        #region Settings

        public WebResponseContentModel<List<PDOEmailModel>> GetAllEmails()
        {
            WebResponseContentModel<List<PDOEmailModel>> dt = new WebResponseContentModel<List<PDOEmailModel>>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<List<PDOEmailModel>>>(WebServiceHelper.GetAllEmails(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<SettingsModel> GetSettings()
        {
            WebResponseContentModel<SettingsModel> settings = new WebResponseContentModel<SettingsModel>();
            try
            {
                settings = GetResponseFromWebRequest<WebResponseContentModel<SettingsModel>>(WebServiceHelper.GetSettings(), "get");
            }
            catch (Exception ex)
            {
                settings.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return settings;
        }

        public WebResponseContentModel<SettingsModel> SaveSettings(SettingsModel newData)
        {
            WebResponseContentModel<SettingsModel> model = new WebResponseContentModel<SettingsModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<SettingsModel>>(WebServiceHelper.SaveSettings(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> RunSQLString(string sSQL)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.RunSQLString(sSQL), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> CreateMailCopy(int mailID)
        {
            WebResponseContentModel<bool> dt = new WebResponseContentModel<bool>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.CreateMailCopy(mailID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<PDOEmailModel> CreateMailCopy(PDOEmailModel newData)
        {
            WebResponseContentModel<PDOEmailModel> model = new WebResponseContentModel<PDOEmailModel>();

            try
            {
                model.Content = newData;
                model = PostWebRequestData<WebResponseContentModel<PDOEmailModel>>(WebServiceHelper.CreateMailCopy(), "post", model);
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<PDOEmailModel> GetMailByID(int mailID)
        {
            WebResponseContentModel<PDOEmailModel> dt = new WebResponseContentModel<PDOEmailModel>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<PDOEmailModel>>(WebServiceHelper.GetMailByID(mailID), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }
        #endregion

        #region "Admin"
        public WebResponseContentModel<bool> CreatePDFAndSendPDOOrdersMultiple()
        {
            WebResponseContentModel<bool> dt = new WebResponseContentModel<bool>();
            try
            {
                dt = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.CreatePDFAndSendPDOOrdersMultiple(), "get");
            }
            catch (Exception ex)
            {
                dt.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return dt;
        }

        public WebResponseContentModel<bool> RunPantheon(string sFile, string sArgs)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.RunPantheon(sFile, sArgs), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<bool> ChangeConfigValue(string sConfigName, string sConfigValue)
        {
            WebResponseContentModel<bool> model = new WebResponseContentModel<bool>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<bool>>(WebServiceHelper.ChangeConfigValue(sConfigName, sConfigValue), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        public WebResponseContentModel<string> GetConfigValue(string sConfigName)
        {
            WebResponseContentModel<string> model = new WebResponseContentModel<string>();

            try
            {
                model = GetResponseFromWebRequest<WebResponseContentModel<string>>(WebServiceHelper.GetConfigValue(sConfigName), "get");
            }
            catch (Exception ex)
            {
                model.ValidationErrorAppSide = ConcatenateExceptionMessage(ex);
            }

            return model;
        }

        #endregion

        public T GetResponseFromWebRequest<T>(string uri, string requestMethod)
        {
            object obj = default(T);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = requestMethod.ToUpper();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string streamString = reader.ReadToEnd();

            obj = JsonConvert.DeserializeObject<T>(streamString);

            return (T)obj;
        }

        public T PostWebRequestData<T>(string uri, string requestMethod, T objectToSerialize)
        {
            object obj = default(T);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = requestMethod.ToUpper();
            request.ContentType = "application/json; charset=utf-8";

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                string clientData = JsonConvert.SerializeObject(objectToSerialize);
                sw.Write(clientData);
                sw.Flush();
                sw.Close();
            }


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string streamString = reader.ReadToEnd();

            obj = JsonConvert.DeserializeObject<T>(streamString);

            return (T)obj;
        }

        private string ConcatenateExceptionMessage(Exception ex)
        {
            return ex.Message + " \r\n" + ex.Source + (ex.InnerException != null ? ex.InnerException.Message + " \r\n" + ex.Source : "");
        }

        public async Task<T> PostWebRequestDataAsync<T>(string uri, string requestMethod, T objectToSerialize)
        {
            object obj = default(T);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = requestMethod.ToUpper();
            request.ContentType = "application/json; charset=utf-8";

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                string clientData = JsonConvert.SerializeObject(objectToSerialize);
                sw.Write(clientData);
                sw.Flush();
                sw.Close();
            }

            var response = (HttpWebResponse)await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);

            // HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string streamString = reader.ReadToEnd();

            obj = JsonConvert.DeserializeObject<T>(streamString);

            return (T)obj;

        }

        public T PostWebRequestData2<T>(string uri, string requestMethod, T objectToSerialize)
        {
            object obj = default(T);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = requestMethod.ToUpper();
            request.ContentType = "application/json; charset=utf-8";

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                string clientData = JsonConvert.SerializeObject(objectToSerialize);
                sw.Write(clientData);
                sw.Flush();
                sw.Close();
            }

            var response = (HttpWebResponse)WebRequestExtensions.GetResponseWithTimeout(request, 36000);

            // HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string streamString = reader.ReadToEnd();

            obj = JsonConvert.DeserializeObject<T>(streamString);

            return (T)obj;

        }
    }

    public static class WebRequestExtensions
    {
        public static Stream GetRequestStreamWithTimeout(
            this WebRequest request,
            int? millisecondsTimeout = null)
        {
            return AsyncToSyncWithTimeout(
                request.BeginGetRequestStream,
                request.EndGetRequestStream,
                millisecondsTimeout ?? request.Timeout);
        }

        public static WebResponse GetResponseWithTimeout(
            this HttpWebRequest request,
            int? millisecondsTimeout = null)
        {
            return AsyncToSyncWithTimeout(
                request.BeginGetResponse,
                request.EndGetResponse,
                millisecondsTimeout ?? request.Timeout);
        }

        private static T AsyncToSyncWithTimeout<T>(
            Func<AsyncCallback, object, IAsyncResult> begin,
            Func<IAsyncResult, T> end,
            int millisecondsTimeout)
        {
            var iar = begin(null, null);
            if (!iar.AsyncWaitHandle.WaitOne(millisecondsTimeout))
            {
                var ex = new TimeoutException();
                throw new WebException(ex.Message, ex, WebExceptionStatus.Timeout, null);
            }
            return end(iar);
        }
    }
}