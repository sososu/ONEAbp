import request from '@/utils/request'

export function getInfo() {
  return request({
    url: '/api/account/my-profile',
    method: 'get'
  })
}

export function logout() {
  return request({
    url: '/api/account/logout',
    method: 'get'
  })
}

export function register(data) {
  return request({
    url: '/api/account/register',
    method: 'post',
    data: data
  })
}

export function setUserInfo(data) {
  return request({
    url: '/api/account/my-profile',
    method: 'put',
    data: data
  })
}

export function changePassword(data) {
  return request({
    url: '/api/account/my-account/change-password',
    method: 'post',
    data: data
  })
}
