import Cookies from 'js-cookie'
import { applicationConfiguration, tenantsByName } from '@/api/abp'

const useAppStore = defineStore(
  'app',
  {
    state: () => ({
      sidebar: {
        opened: Cookies.get('sidebarStatus') ? !!+Cookies.get('sidebarStatus') : true,
        withoutAnimation: false,
        hide: false
      },
      device: 'desktop',
      size: Cookies.get('size') || 'default',
      abpConfig: null,
      tenant: Cookies.get('tenant')
    }),
    actions: {
      toggleSideBar(withoutAnimation) {
        if (this.sidebar.hide) {
          return false;
        }
        this.sidebar.opened = !this.sidebar.opened
        this.sidebar.withoutAnimation = withoutAnimation
        if (this.sidebar.opened) {
          Cookies.set('sidebarStatus', 1)
        } else {
          Cookies.set('sidebarStatus', 0)
        }
      },
      closeSideBar({ withoutAnimation }) {
        Cookies.set('sidebarStatus', 0)
        this.sidebar.opened = false
        this.sidebar.withoutAnimation = withoutAnimation
      },
      toggleDevice(device) {
        this.device = device
      },
      setSize(size) {
        this.size = size;
        Cookies.set('size', size)
      },
      toggleSideBarHide(status) {
        this.sidebar.hide = status
      },

      setApplicationConfiguration() {
        return new Promise((resolve, reject) => {
           applicationConfiguration()
            .then(response => {
              this.abpConfig=response;  
              resolve(response)
            })
            .catch(error => {
              reject(error)
            })
        })
      },
      setTenant(name) {
        return new Promise((resolve, reject) => {
          if (!name) {
            this.tenant=''
            this.applicationConfiguration().then(() => {
              resolve()
            })
            return
          }
          tenantsByName(name)
            .then(response => {
              if (response.success) {
                this.tenant= response.tenantId
                this.applicationConfiguration().then(() => {
                  resolve(response)
                })
                return
              }
    
              resolve(response)
            })
            .catch(error => {
              reject(error)
            })
        })
      }
    }
  })

export default useAppStore
