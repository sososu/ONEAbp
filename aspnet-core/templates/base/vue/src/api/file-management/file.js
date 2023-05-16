import request from '@/utils/request'
import {download} from '@/utils/request'




// export function createFile(data) {
//   return request({
//     url: '/api/file-management/files/upload',
//     method: 'post',
//     data: data
//   })
// }

// export function getFileByName(name) {
//   return request({
//     url: `/api/file-management/files/${name}`,
//     method: 'get',
//     responseType: 'arraybuffer'
//   })
// }



export function createFile(data) {
  return request({
    url: '/api/file-management/file',
    method: 'post',
    data: data
  })
}

export function deleteFile(name) {
  return request({
    url: `/api/file-management/file/${name}`,
    method: 'delete',
  })
}

export function getFile(name) {
  return request({
    url: `/api/file-management/file/${name}`,
    method: 'get',
  })
}

export function getFileStream(name) {
  return request({
    url: `/api/file-management/file/stream/${name}`,
    method: 'get',
  })
}


export function getFilePage(data) {
  return request({
    url: `/api/file-management/file/page`,
    method: 'get',
    params:data
  })
}


export function getStatistics() {
  return request({
    url: `/api/file-management/file/statistics`,
    method: 'get'
  })
}


export function downloadFile(name,filename) {
  return download(`/api/file-management/file/download?name=${name}`,{},filename)
}