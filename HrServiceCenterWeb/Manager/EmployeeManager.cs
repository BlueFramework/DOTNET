using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueFramework.Blood;
using HrServiceCenterWeb.Models;

namespace HrServiceCenterWeb.Manager
{
    public class EmployeeManager
    {
        public List<PositionInfo> GetPositions()
        {
            EntityContext context = Session.CreateContext();
            List<PositionInfo> list = context.SelectList<PositionInfo>("hr.position.findPositions", null);
            return list;
        }

        public List<CompanyInfo> GetCompanies(string query)
        {
            EntityContext context = Session.CreateContext();
            List<CompanyInfo> list = context.SelectList<CompanyInfo>("hr.company.findCompanys", query);
            return list;
        }

        public CompanyInfo GetCompany(int companyId)
        {
            EntityContext context = Session.CreateContext();
            CompanyInfo companyInfo = context.Selete<CompanyInfo>("hr.company.findCompanyById", companyId);
            companyInfo.Positions = context.SelectList<CompanyPositionSetInfo>("hr.company.findPositions", companyId);
            return companyInfo;
        }

        public CompanyInfo SaveCompany(CompanyInfo companyInfo)
        {

            using (EntityContext context = Session.CreateContext())
            {
                try
                {
                    context.BeginTransaction();

                    if (companyInfo.CompanyId > 0)
                        context.Save<CompanyInfo>("hr.company.updateCompany", companyInfo);
                    else
                    {
                        context.Save<CompanyInfo>("hr.company.insertCompany", companyInfo);
                        CompanyAccountInfo account = new CompanyAccountInfo()
                        {
                            CompanyId = companyInfo.CompanyId
                        };
                        context.Save<CompanyAccountInfo>("hr.company.insertAccount", account);
                    }

                    context.Commit();
                }
                catch (Exception ex)
                {
                    ex = null;
                    context.Rollback();
                    companyInfo = null;
                }
            }
            return companyInfo;
        }

        public bool SaveRecharge(CompanyAccountRecordInfo accountRecordInfo)
        {
            bool pass = true;
            
            accountRecordInfo.CreateTime = DateTime.Now;

            using (EntityContext context = Session.CreateContext())
            {
                try
                {
                    context.BeginTransaction();
                    CompanyAccountInfo accountInfo = new CompanyAccountInfo()
                    {
                        CompanyId = accountRecordInfo.CompanyId,
                        AccountId = accountRecordInfo.AccountId,
                        AccountBalance = accountRecordInfo.AccountBalance+accountRecordInfo.Money
                    };
                    context.Save<CompanyAccountInfo>("hr.company.updateCompanyAccount", accountInfo);
                    context.Save<CompanyAccountRecordInfo>("hr.company.insertCompanyAccountDetail", accountRecordInfo);
                    context.Commit();
                }
                catch
                {
                    context.Rollback();
                    pass = false;
                }
            }
            return pass;
        }

        public bool DeleteCompany(int companyId)
        {
            bool pass = true;
            using (EntityContext context = Session.CreateContext())
            {
                try
                {
                    context.BeginTransaction();
                    context.Delete("hr.company.deleteCompanyAccount", companyId);
                    context.Delete("hr.company.deleteCompany", companyId);
                    context.Commit();
                }
                catch (Exception ex)
                {
                    ex = null;
                    context.Rollback();
                    pass = false;
                }
            }
            return pass;
        }

        public List<CompanyPositionSetInfo> GetPositonSets(int companyId)
        {
            List<CompanyPositionSetInfo> list = null;
            using (EntityContext context = new EntityContext())
            {
                list = context.SelectList<CompanyPositionSetInfo>("hr.company.findPositions", companyId);
            }
            return list;
        }

        public bool SavePosition(CompanyPositionSetInfo positionSetInfo)
        {
            bool pass = true;
            using (EntityContext context = new EntityContext())
            {
                try
                {
                    context.BeginTransaction();
                    context.Delete<CompanyPositionSetInfo>("hr.company.deletePositions", positionSetInfo);
                    context.Save< CompanyPositionSetInfo>("hr.company.insertPosition", positionSetInfo);
                    context.Commit();
                }
                catch
                {
                    pass = false;
                    context.Rollback();
                }
            }
            return pass;
        }

        public bool DeletePostion(CompanyPositionSetInfo positionSetInfo)
        {
            using (EntityContext context = new EntityContext())
            {
                context.Delete<CompanyPositionSetInfo>("hr.company.deletePositions", positionSetInfo);
            }
            return true;
        }

        public EmployeeInfo GetEmployee(int personId)
        {
            EmployeeInfo employeeInfo = null;
            using (EntityContext context = new EntityContext())
            {
                employeeInfo = context.Selete<EmployeeInfo>("hr.employee.findEmployee", personId);
            }
            return employeeInfo;
        }

        public List<EmployeeInfo> GetEmployees(int companyId)
        {
            EmployeeInfo employee = new EmployeeInfo();
            employee.CompanyId = companyId;
            return GetEmployees(employee);
        }

        public List<EmployeeInfo> GetEmployees(EmployeeInfo employee)
        {
            List<EmployeeInfo> list = null;
            using (EntityContext context = new EntityContext())
            {
                list = context.SelectListByTemplate<EmployeeInfo>("hr.employee.findEmployees", employee);
            }
            return list;
        }

        public bool SaveEmployee(EmployeeInfo employeeInfo)
        {
            bool pass;
            using (EntityContext context = Session.CreateContext())
            {
                if (employeeInfo.PersonId == 0)
                {
                    employeeInfo.CreateTime = DateTime.Now.ToString();
                    employeeInfo.Creator = BlueFramework.User.UserContext.CurrentUser.UserId;
                    pass = context.Save<EmployeeInfo>("hr.employee.insertEmployee", employeeInfo);

                }
                else
                    pass = context.Save<EmployeeInfo>("hr.employee.updateEmployee", employeeInfo);
            }

            return pass;
        }

        public bool DeleteEmployee(int personId)
        {
            bool pass;
            using (EntityContext context = Session.CreateContext())
            {
                pass = context.Delete("hr.employee.deleteEmployee", personId);
            }

            return pass;
        }
    }
}