import { createRole, deleteRole, updateRole} from "@/api/system/role";
import { getAssignableRoles} from "@/api/system/user";
  const useRoleStore = defineStore("role", {
    state: () => ({
      roles: [],
    }),
    actions: {
      modifyRole(data) {
          return new Promise((resolve, reject) => {
            updateRole(data)
          .then((res) => {
            this.getRoles();
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      })},
  
      addRole(data) {
          return new Promise((resolve, reject) => {
            createRole(data)
          .then((res) => {
            this.getRoles();
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      })},
  
      removeRole(id) {
          return new Promise((resolve, reject) => {
            deleteRole(id)
          .then((res) => {
            this.getRoles();
            resolve(res);
          })
          .catch((error) => {
            reject(error);
          });
      })},

      getRoles(){
        return new Promise((resolve, reject) => {
            getAssignableRoles().then(res => {
                this.roles = res.items;
                resolve(res)
              })
          .catch((error) => {
            reject(error);
          });
      })},

    },
  });
  
  export default useRoleStore;
  