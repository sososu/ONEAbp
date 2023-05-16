import request from '@/utils/request'

// 登录方法
export function login(username, password, code, uuid) {
  const data = {
    username,
    password,
    code,
    uuid
  }
  return request({
    url: '/login',
    headers: {
      isToken: false
    },
    method: 'post',
    data: data
  })
}

// 注册方法
export function register(data) {
  return request({
    url: '/register',
    headers: {
      isToken: false
    },
    method: 'post',
    data: data
  })
}

// 获取用户详细信息
export function getInfo(data) {
  return request({
    url: '/api/sys/getInfo',
    method: 'get',
    params:data,
  })
}

// 退出方法
export function logout() {
  return request({
    url: '/logout',
    method: 'post'
  })
}

// 获取验证码
export function getCodeImg() {
  return request({
    url: '/captchaImage',
    headers: {
      isToken: false
    },
    method: 'get',
    timeout: 20000
  })
}


// 获取租户
export function currentTenant() {
  return request({
    url: '/api/account/tenants/current-tenant/name',
    headers: {
      isToken: false
    },
    method: 'get',
    timeout: 20000
  })
}


 // 租户提交
export function submitTenant(data) {
  return request({
    url: '/api/account/tenants/current-tenant/submit',
    headers: {
      isToken: false
    },
    method: 'post',
    data: data
  })
}


// 用户登录
 export function userLogin(data) {
  return request({
    url: '/api/account/login',
    headers: {
      isToken: false
    },
    method: 'post',
    data: data
  })
}


// 用户登出
export function userLogout(data) {
  return request({
    url: '/api/account/logout',
    headers: {
      isToken: false
    },
    method: 'post',
    data: data
  })
}


// 登录失败
export function loginError(query) {
  return request({
    url: '/api/account/error',
    headers: {
      isToken: false
    },
    method: 'get',
    params: query
  })
}

// 授权同意
export function userConsent(data) {
  return request({
    url: '/api/account/consent',
    headers: {
      isToken: false
    },
    method: 'post',
    data: data
  })
}


// 授权同意
export function userConsentSubmit(data) {
  return request({
    url: '/api/account/consent/submit',
    headers: {
      isToken: false
    },
    method: 'post',
    data: data
  })
}


// 租户登陆
export function loginTenantConenct(data) {
  return request({
    url: '/connect/token',
    headers: {
      isToken: false,
      "Content-Type":"application/x-www-form-urlencoded"
    },
    method: 'post',
    data: data
  })
}