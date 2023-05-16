import request from '@/utils/request'

export function getUsers(query) {
  return request({
    url: '/api/identity/users/page',
    method: 'get',
    params: query
  })
}

export function getUserById(id) {
  return request({
    url: `/api/identity/users/${id}`,
    method: 'get'
  })
}

export function createUser(payload) {
  return request({
    url: '/api/identity/users',
    method: 'post',
    data: payload
  })
}

export function createUserToOrg(payload) {
  return request({
    url: '/api/identity/users/create-to-organizations',
    method: 'post',
    data: payload
  })
}

export function updateUser(payload) {
  return request({
    url: `/api/identity/users/${payload.id}`,
    method: 'put',
    data: payload
  })
}

export function resetPassword(id) {
  return request({
    url: `/api/identity/users/${id}/reset-password`,
    method: 'put'
  })
}

export function updateUserToOrg(payload) {
  return request({
    url: `/api/identity/users/${payload.id}/update-to-organizations`,
    method: 'put',
    data: payload
  })
}

export function deleteUser(id) {
  return request({
    url: `/api/identity/users/${id}`,
    method: 'delete'
  })
}

export function getRolesByUserId(id) {
  return request({
    url: `/api/identity/users/${id}/roles`,
    method: 'get'
  })
}

export function getAssignableRoles() {
  return request({
    url: '/api/identity/users/assignable-roles',
    method: 'get'
  })
}

export function getOrganizationsByUserId(id, includeDetails = false) {
  return request({
    url: `/api/identity/users/${id}/organizations`,
    method: 'get',
    params: includeDetails
  })
}

/**
 * 添加成员到组织单元中
 * @param {string} id
 * @param {Array} ouId
 */
export function addToOrganization(id, ouIds) {
  return request({
    url: `/api/identity/users/${id}/add-to-organizations`,
    method: 'post',
    data: ouIds
  })
}

/*account profile*/
/*待修改的*/

/*更新头像*/
export function uploadAvatar(data){
  return request({
    url: `/api/account/my-profile/upload-avatar`,
    method: 'post',
    data: data
  })
}


/*修改自己密码*/
export function updateUserPwd(oldPassword,newPassword){
  return request({
    url: `/api/account/my-profile/change-password`,
    method: 'post',
    data: {'currentPassword':oldPassword,'newPassword':newPassword}
  })
}



/*更新个人信息*/
export function updateUserProfile(data){
  return request({
    url: `/api/account/my-profile`,
    method: 'put',
    data: data
  })
}




/*获取个人信息*/
export function getUserProfile(){
  return request({
    url: `/api/account/my-profile`,
    method: 'get'
  })
}