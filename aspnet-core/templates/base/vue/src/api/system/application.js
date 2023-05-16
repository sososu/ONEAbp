import request from '@/utils/request'

//添加应用
export function addApp(data) {
  return request({
    url: '/api/sys-resource/app',
    method: 'post',
    data:data
  })
}



//更新应用
export function updateApp(id,data) {
    return request({
      url: `/api/sys-resource/app/${id}`,
      method: 'put',
      data:data
    })
  }


  //删除应用
export function deleteApp(id) {
    return request({
      url: '/api/sys-resource/app/' + id,
      method: 'delete'
    })
  }

  //获取应用
  export function getPage(data) {
    return request({
      url: '/api/sys-resource/app/page',
      method: 'get',
      params:data
    })
  }