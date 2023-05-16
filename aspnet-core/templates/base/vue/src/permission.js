import router from './router'
import { ElMessage } from 'element-plus'
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'
import { getToken } from '@/utils/auth'
import { isHttp } from '@/utils/validate'
import { isRelogin } from '@/utils/request'
import useUserStore from '@/store/modules/user'
import useSettingsStore from '@/store/modules/settings'
import usePermissionStore from '@/store/modules/permission'
import { useOidcLogin } from '@/libs/oidcClientConfig'
import { getUrlParam } from '@/utils/toolFn'
import { cancelAllPening} from "@/libs/axiosPending"
import useAppStore from '@/store/modules/app'
NProgress.configure({ showSpinner: false });

const whiteList = ['/login', '/auth-redirect', '/bind', '/register', '/callback', '/error', '/logout','/consent'];

router.beforeEach(async(to, from, next) => {
  NProgress.start()
  const userStore = useUserStore()
  const appStore = useAppStore()
  const permissionStore = usePermissionStore()
  let abpConfig=appStore.abpConfig??await appStore.setApplicationConfiguration()
      
  if (abpConfig.currentUser.isAuthenticated) {
        if (to.path === '/login') {
          // if is logged in, redirect to the home page
          next({ path: '/' })
          NProgress.done() // hack: https://github.com/PanJiaChen/vue-element-admin/pull/2939
        } else {
          
          const userName=userStore.userName
          
          if(userName){
            next()
          }
          else{
             try{
              // get user info
              await userStore.getInfo()
              userStore.setRole( abpConfig.currentUser.roles)
            
              let keys=Object.keys(abpConfig.auth.grantedPolicies)
              userStore.setPermission(keys)
          
              let accessRoutes=await permissionStore.generateRoutes();
                accessRoutes.forEach(route => {
                  if (!isHttp(route.path)) {
                    router.addRoute(route) // 动态添加可访问路由表
                  }
                })
                next({ ...to, replace: true }) // hack方法 确保addRoutes已完成
               }catch(error){
                ElMessage.error(error)
                NProgress.done()
              }
            }
        }


  } else {
    // 没有token
    if (whiteList.indexOf(to.path) !== -1) {
      // 在免登录白名单，直接进入
      next()
    } else {
      useOidcLogin();
      NProgress.done()
    }
  }
})

router.afterEach(() => {
  NProgress.done()
})
