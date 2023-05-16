import request from '@/utils/request'

export function getAccountSetting() {
  return request({
    url: '/api/account/setting',
    method: 'get'
  })
}

export function setAccountSetting(values) {
  return request({
    url: '/api/account/setting',
    method: 'put',
    data: values
  })
}


export function getIdentitySetting() {
  return request({
    url: '/api/identity/setting',
    method: 'get'
  })
}

export function setIdentitySetting(values) {
  return request({
    url: '/api/identity/setting',
    method: 'put',
    data: values
  })
}


export function getFileSetting() {
  return request({
    url: '/api/file/setting',
    method: 'get'
  })
}

export function setFileSetting(values) {
  return request({
    url: '/api/file/setting',
    method: 'put',
    data: values
  })
}

