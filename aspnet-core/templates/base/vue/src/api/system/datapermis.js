import request from '@/utils/request'
import { transformAbpListQuery } from '@/utils/abp'

//userRule

export function listUserRules(query) {
  return request({
    url: '/api/data-permission/user-rule/page',
    method: 'get',
    params: query
  })
}

export function getUserRule(id) {
  return request({
    url: `/api/data-permission/user-rule/${id}`,
    method: 'get'
  })
}

export function createUserRule(payload) {
  return request({
    url: '/api/data-permission/user-rule',
    method: 'post',
    data: payload
  })
}

export function updateUserRule(payload) {
  return request({
    url: `/api/data-permission/user-rule/${payload.id}`,
    method: 'put',
    data: payload
  })
}

export function deleteUserRule(id) {
  return request({
    url: `/api/data-permission/user-rule/${id}`,
    method: 'delete'
  })
}

export function getUserRuleKv() {
    return request({
      url: '/api/data-permission/user-rule/kv',
      method: 'get',
    })
  }


  export function getUserTargetFields() {
    return request({
        url: '/api/data-permission/user-rule/fields',
        method: 'get',
      })
  }

//dataRule

export function listDataRules(query) {
    return request({
      url: '/api/data-permission/data-rule/page',
      method: 'get',
      params: query
    })
  }
  
  export function getDataRule(id) {
    return request({
      url: `/api/data-permission/data-rule/${id}`,
      method: 'get'
    })
  }
  
  export function createDataRule(payload) {
    return request({
      url: '/api/data-permission/data-rule',
      method: 'post',
      data: payload
    })
  }
  
  export function updateDataRule(payload) {
    return request({
      url: `/api/data-permission/data-rule/${payload.id}`,
      method: 'put',
      data: payload
    })
  }
  
  export function deleteDataRule(id) {
    return request({
      url: `/api/data-permission/data-rule/${id}`,
      method: 'delete'
    })
  }

  export function getDataRuleKv(query) {
    return request({
      url: `/api/data-permission/data-rule/kv`,
      method: 'get',
      params:query
    })
  }

  export function getPredefineFieldValues() {
    return request({
      url: `/api/data-permission/data-rule/define-value`,
      method: 'get'
    })
  }

  //Rule

export function listRules(query) {
    return request({
      url: '/api/data-permission/rule/page',
      method: 'get',
      params: query
    })
  }
  
  export function getRule(id) {
    return request({
      url: `/api/data-permission/rule/${id}`,
      method: 'get'
    })
  }
  
  export function createRule(payload) {
    return request({
      url: '/api/data-permission/rule',
      method: 'post',
      data: payload
    })
  }
  
  export function updateRule(payload) {
    return request({
      url: `/api/data-permission/rule/${payload.id}`,
      method: 'put',
      data: payload
    })
  }
  
  export function deleteRule(id) {
    return request({
      url: `/api/data-permission/rule/${id}`,
      method: 'delete'
    })
  }


  export function setEnable(id,payload) {
    return request({
      url: `/api/data-permission/rule/${id}/enable`,
      method: 'put',
      data: payload
    })
  }

  export function getRuleByTargetName(query) {
    return request({
      url: `/api/data-permission/rule/by-targetname`,
      method: 'get',
      params:query
    })
  }



  //DataTarget

  export function getPageTargets(query) {
    return request({
      url: '/api/data-permission/target/page',
      method: 'get',
      params: query
    })
  }


  export function listTargets() {
    return request({
      url: '/api/data-permission/target/list',
      method: 'get'
    })
  }

  export function getTarget(name) {
    return request({
      url: `/api/data-permission/target/${name}`,
      method: 'get',
    })
  }

  //orderdemo
  export function listOrder(query) {
    return request({
      url: '/api/order-demo/page',
      method: 'get',
      params: query
    })
  }

