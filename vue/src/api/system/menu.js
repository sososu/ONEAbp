import request from '@/utils/request'

// 分页查询菜单列表
export function listMenu(query) {
  return request({
    url: '/api/sys-resource/menu/page',
    method: 'get',
    params: query
  })
}

// 查询菜单详细
export function getMenu(id) {
  return request({
    url: `/api/sys-resource/menu/${id}`,
    method: 'get'
  })
}


// 新增菜单
export function addMenu(data) {
  return request({
    url: '/api/sys-resource/menu',
    method: 'post',
    data: data
  })
}

// 修改菜单
export function updateMenu(id,data) {
  return request({
    url: `/api/sys-resource/menu/${id}`,
    method: 'put',
    data: data
  })
}

// 删除菜单
export function delMenu(id) {
  return request({
    url: '/api/sys-resource/menu/' + id,
    method: 'delete'
  })
}


