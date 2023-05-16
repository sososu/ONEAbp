import request from '@/utils/request'

export function exchangeToken(data) {
  return request({
    url: '/connect/token',
    method: 'post',
    data: data,
    headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
  })
}

