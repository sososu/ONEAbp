import request from '@/utils/request'

//获取路由
export function getRouters(code) {
  return request({
    url: `/api/routers?appCode=${code}`,
    method: 'get'
  })
}

//获取选择角色应用
export function getSelectRoleApps(roleId) {
  return request({
    url: `/api/select/role/apps?roleId=${roleId}`,
    method: 'get'
  })
}

//获取选择角色菜单树
export function getSelectRoleMenuTree(appId,roleId) {
  return request({
    url: `/api/select/role/menu-tree?appId=${appId}&roleId=${roleId}`,
    method: 'get',
  })
}

//角色授权
export function grant(data) {
  return request({
    url: `/api/select/role/grant`,
    method: 'post',
    data:data
  })
}

// 获取权限字符
export function getPerms(query) {
  return request({
    url: '/api/perms/search',
    method: 'get',
    params:query
  })
}