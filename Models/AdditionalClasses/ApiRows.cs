using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Soneta.Business;

namespace WebApplicationYes.Models.AdditionalClasses
{ 
    public abstract class ApiRows
    {
        public enum RowAction
        {
            GetRow = 1,
            CreateRow = 2,
            DeleteRow = 3
        }

        internal LoginEnova loginEnova = null;
        public object GetRow(string[] getParams) => InvokeActionWithLogin(RowAction.GetRow, getParams);
        public object CreateRow(object row) => InvokeActionWithLogin(RowAction.CreateRow, row);
        public object DeleteRow(object row) => InvokeActionWithLogin(RowAction.DeleteRow, row);

        private object InvokeActionWithLogin(RowAction action, object data)
        {
            if (loginEnova != null)
                loginEnova.LogoutFromEnova();
            loginEnova = new LoginEnova();
            var login = loginEnova.Login;

            try
            {
                switch (action)
                {
                    case RowAction.GetRow:
                        return GetFromEnovaAction((string[])data);
                    case RowAction.CreateRow:
                        return CreateInEnovaAction(data);
                    case RowAction.DeleteRow:
                        return DeleteFromEnovaAction(data);
                }
                
            }
            catch (Exception ex) { loginEnova.LogoutFromEnova(); throw ex; }
            return null;
        }

        protected abstract object GetFromEnovaAction(string[] getParam);
        protected abstract object CreateInEnovaAction(object row);
        protected abstract object DeleteFromEnovaAction(object row);
    }
}