import request from '@/utils/request'

export function getSaleVersions(query) {
  return request({
    url: '/api/sys-resource/sale-version/page',
    method: 'get',
    params: query
  })
}

export function getSaleVersionById(id) {
  return request({
    url: `/api/sys-resource/sale-version/${id}`,
    method: 'get'
  })
}

export function createSaleVersion(payload) {
  return request({
    url: '/api/sys-resource/sale-version',
    method: 'post',
    data: payload
  })
}

export function updateSaleVersion(id,payload) {
  return request({
    url: `/api/sys-resource/sale-version/${id}`,
    method: 'put',
    data: payload
  })
}

export function deleteSaleVersion(id) {
  return request({
    url: `/api/sys-resource/sale-version/${id}`,
    method: 'delete'
  })
}

export function getSaleVersionApps(id) {
    return request({
      url: `/api/sys-resource/sale-version/${id}/apps`,
      method: 'get'
    })
  }

export function getSaleVersionMenus(id,appId) {
    return request({
      url: `/api/sys-resource/sale-version/${id}/menus?appId=${appId}`,
      method: 'get'
    })
  }

  export function SetSaleVersionMenus(id,data) {
    return request({
      url: `/api/sys-resource/sale-version/${id}/menus`,
      method: 'put',
      data:data
    })
  }