import {
    createTenant,
    updateTenant,
    deleteTenant,
    getTenants,
    updateTenantSaleVersion
} from "@/api/sass/tenant";
  
  const useTenantStore = defineStore("Tenant", {
    state: () => ({
      deptTree: [],
    }),
    actions: {
      modifyTenant(data) {
          return new Promise((resolve, reject) => {
            updateTenant(data)
          .then((res) => {
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      })},
  
      addTenant(data) {
          return new Promise((resolve, reject) => {
        createTenant(data)
          .then((res) => {
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      })},
  
      removeTenant(id) {
          return new Promise((resolve, reject) => {
            deleteTenant(id)
          .then((res) => {
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      })},
  
      // listTenantt() {
      //     return new Promise((resolve, reject) => {
      //   getTenanttTree()
      //     .then((res) => {
      //       this.deptTree = res.items;
      //       resolve(res);
      //     })
      //     .catch((error) => {
      //       reject(error);
      //     });
      // })}
    },
  });
  
  export default useTenantStore;
  