import {
  addApp,
  updateApp,
  deleteApp,
  getPage,
} from "@/api/system/application";


const useApplicationStore = defineStore("application", {
  state: () => ({
    deptTree: [],
  }),
  actions: {
    modifyApplication(id,data) {
        return new Promise((resolve, reject) => {
          updateApp(id,data)
        .then((res) => {
          resolve(res);
        })
        .catch((error) => {
          reject(error);
        });
    })},

    addApplication(data) {
        return new Promise((resolve, reject) => {
      addApp(data)
        .then((res) => {
          resolve(res);
        })
        .catch((error) => {
          reject(error);
        });
    })},

    removeApplication(id) {
        return new Promise((resolve, reject) => {
          deleteApp(id)
        .then((res) => {
          resolve(res);
        })
        .catch((error) => {
          reject(error);
        });
    })},

    // listApplicationt() {
    //     return new Promise((resolve, reject) => {
    //   getApplicationtTree()
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

export default useApplicationStore;
