import request from '@/utils/request'
import { transformAbpListQuery } from '@/utils/abp'



export function getOrganizationsAll() {
  return request({
    url: '/api/identity/organizations/list',
    method: 'get'
  })
}

export function queryOrganizations(query) {
  return request({
    url: '/api/identity/organizations/list',
    method: 'get',
    params: query
  })
}

export function getOrganizationExclude(pid) {
  return request({
    url: `/api/identity/organizations/list/exclude/${pid}`,
    method: 'get'
  })
}

export function getOrganizationtTree() {
  return request({
    url: `/api/identity/organizations/tree`,
    method: 'get'
  })
}


export function createOrganization(payload) {
  return request({
    url: '/api/identity/organizations',
    method: 'post',
    data: payload
  })
}

export function updateOrganization(payload) {
  return request({
    url: `/api/identity/organizations`,
    method: 'put',
    data: payload
  })
}

export function deleteOrganization(id) {
  return request({
    url: `/api/identity/organizations/${id}`,
    method: 'delete'
  })
}
