import request from '@/utils/request'
import { transformAbpListQuery } from '@/utils/abp'

export function deleteDictionary(id) {
  return request({
    url: `/api/data-dictionary/datas/${id}`,
    method: 'delete'
  })
}

export function createDataDictionaryCategory(payload) {
  return request({
    url: '/api/data-dictionary/datas/category',
    method: 'post',
    data: payload
  })
}

export function updateDataDictionaryCategory(id,payload) {
  return request({
    url: `/api/data-dictionary/datas/category/${id}`,
    method: 'put',
    data: payload
  })
}

export function getDataDictionaryCategorys() {
  return request({
    url: '/api/data-dictionary/datas/categorys',
    method: 'get'
  })
}

// details
export function createDataDictionaryItem(payload) {
  return request({
    url: '/api/data-dictionary/datas/item',
    method: 'post',
    data: payload
  })
}

export function updateDataDictionaryItem(id,payload) {
  return request({
    url: `/api/data-dictionary/datas/item/${id}`,
    method: 'put',
    data: payload
  })
}

export function getDataDictionaryItems(query) {
  return request({
    url: '/api/data-dictionary/datas/items',
    method: 'get',
    params:query
  })
}


export function getDataDictionaryItemsByCode(query) {
  return request({
    url: '/api/data-dictionary/datas/items/bycode',
    method: 'get',
    params:query
  })
}