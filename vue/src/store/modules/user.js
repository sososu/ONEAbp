import { getInfo,logout} from '@/api/user'
import { exchangeToken} from '@/api/extensionGrant'
import { getToken, setToken, removeToken,setTenant } from '@/utils/auth'
import defAva from '@/assets/images/profile.jpg'
import { callBack, useOidcLogout } from "@/libs/oidcClientConfig";
import { use } from 'echarts';
import {
  getFile
} from "@/api/file-management/file";
const useUserStore = defineStore(
  'user',
  {
    state: () => ({
      token: getToken(),
      name: '',
      userName: '',
      avatar: '',
      email: '',
      phoneNumber: '',
      sex:'',
      roles: [],
      permissions:[]
    }),
    actions: {
     exchangeLogin(tenantId){
      return new Promise((resolve, reject) => {
        const params = new URLSearchParams() // 创建对象
        params.append('grant_type', "impersonation")
        params.append('access_token', this.token) 
        params.append('client_id', "Admin_App") 
      if(tenantId){
        params.append('tenantid', tenantId) 
      }
       
        exchangeToken(params)
    .then(res => {
      setToken(res.access_token)
      this.token = res.access_token
      console.log("token:"+this.token)
      setTenant(tenantId)
      resolve()
    })
    .catch(error => {
      reject(error)
    });
      });
     },

    login(){
      return new Promise((resolve, reject) => {
        callBack()
    .then(res => {
      setToken(res.access_token)
      this.token = res.access_token
      console.log("token:"+this.token)
      resolve()
    })
    .catch(error => {
      reject(error)
    });
      });
      },

      // 获取用户信息
      getInfo() {
        return new Promise((resolve, reject) => {
          //{'appCode':import.meta.env.VITE_APP_Code}
          getInfo().then(res => {
            const user = res
            const avatar = (user.avatar == "" || user.avatar == null) ? defAva : import.meta.env.VITE_APP_BASE_API + user.avatar
            this.name = user.name
            this.avatar = avatar
            this.userName=user.userName
            this.email=user.email
            this.phoneNumber=user.phoneNumber
            this.sex=user.sex

            resolve(res)
          }).catch(error => {
            reject(error)
          })
        })
      },
      
      // 退出系统
      logOut() {
        return new Promise((resolve, reject) => {
          logout().then(() => {
            this.token = ''
            this.roles = []
            this.permissions = []
            removeToken()
            resolve()
          }).catch(error => {
            reject(error)
          })
        })
      },

      setRole(roles){
        this.roles=roles
      },
      setPermission(permissions){
        this.permissions=permissions
      }
    }
  })

export default useUserStore
