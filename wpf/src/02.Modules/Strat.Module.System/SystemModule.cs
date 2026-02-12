using Strat.Shared;
using Strat.Module.System.Views;
using Strat.Module.System.ViewModels;

namespace Strat.Module.System;

public class SystemModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<UserManagementView, UserManagementViewModel>();
        containerRegistry.RegisterForNavigation<RoleManagementView, RoleManagementViewModel>();
        containerRegistry.RegisterForNavigation<OrganizationManagementView, OrganizationManagementViewModel>();
        containerRegistry.RegisterForNavigation<FunctionManagementView, FunctionManagementViewModel>();
        // containerRegistry.RegisterForNavigation<InterfaceManagementView, InterfaceManagementViewModel>();
        // containerRegistry.RegisterForNavigation<SystemConfigView, SystemConfigViewModel>();
        // containerRegistry.RegisterForNavigation<DictManagementView, DictManagementViewModel>();
        
        containerRegistry.RegisterForNavigation<UserEditDialog, UserEditDialogViewModel>("UserEditDialog");
        containerRegistry.RegisterForNavigation<RoleEditDialog, RoleEditDialogViewModel>("RoleEditDialog");
        containerRegistry.RegisterForNavigation<RolePermissionDialog, RolePermissionDialogViewModel>("RolePermissionDialog");
        containerRegistry.RegisterForNavigation<OrganizationEditDialog, OrganizationEditDialogViewModel>("OrganizationEditDialog");
        containerRegistry.RegisterForNavigation<FunctionEditDialog, FunctionEditDialogViewModel>("FunctionEditDialog");
        
        containerRegistry.AutoRegisterViewForNavigation();
    }
}
