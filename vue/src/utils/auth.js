import Cookies from 'js-cookie'
import useAppStore from '@/store/modules/app'
const TokenKey = 'Admin-Token'
const TenantKey = 'Tenant-Key'


export function getToken() {
  return Cookies.get(TokenKey)
}

export async function setToken(token) {
  const result = Cookies.set(TokenKey, token)
  const appStore = useAppStore()
  await appStore.setApplicationConfiguration()
  return result
}

export async function removeToken() {
  const result = Cookies.remove(TokenKey)
  const appStore = useAppStore()
  await appStore.setApplicationConfiguration()
  return result
}


export async function setTenant(tenant) {
  const result = Cookies.set(TenantKey,tenant)
  return result
}

export function getTenant() {
  return Cookies.get(TenantKey)
}