import type { UserInfo } from '@vben/types';

import { baseRequestClient, requestClient } from '#/api/request';

export namespace AuthApi {
  /** 登录接口参数 */
  export interface LoginParams {
    password?: string;
    username?: string;
  }

  /** 登录接口返回值 */
  export interface LoginResult {
    accessToken: string;
  }

  export interface RefreshTokenResult {
    data: string;
    status: number;
  }

  /** 组织架构树节点 */
  export interface OrganizationTreeNode {
    id: number;
    name: string;
    children?: OrganizationTreeNode[];
    remark: string;
  }
}

/**
 * 登录
 */
export async function loginApi(data: AuthApi.LoginParams) {
  return requestClient.post<AuthApi.LoginResult>('/auth/login', data);
}

/**
 * 刷新accessToken
 */
export async function refreshTokenApi() {
  return baseRequestClient.post<AuthApi.RefreshTokenResult>('/auth/refresh', {
    withCredentials: true,
  });
}

/**
 * 退出登录
 */
export async function logoutApi() {
  return baseRequestClient.post('/auth/logout', {
    withCredentials: true,
  });
}

/**
 * 获取用户权限码
 */
export async function getAccessCodesApi() {
  return requestClient.get<string[]>('/auth/functions');
}

/**
 * 获取用户信息
 * @returns
 */
export const getUserInfoApi = async () => {
  return requestClient.get<UserInfo>('/auth/vben-user-info');
};

/** 获取组织机构 */
export const getOrganizationTreeData = () => {
  return requestClient.get<AuthApi.OrganizationTreeNode[]>('/auth/organization-tree');
};
