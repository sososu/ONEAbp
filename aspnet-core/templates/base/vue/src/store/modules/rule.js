import {
    getUserRuleKv,
    getDataRuleKv,
} from "@/api/system/datapermis";
const useUserStore = defineStore(
  'rule',
  {
    state: () => ({
      userRules: [],
    //   allDataRules:[]
    }),
    actions: {

      getUserRules() {
        return new Promise((resolve, reject) => {
            getUserRuleKv().then(res => {
                this.userRules= res.items;
                resolve(res)
            }).catch(error => {
            reject(error)
          })
        })
      },

    //    getDataRules(dataTarget) {
    //         return new Promise((resolve, reject) => {
    //             getDataRuleKv(dataTarget).then(res => {
    //                 let delete_index=-1;
    //                 for (let index = 0; index < this.allDataRules.length; index++) {
    //                     if(this.allDataRules[index].key==dataTarget){
    //                         delete_index=index;
    //                         break;
    //                      }
    //                 }

    //                 if(delete_index>-1){
    //                    this.allDataRules.splice(delete_index, 1)
    //                 }

    //                 this.allDataRules.push({key:dataTarget,value:res.items})
    //                 resolve(res)
    //             }).catch(error => {
    //             reject(error)
    //           })
    //         })
    //       },
      
    }
  })

export default useUserStore
